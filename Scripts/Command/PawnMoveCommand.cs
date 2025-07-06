using Quoridor.Board;
using Quoridor.Player.Skill;

namespace Quoridor.Command
{
    public class PawnMoveCommand : BoardCommand
    {
        private int _turnIndex;
        private Pos _startPos;
        private Pos _targetPos;

        public PawnMoveCommand(IBoard board, int turnIndex, Pos targetPos, ISkill skill)
        : base(board, skill)
        {
            _turnIndex = turnIndex;
            _startPos = _board.Pawns[turnIndex];
            _targetPos = targetPos;
        }
        
        protected override void ExecuteSkill() => MovePawn(_targetPos);

        protected override void UndoSkill() => MovePawn(_startPos);

        private void MovePawn(Pos pos) => _board.Pawns[_turnIndex] = pos;
    }
}
