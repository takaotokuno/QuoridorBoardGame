namespace Quoridor.Player.Status
{
    public class SealedSkill : AbstractStatus
    {
        private string _tagName;
        public SealedSkill(int remainTurns, string tagName)
        : base("Sealed" + tagName, remainTurns)
        => _tagName = tagName;

        protected override void ApplyStatus(PlayerController player)
        => player.AddSealedSkillSet(_tagName);

        public override void RemoveStatus(PlayerController player)
        => player.RemoveSealedSkillSet(_tagName);
    }
}