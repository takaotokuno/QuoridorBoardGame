using System.Collections.Generic;
using Quoridor.Logic;
using Quoridor.Command;
using Quoridor.Board;

namespace Quoridor.Player.Skill
{
    public class MovePawn : BoardSkillStrategy
    {
        private int _turnIndex;
        private int _distance;
        
        private MovePawn(){}
        public MovePawn(int turnIndex, int distance)
        {
            _turnIndex = turnIndex;
            _distance = distance;
        }
        
        public override IEnumerable<Pos> GetValidMoves(IBoard board)
        => MoveValidator.IdentifyPawnMoves(board.Grid, board.Pawns, _turnIndex, _distance);

        public override ICommand GenerateCommand(IBoard board, Pos move, SkillController skill)
        => new PawnMoveCommand(board, _turnIndex, move, skill);
    }
}
