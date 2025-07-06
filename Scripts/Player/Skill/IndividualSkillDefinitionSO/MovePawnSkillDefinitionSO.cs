using UnityEngine;

namespace Quoridor.Player.Skill
{
    [CreateAssetMenu(fileName = "SkillDefinition", menuName = "Skill/MovePawn")]
    public class MovePawnSkillDefinitionSO : SkillDefinitionSO, IBoardSkillDefinition
    {
        public int pDistance;
        public BoardSkill CreateSkill(int turnIndex)
        {
            BoardSkillStrategy strategy = new MovePawn(turnIndex, pDistance);
            return new BoardSkill(strategy, pCapacity, pInterval);
        }
    }
}