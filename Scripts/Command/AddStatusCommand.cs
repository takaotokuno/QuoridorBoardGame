using Quoridor.Player;
using Quoridor.Player.Skill;
using Quoridor.Player.Status;

namespace Quoridor.Command
{
    public class AddStatusCommand : SkillCommand
    {
        private IPlayer _target;
        private IStatus _status;
        public AddStatusCommand(IPlayer target, IStatus status, ISkill skill) : base(skill)
        {
            _target = target;
            _status = status;
        }
        protected override void ExecuteSkill() => _target.AddActiveStatus(_status);
        protected override void UndoSkill() => _target.RemoveActiveStatus(_status);
    }
}