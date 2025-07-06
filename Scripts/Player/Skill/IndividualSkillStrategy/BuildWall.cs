using System.Collections.Generic;
using Quoridor.Logic;
using Quoridor.Board;
using Quoridor.Command;

namespace Quoridor.Player.Skill
{
    public class BuildWall : BoardSkillStrategy
    {
        public override IEnumerable<Pos> GetValidMoves(IBoard board)
        => MoveValidator.IdentifyWallMoves(board.Grid, board.Pawns);

        public override ICommand GenerateCommand(IBoard board, Pos move, SkillController skill)
        => new WallMoveCommand(board, BoardUtil.Get3WallCoodinate(move), true, skill);
    }
}