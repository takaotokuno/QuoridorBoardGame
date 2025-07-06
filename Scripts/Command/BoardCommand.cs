using Quoridor.Board;
using Quoridor.Player.Skill;

namespace Quoridor.Command
{
    public abstract class BoardCommand : SkillCommand
    {
        protected IBoard _board;
        protected BoardCommand(IBoard board, ISkill skill) : base(skill)
        {
            _board = board;
        }
    }
}