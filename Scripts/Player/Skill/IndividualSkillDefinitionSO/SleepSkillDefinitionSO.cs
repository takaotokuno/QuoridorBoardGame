using UnityEngine;

namespace Quoridor.Player.Skill
{
    [CreateAssetMenu(fileName = "SkillDefinition", menuName = "Skill/Sleep")]
    public class SleepSkillDefinitionSO : SkillDefinitionSO, IBuffSkillDefinition
    {
        public int pRemainTurns;

        public BuffSkill CreateSkill(IPlayer player, IPlayer opponent)
        {
            BuffSkillStrategy strategy = new Sleep();
            BuffSkill skill = new BuffSkill(
                strategy, opponent, pRemainTurns, pCapacity, pInterval
            );
            return skill;
        }
    }
}