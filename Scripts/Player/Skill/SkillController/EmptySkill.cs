using Quoridor.Command;

namespace Quoridor.Player.Skill
{
    public class EmptySkill : SkillController
    {
        public EmptySkill() : base(0, 0){}

        public override void Execute()
        => _commandInvoker.ExecuteCommand(new EmptyCommand());
    }
}