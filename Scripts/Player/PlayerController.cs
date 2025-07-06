using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Quoridor.Command;
using Quoridor.Board;
using Quoridor.Player.Skill;
using Quoridor.Player.Status;

namespace Quoridor.Player
{
    public class PlayerController : IPlayer
    {
        // Unityメインスレッド
        private static SynchronizationContext s_mainThreadContext;

        // 外部依存
        private IBoard _board;
        private int _turnIndex;
        private IPlayer _opponent;
        private IMediator _mediator;
        private ICommandInvoker _commandInvoker;

        // CPU
        private CPUController _cpuController;

        // プレイヤーの状態
        private ModifiableBool _isAuto; // 操作モード（True:自動 false:手動）
        private bool _canMove; // 行動の可否
        private List<ISkill> _skillSlots; // スキル固定長スロット
        public List<IStatus> _waitingStatusList; // 待機中ステータス（バフ・デバフ等）
        public List<IStatus> _activeStatusList; // 発動中ステータス（バフ・デバフ等）
        private HashSet<string> _sealedSkillSet; // 使用できないスキル一覧

        // Getter ※IPlayerにより外部利用可
        public int TurnIndex => _turnIndex;
        public int RemainWalls => (_skillSlots.Count() > 1) ? _skillSlots[1].Charge : 0;
        public List<ISkill> SkillSlots => _skillSlots;
        public List<IStatus> ActiveStatusList => _activeStatusList.Concat(_waitingStatusList).ToList();

        // インスタンス化～PlayerConfigセット関連
        private PlayerController() { }
        public PlayerController(IBoard board, int turnIndex)
        {
            s_mainThreadContext = SynchronizationContext.Current;
            _board = board;
            _turnIndex = turnIndex;
            _canMove = true;
            _skillSlots = new List<ISkill>();
            _waitingStatusList = new List<IStatus>();
            _activeStatusList = new List<IStatus>();
            _sealedSkillSet = new HashSet<string>();
        }

        public void SetExternalInfo(IPlayer opponent, ICommandInvoker commandInvoker)
        {
            _opponent = opponent;
            _commandInvoker = commandInvoker;
        }

        public void SetMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void SetConfig(PlayerConfigSO config, List<SkillDefinitionSO> skillDefinitions)
        {
            _canMove = true;
            _isAuto = new ModifiableBool(config.pIsAuto);

            _skillSlots.Clear();
            SkillFactory skillFactory = new SkillFactory(this, _opponent, _commandInvoker);
            skillFactory.SetSkillSlots(_skillSlots, skillDefinitions);

            // ステータスクリア
            _waitingStatusList.Clear();
            _activeStatusList.Clear();
            _sealedSkillSet.Clear();

            _cpuController = new CPUController(config, _board, this, _opponent);

            ExecutePassiveSkill();
            _mediator.Notify(this, GameEvent.UPDATE, null);
        }

        private void ExecutePassiveSkill()
        {
            foreach (ISkill skill in _skillSlots.Where(
                skill => skill != null && skill.TagSet.Contains(MainTags.Passive)
            )) skill.Execute();
            _mediator.Notify(this, GameEvent.UPDATE, null);
        }


        // TurnManager通知時（有効ステータス適用～操作待機orCPU呼出）
        public void OnTurnStart()
        {
            CountSkillTurn();
            ApplyActiveStatuses();

            if (_canMove)
            {
                ApplySealedSkill();
                SetNormalSkill();
                if (_isAuto.OrBool) BootCPU();
            }
            else _mediator.Notify(this, GameEvent.TURN_END, null);
        }

        private void CountSkillTurn()
        {
            _commandInvoker.ExecuteCommand(new SkillsCountTurnCommand(_skillSlots));
            _mediator.Notify(this, GameEvent.UPDATE, null);
        }

