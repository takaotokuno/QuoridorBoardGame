namespace Quoridor.Player.Status
{
    public abstract class AbstractStatus : IStatus
    {
        protected string _name;
        private int _remainTurns;
        public string Name => _name;
        public int RemainTurns => _remainTurns;
        
        public AbstractStatus(string name, int remainTurns)
        {
            _name = name;
            _remainTurns = remainTurns;
        }

        public void Apply(PlayerController player)
        {
            _remainTurns -- ;
            ApplyStatus(player);
        }

        protected virtual void ApplyStatus(PlayerController player){}

        public void Remove(PlayerController player)
        {
            _remainTurns -- ;
            RemoveStatus(player);
        }

        public virtual void RemoveStatus(PlayerController player){}

        public void AdjustTurn(int amount) => _remainTurns += amount;
    }
}
