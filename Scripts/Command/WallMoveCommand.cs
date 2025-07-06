using System.Collections.Generic;
using Quoridor.Board;
using Quoridor.Player.Skill;

namespace Quoridor.Command
{
    public class WallMoveCommand : BoardCommand
    {
        private List<Pos> _walls;
        private bool _doBuild;
        public WallMoveCommand(IBoard board, List<Pos> walls, bool doBuild, ISkill skill)
        : base(board, skill)
        {
            _walls = walls;
            _doBuild = doBuild;
        }

        protected override void ExecuteSkill() => SlideWall(_doBuild);

        protected override void UndoSkill() => SlideWall(!_doBuild);

        private void SlideWall(bool doBuild){
            foreach(Pos wall in _walls)
            {
                _board.Grid[wall.X, wall.Y] = (doBuild) ? 1 : 0;
            }
        }
    }
}