        private void ApplyActiveStatuses()
        {
            ICommand command = new ApplyStatusCommand(this, _waitingStatusList, _activeStatusList);
            _commandInvoker.ExecuteCommand(command);
            _mediator.Notify(this, GameEvent.UPDATE, null);
        }

        private void ApplySealedSkill()
        {
            foreach (ISkill skill in _skillSlots)
            {
                if (skill == null) continue;
                if (_sealedSkillSet.Any(tag => skill.TagSet.Contains(tag)))
                {
                    skill?.SetState(SkillState.SEALED);
                }
                else skill?.Release();
            }
        }

        private void SetNormalSkill()
        {
            // 0, 1がノーマルスキル
            for (int i = 0; i < 2; i++) _skillSlots[i].Execute();
            _mediator.Notify(this, GameEvent.UPDATE, null);
        }

        private async void BootCPU()
        {
            await Task.Run(() => _cpuController.SearchBestMove());
            s_mainThreadContext.Post(_ =>
            {
                if (_canMove) _cpuController.InstructMove();
                _mediator.Notify(this, GameEvent.TURN_END, null);
            }, null);
        }

        // ユーザー操作
        public bool JudgeSelectableMove(Pos move)
        {
            if (_isAuto.OrBool) return false;
            foreach (BoardSkill skill in _skillSlots.OfType<BoardSkill>())
            {
                if (skill.GetValidMoves(_board).Contains(move)) return true;
            }
            return false;
        }

        public void OnInstructUseSkill(int index)
        {
            if (_isAuto.OrBool) return;

            index += 2;
            ISkill targetSkill = _skillSlots[index];

            if (!SkillUtil.CanUse(targetSkill)) return;

            // キャンセル操作（通常スキルに戻す）
            if (targetSkill.State == SkillState.SELECTED)
            {
                targetSkill.SetState(SkillState.READY);
                SetNormalSkill();
                return;
            }

            // スキル実行
            if (targetSkill is BoardSkill)
            {
                // Boardスキル → 他スキルをREADYに
                foreach (ISkill skill in _skillSlots)
                {
                    skill.SetState(SkillState.READY);
                }
            }
            else
            {
                // Instanceスキル → 通常スキルに戻す + 特殊スキルをCOOLDOWNに
                SetNormalSkill();
                for (int i = 2; i < _skillSlots.Count; i++)
                {
                    _skillSlots[i].SetState(SkillState.COOLDOWN);
                }
            }

            _skillSlots[index].Execute();
            _mediator.Notify(this, GameEvent.UPDATE, null);
        }

        public void OnInstructMove(Pos move)
        {
            if (_isAuto.OrBool) return;

            foreach (BoardSkill skill in _skillSlots.OfType<BoardSkill>())
            {
                if (skill.GetValidMoves(_board).Contains(move))
                {
                    skill.Execute(_board, move);
                    _mediator.Notify(this, GameEvent.TURN_END, null);
                    break;
                }
            }
        }


        // メンバ変数変更（ステータス適用時等）
        public void AddActiveStatus(IStatus status) => _waitingStatusList.Add(status);
        public void RemoveActiveStatus(IStatus status)
        {
            _waitingStatusList.Remove(status);
            _activeStatusList.Remove(status);
        }
        public void SwitchAutoManualMode(bool isAuto) => _isAuto.pModifier = isAuto;
        public void SwitchMovable(bool canMove) => _canMove = canMove;
        public void AddSealedSkillSet(string skillTag) => _sealedSkillSet.Add(skillTag);
        public void RemoveSealedSkillSet(string skillTag) => _sealedSkillSet.Remove(skillTag);
        public void CorrectCpuLevel(int relative, int absolute)
        => _cpuController.SetCpuLevel(relative, absolute); // 強さ計算式 : Min(base + relative, absolute)

        public void OnGameEnd()
        {
            _canMove = false;
            foreach(ISkill skill in _skillSlots)
            {
                skill.SetState(SkillState.COOLDOWN);
            }
        }
    }
}