using Quoridor.Player.Status;

namespace Quoridor.Player.Skill
{
    public class WallRecovery : BuffSkillStrategy
    {
        private int _interval;
        private int _amount;

        public WallRecovery(int interval, int amount)
        {
            _interval = interval;
            _amount = amount;
        }
    
        protected override IStatus GenerateStatus(int remainTurns)
        {
            string name = "WallRecovery_" + _interval + "_" + _amount;
            return new Status.WallRecovery(name, remainTurns, _interval, _amount);
        }
    }
}
