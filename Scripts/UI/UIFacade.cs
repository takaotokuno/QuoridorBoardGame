using Quoridor.Board;

namespace Quoridor.UI
{
    public abstract class UIFacade : Facade
    {
        // UIの直接指示
        public abstract void SetActiveCamera(bool isActive);
        public abstract void SetRaycastEnabled(bool isEnabled);
        public abstract void ResetUI();
        public abstract void ShowBoard();
        public abstract void ShowPawn1();
        public abstract void ShowPawn2();
        public abstract void ShowUI();
        public abstract void ShowResult(RESULT result);

        // Mediatorからの通知
        public abstract void SetPlayersConfig(IStartConfig config);
        public void OnGameStart() => ShowUI();
        public void OnGameEnd(RESULT result) => ShowResult(result);
        public abstract void UpdateUI();

        // UIManagerからの通知
        public abstract bool CanMove(UIManager manager, Pos move);
        public abstract void OnMove(UIManager manager, Pos move);
        public abstract void OnUseSkill(UIManager manager, int turnIndex, int slotIndex);
        public abstract void OnStart(UIManager manager, IStartConfig config);
        public abstract void OnResign(UIManager manager);
        public abstract void OnSkip(UIManager manager);
        public abstract void OnReset(UIManager manager);
    }
}