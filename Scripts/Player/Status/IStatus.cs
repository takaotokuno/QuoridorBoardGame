namespace Quoridor.Player.Status
{
    public interface IStatusView
    {
        public string Name {get;}
        public int RemainTurns {get;}
    }

    public interface IStatus : IStatusView
    {
        public void Apply(PlayerController player);
        public void Remove(PlayerController player);
        public void AdjustTurn(int amount);
    }
}
