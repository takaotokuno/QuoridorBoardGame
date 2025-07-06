using Quoridor.Board;

namespace Quoridor.UI
{
    public class SkipButton : TurnUIButton
    {
        private GameMode _gameMode;
        public void SetGameMode(GameMode gameMode) => _gameMode = gameMode;
        public void JudgeAppearance(TurnManager turnManager)
        {
            bool isActive = gameObject.activeSelf;

            int turnIndex = turnManager.TurnIndex;
            RESULT result = turnManager.ResultRef;

            bool isWinP1 = (result == RESULT.P1_CHECKMATE && turnIndex == 0);
            bool isWinP2 = (result == RESULT.P2_CHECKMATE && turnIndex == 1);

            if (_gameMode == GameMode.SCENARIO)
            {
                if (!isActive && isWinP1) AppearanceElement();
                SetInteractable(isWinP1);
            }
            else
            {
                if (!isActive && (isWinP1 || isWinP2)) AppearanceElement();
                SetInteractable(isWinP1 || isWinP2);
            }
        }
        public void OnSkip() => _manager?.OnSkip(this);
    }
}