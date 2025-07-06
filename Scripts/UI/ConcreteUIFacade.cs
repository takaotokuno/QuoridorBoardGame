using UnityEngine;
using Quoridor.Board;
using System.Threading.Tasks;

namespace Quoridor.UI
{
    public class ConcreteUIFacade : UIFacade
    {
        public static UIFacade p_sInstance;

        [SerializeField] private UICamera pQuoridorCamera;
        [SerializeField] private StartPanelManager pStartPanelManager;
        [SerializeField] private CanvasTurnUIManager pTurnUIManager;
        [SerializeField] private CanvasParameterUIManager pParameterUIManager;
        [SerializeField] private CanvasSkillUIManager pSkillUIManager;
        [SerializeField] private CanvasStatusUIManager pStatusUIManager;
        [SerializeField] private BoardObjectManager pBoardUIManager;

        private void Awake()
        {
            if (p_sInstance == null)
            {
                p_sInstance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        private void Start()
        {
            pStartPanelManager?.SetFacade(this);
            pTurnUIManager?.SetFacade(this);
            pParameterUIManager?.SetFacade(this);
            pSkillUIManager?.SetFacade(this);
            pStatusUIManager?.SetFacade(this);
            pBoardUIManager?.SetFacade(this);

            SetActiveCamera(!IsScenarioMode);
            SetRaycastEnabled(!IsScenarioMode);
            pStartPanelManager?.SetActive(!IsScenarioMode);
        }

        public override void SetActiveCamera(bool isActive)
        => pQuoridorCamera?.SetActive(isActive);

        public override void SetRaycastEnabled(bool isEnabled)
        => pQuoridorCamera?.SetRaycastEnabled(isEnabled);
        
        public override void ResetUI()
        {
            if (GameModeRef == GameMode.SCENARIO)
            {
                SetActiveCamera(false);
            }
            else pStartPanelManager?.SetActive(true);

            pTurnUIManager?.SetActive(false);
            pParameterUIManager?.SetActive(false);
            pSkillUIManager?.SetActive(false);
            pStatusUIManager?.SetActive(false);
        }

        public override void ShowBoard()
        {
            SetActiveCamera(true);
            pBoardUIManager?.ShowBoard(BoardRef);
        }

        public override void ShowPawn1()
        {
            SetActiveCamera(true);
            pBoardUIManager?.ShowPawn1(BoardRef);
        }

        public override void ShowPawn2()
        {
            SetActiveCamera(true);
            pBoardUIManager?.ShowPawn2(BoardRef);
        }

        public override async void ShowUI()
        {
            if(GameModeRef == GameMode.SCENARIO)
            {
                pStartPanelManager?.SetActive(false);
            }
            else pStartPanelManager?.SetActive(true);

            SetActiveCamera(true);

            pBoardUIManager?.AppearanceElement(BoardRef);

            await Task.Delay(1000);

            pTurnUIManager?.AppearanceElement();
            pParameterUIManager?.AppearanceElement();
            pSkillUIManager?.AppearanceElement();
            pStatusUIManager?.AppearanceElement();
        }

        public override void ShowResult(RESULT result)
        {
            pTurnUIManager?.ShowResult(result);
        }

        public override void SetPlayersConfig(IStartConfig config)
        {
            pStartPanelManager?.SetPlayersConfig(config);
            pTurnUIManager?.SetPlayersConfig(config);
            pParameterUIManager?.SetPlayersConfig(config);
            pSkillUIManager?.SetPlayersConfig(config);
            pStatusUIManager?.SetPlayersConfig(config);
            pBoardUIManager?.SetPlayersConfig(config);
        }

        public override void UpdateUI()
        {
            pTurnUIManager?.UpdateDisplay(TurnManagerRef);
            pParameterUIManager?.UpdateDisplay(BoardRef, PlayersRef);
            pSkillUIManager?.UpdateDisplay(PlayersRef);
            pStatusUIManager?.UpdateDisplay(PlayersRef);
            pBoardUIManager?.UpdateDisplay(BoardRef);
        }

        public override bool CanMove(UIManager manager, Pos move)
        => PlayersRef[TurnManagerRef.TurnIndex]?.JudgeSelectableMove(move) ?? false;

        public override void OnMove(UIManager manager, Pos move)
        => PlayersRef[TurnManagerRef.TurnIndex]?.OnInstructMove(move);

        public override void OnUseSkill(UIManager manager, int turnIndex, int slotIndex)
        {
            if (TurnManagerRef.TurnIndex == turnIndex)
            {
                PlayersRef[turnIndex]?.OnInstructUseSkill(slotIndex);
            }
        }

        public override void OnStart(UIManager manager, IStartConfig config)
        => MediatorRef?.Notify(this, GameEvent.START, config);

        public override void OnResign(UIManager manager)
        => NotifyGameEnd(1 - TurnManagerRef.TurnIndex);

        public override void OnSkip(UIManager manager)
        => NotifyGameEnd(TurnManagerRef.TurnIndex);

        private void NotifyGameEnd(int winnerIndex)
        {
            RESULT result = (winnerIndex == 0) ? RESULT.P1_WIN : RESULT.P2_WIN;
            MediatorRef?.Notify(this, GameEvent.END, result);
        }

        public override void OnReset(UIManager manager)
        => _director?.CreateMatch();
    }
}
