using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;
using Quoridor.Board;
using Quoridor.UI;

namespace Quoridor{
    public class UtageFacade : Facade, IColleague
    {
        public static UtageFacade p_sInstance;
        [SerializeField] private AdvEngine _engine;
        public bool IsPlaying { get; private set; }
        private UIFacade UIFacadeRef => ConcreteUIFacade.p_sInstance;

        private void Awake()
        {
            if (p_sInstance == null)
            {
                p_sInstance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public void OnDoCommand(AdvCommandSendMessage command)
        {
            switch (command.Name)
            {
                case "GameReady":
                    _director.CreateMatch();
                    InformGameReady(
                        command.Arg2
                        , Convert.ToInt32(command.Arg3)
                    );
                    break;

                case "GameStart":
                    UIFacadeRef?.SetRaycastEnabled(true);
                    MediatorRef?.Notify(this, GameEvent.START, null);
                    break;

                case "ShowPawn1":
                    UIFacadeRef?.ShowPawn1();
                    break;

                case "ShowPawn2":
                    UIFacadeRef?.ShowPawn2();
                    break;

                case "ShowBoard":
                    UIFacadeRef?.ShowBoard();
                    break;

                case "SwitchAutoMode":
                    PlayersRef[1].SwitchAutoManualMode(true);
                    break;

                case "Move":
                    int turnIndex = int.Parse(command.Arg4);
                    PlayersRef[turnIndex].OnTurnStart();
                    PlayersRef[turnIndex].OnInstructMove(
                        new Pos(int.Parse(command.Arg2), int.Parse(command.Arg3))
                    );
                    _engine?.JumpScenario(command.Arg5);
                    break;

                default:
                    Debug.Log("Unknown Message:" + command.Name);
                    break;
            }
        }

        public void JumpScenario(string label)
        => StartCoroutine(JumpScenarioAsync(label, null));

        public void JumpScenario(string label, Action onComplete)
        => StartCoroutine(JumpScenarioAsync(label, onComplete));

        IEnumerator JumpScenarioAsync(string label, Action onComplete)
        {
            IsPlaying = true;
            _engine?.JumpScenario(label);
            while (!_engine.IsEndOrPauseScenario)
            {
                yield return null;
            }
            IsPlaying = false;
            if (onComplete != null) onComplete();
        }

        public void SetParameter(Dictionary<string, object> dic)
        {
            foreach (string param in dic.Keys) SetParameter(param, dic[param]);
        }

        public void SetParameter(string label, object value)
        {
            if (value is string) SetParameterString(label, Convert.ToString(value));
            else if (value is bool) SetParameterString(label, Convert.ToBoolean(value));
            else if (value is int) SetParameterString(label, Convert.ToInt32(value));
        }

        public void SetParameterString(string label, string value)
        => _engine.Param.SetParameterString(label, value);
        public void SetParameterString(string label, bool value)
        => _engine.Param.SetParameterBoolean(label, value);
        public void SetParameterString(string label, int value)
        => _engine.Param.SetParameterInt(label, value);

        public void InformGameReady(string cpuName, int turnIndex)
        {
            StartConfig config = new StartConfig();
            config.SetPlayerConfigs(
                StartConfig.LoadPlayerConfig("HumanPlayer")
                , StartConfig.LoadPlayerConfig(cpuName)
            );
            config.AddSkillDefinitions(StartConfig.LoadSkillDefinitions(new()));
            config.SetFirstTurnIndex(turnIndex);

            MediatorRef?.Notify(this, GameEvent.READY, config);
        }

        public void OnGameEnd(RESULT result)
        {
            SetParameter("isGameOver", result == RESULT.P2_WIN);
            UIFacadeRef?.SetActiveCamera(false);
            UIFacadeRef?.SetRaycastEnabled(false);
            JumpScenario("QuoridorEnd");
        }

        public void OnTurnEnd()
        {
            JumpScenario("QuoridorTurn");
        }
    }
}


