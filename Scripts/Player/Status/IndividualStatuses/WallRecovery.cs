namespace Quoridor.Player.Status
{
    public class WallRecovery : AbstractStatus
    {
        private int _interval;
        private int _intervalCount;
        private int _amount;
        
        public WallRecovery(string name, int remainTurns, int interval, int amount)
        : base(name, remainTurns)
        {
            _interval = interval;
            _intervalCount = interval;
            _amount = amount;
        }
        
        protected override void ApplyStatus(PlayerController player)
        {
            if(_intervalCount < 1)
            {
                player.SkillSlots[1].AdjustCharge(_amount);
                _intervalCount = _interval;
            }
            _intervalCount --;
        }
    }
}