using Quoridor.Player.Status;

namespace Quoridor.Player.Skill
{
    public class Sleep : BuffSkillStrategy
    {
        protected override IStatus GenerateStatus(int remainTurns)
        => new Status.Sleep(remainTurns);
    }
}