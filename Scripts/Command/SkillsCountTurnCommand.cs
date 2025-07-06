using System.Collections.Generic;
using Quoridor.Player.Skill;

namespace Quoridor.Command
{
    public class SkillsCountTurnCommand : ICommand
    {
        List<ISkill> _skillList;
        private List<int> _turnCountList;
        public SkillsCountTurnCommand(List<ISkill> skillList)
        {
            _skillList = skillList;
            _turnCountList = new ();
        }

        public void Execute()
        {
            foreach(ISkill skill in _skillList) _turnCountList.Add(skill.CountInterval(-1));
        }

        public void Undo()
        {
            for(int i = 0; i < _turnCountList.Count; i++)
            {
                if(_turnCountList[i] > 0) _skillList[i].CountInterval(1);
            }
        }
    }
}