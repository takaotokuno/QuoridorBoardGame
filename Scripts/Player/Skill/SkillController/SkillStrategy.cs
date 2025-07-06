using System.Collections.Generic;
using Quoridor.Board;
using Quoridor.Command;
using Quoridor.Player.Status;

namespace Quoridor.Player.Skill
{
    public abstract class BoardSkillStrategy
    {
        public abstract IEnumerable<Pos> GetValidMoves(IBoard board);
        public abstract ICommand GenerateCommand(IBoard board, Pos move, SkillController skill);
    }

    public abstract class BuffSkillStrategy
    {
        public ICommand GenerateCommand(IPlayer target, int remainTurns, SkillController skill)
        {
            return new AddStatusCommand(target, GenerateStatus(remainTurns), skill);
        }
        protected abstract IStatus GenerateStatus(int remainTurns);
    }
}