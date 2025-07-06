using UnityEngine;

namespace Quoridor.Player.Skill
{
    [CreateAssetMenu(fileName = "SkillDefinition", menuName = "Skill/BuildWall")]
    public class BuildWallSkillDefinitionSO : SkillDefinitionSO, IBoardSkillDefinition
    {
        public BoardSkill CreateSkill(int turnIndex)
        {
            BoardSkillStrategy strategy = new BuildWall();
            return new BoardSkill(strategy, pCapacity, pInterval);
        }
    }
}