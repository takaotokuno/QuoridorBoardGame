namespace Quoridor.UI
{
    public class TurnUIButton : UIButton
    {
        protected CanvasTurnUIManager _manager;
        public void SetManager(CanvasTurnUIManager manager) => _manager = manager;
    }
}