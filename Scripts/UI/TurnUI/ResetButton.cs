namespace Quoridor.UI
{
    public class ResetButton : TurnUIButton
    {
        public void OnReset() => _manager?.OnReset(this);
    }
}