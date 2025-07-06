using Quoridor.Player.Status;

namespace Quoridor.Player.Skill
{
    public class Confuse : BuffSkillStrategy
    {
        protected override IStatus GenerateStatus(int remainTurns)
        => new Confusion(remainTurns);
    }
}
