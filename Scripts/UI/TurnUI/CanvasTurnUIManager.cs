using UnityEngine;
using Quoridor.Board;

namespace Quoridor.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class CanvasTurnUIManager : UIManager
    {
        // Game Objects
        [SerializeField] private TurnView pTurnView;
        [SerializeField] private ResultView pResultView;
        [SerializeField] private ResignButton pResignButton;
        [SerializeField] private SkipButton pSkipButton;
        [SerializeField] private ResetButton pResetButton;
        [SerializeField] private StandPictureFrame pStandPictureFrame;

        protected override void Awake()
        {
            pResignButton?.SetManager(this);
            pSkipButton?.SetManager(this);
            pSkipButton?.SetGameMode(_facade.GameModeRef);
            pResetButton?.SetManager(this);
            base.Awake();
        }

        public override void AppearanceElement()
        {
            pResultView?.SetActive(false);
            pSkipButton?.SetActive(false);
            pResetButton?.SetActive(false);
            gameObject.SetActive(true);

            gameObject.GetComponent<AudioSource>().Play();

            pTurnView?.AppearanceElement();
            pResignButton?.AppearanceElement();
            pStandPictureFrame?.AppearanceElement();
        }

        // 表示更新
        public void UpdateDisplay(TurnManager turnManager)
        {
            pTurnView?.UpdateDisplay(turnManager);
            pSkipButton?.JudgeAppearance(turnManager);
            if(turnManager.TurnIndex == 1) pStandPictureFrame?.TurnStart();
        }

        // ゲーム終了時表示（降参orスキップボタンは非表示にする）
        public void ShowResult(RESULT result)
        {
            pResignButton?.SetInteractable(false);
            pSkipButton?.SetActive(false);
            pResetButton?.SetActive(false);
            pResultView?.ShowResult(result);
            pResultView?.AppearanceElement();
        }

        public override void SetPlayersConfig(IStartConfig config)
        {
            pStandPictureFrame?.SetThinkingTime(config);
        }

        // 下位クラス（ボタン）からの通知をFacadeに送信
        public void OnResign(ResignButton button) => _facade.OnResign(this);
        public void OnSkip(SkipButton button) => _facade.OnSkip(this);
        public void OnReset(ResetButton button) => _facade.OnReset(this);
    }
}