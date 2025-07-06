using UnityEngine;

namespace Quoridor.Player.Skill
{
    [CreateAssetMenu(fileName = "SkillDefinition", menuName = "Skill/DestroyWall")]
    public class DestroyWallSkillDefinitionSO : SkillDefinitionSO, IBoardSkillDefinition
    {
        public BoardSkill CreateSkill(int turnIndex)
        {
            BoardSkillStrategy strategy = new DestroyWall();
            return new BoardSkill(strategy, pCapacity, pInterval);
        }
    }
}