namespace Quoridor.Player.Status
{
    public class Confusion : AbstractStatus
    {
        private bool _isFirst;
        public Confusion(int remainTurns)
        : base("Confusion", remainTurns)
        => _isFirst = true;
        
        protected override void ApplyStatus(PlayerController player)
        {
            if(_isFirst)
            {
                player.CorrectCpuLevel(0, 0);
                player.SwitchAutoManualMode(true);
            }
            _isFirst = false;
        }
        
        public override void RemoveStatus(PlayerController player)
        {
            player.CorrectCpuLevel(0, 999);
            player.SwitchAutoManualMode(false);
        }
    }
}