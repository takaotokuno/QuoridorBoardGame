using Quoridor.Player.Status;

namespace Quoridor.Player.Skill
{
    public class SealSkill : BuffSkillStrategy
    {
        private string _tagName;
        public SealSkill(string tagName)
        => _tagName = tagName;
        
        protected override IStatus GenerateStatus(int remainTurns)
        => new SealedSkill(remainTurns, _tagName);
    }
}