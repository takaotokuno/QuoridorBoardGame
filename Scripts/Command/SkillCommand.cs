using Quoridor.Player.Skill;

namespace Quoridor.Command
{
    public abstract class SkillCommand : ICommand
    {
        protected ISkill _skill;
        private SkillCommand(){}
        protected SkillCommand(ISkill skill) => _skill = skill;

        public void Execute()
        {
            _skill.AdjustCharge(-1);
            _skill.CoolDown();
            ExecuteSkill();
        }

        public void Undo()
        {
            _skill.AdjustCharge(1);
            _skill.Refresh();
            UndoSkill();
        }

        protected abstract void ExecuteSkill();
        protected abstract void UndoSkill();
    }
}