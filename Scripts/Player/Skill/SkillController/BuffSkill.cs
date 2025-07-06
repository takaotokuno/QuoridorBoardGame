namespace Quoridor.Player.Skill
{
    public class BuffSkill : SkillController, IInstanceSkill
    {
        protected BuffSkillStrategy _strategy;
        protected IPlayer _target;
        protected int _remainTurns;
        public BuffSkill(
            BuffSkillStrategy strategy
            , IPlayer target
            , int remainTurns
            , int capacity
            , int interval)
        : base(capacity, interval)
        {
            _strategy = strategy;
            _target = target;
            _remainTurns = remainTurns;
        }

        public override void Execute()
        => _commandInvoker.ExecuteCommand(_strategy.GenerateCommand(_target, _remainTurns, this));
    }
}
