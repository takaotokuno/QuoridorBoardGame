using System.Collections.Generic;
using Quoridor.Command;

namespace Quoridor.Player.Skill
{
    public class SkillFactory
    {
        private IPlayer _player;
        private IPlayer _opponent;
        private ICommandInvoker _commandInvoker;

        public SkillFactory(IPlayer player, IPlayer opponent, ICommandInvoker commandInvoker)
        {
            _player = player;
            _opponent = opponent;
            _commandInvoker = commandInvoker;
        }

        public void SetSkillSlots(List<ISkill> skillSlots, List<SkillDefinitionSO> skillDefinitionList)
        {
            foreach(SkillDefinitionSO skillDifinition in skillDefinitionList)
            {
                skillSlots.Add(ConvertDifinition2Skill(skillDifinition));
            }
        }

        private SkillController ConvertDifinition2Skill(SkillDefinitionSO def)
        {
            SkillController skill;

            if(def is IBoardSkillDefinition)
            {
                skill = (def as IBoardSkillDefinition).CreateSkill(_player.TurnIndex);
            }
            else if(def is IBuffSkillDefinition)
            {
                skill = (def as IBuffSkillDefinition).CreateSkill(_player, _opponent);
            }
            else skill = new EmptySkill();

            skill.SetCommandInvoker(_commandInvoker);

            skill.AddTag(def.pSkillName);
            skill.AddTags(def.pTagList);

            return skill;
        }
    }
}
