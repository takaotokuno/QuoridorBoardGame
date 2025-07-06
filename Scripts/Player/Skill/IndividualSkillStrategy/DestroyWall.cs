using System.Collections.Generic;
using Quoridor.Board;
using Quoridor.Command;

namespace Quoridor.Player.Skill
{
    public class DestroyWall : BoardSkillStrategy
    {
        public override IEnumerable<Pos> GetValidMoves(IBoard board)
        {
            for(int row = 0; row < board.Grid.GetLength(1) - 1; row ++)
            {
                for(int col = 0; col < board.Grid.GetLength(1) - 1; col ++)
                {
                    if(BoardUtil.IsWallCoordinates((col, row)))
                    {
                        if(board.Grid[col, row] == 1) yield return new Pos(col, row);
                    }
                }
            }
        }

        public override ICommand GenerateCommand(IBoard board, Pos move, SkillController skill)
        {
            List<Pos> walls = Get3WallCoodinate(board, move);
            return new WallMoveCommand(board, walls, false, skill);
        }

        private static List<Pos> Get3WallCoodinate(IBoard board, Pos wall)
        {
            List<Pos> walls = new List<Pos>{wall};
            int boardSize = board.Grid.GetLength(1);

            Pos firstWall = BoardUtil.GetNeighborCoodinate(wall, -1);
            if(firstWall.X > -1 && firstWall.Y > -1) walls.Add(firstWall);

            Pos end = BoardUtil.GetNeighborCoodinate(wall, 1);
            if(end.X < boardSize && end.Y < boardSize) walls.Add(end);

            return walls;
        }
    }
}