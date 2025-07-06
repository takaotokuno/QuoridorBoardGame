using System;
using System.Collections.Generic;
using System.Linq;
using Quoridor.Board;

namespace Quoridor.Logic
{
    public class MoveValidator
    {
        public static IEnumerable<Pos> IdentifyPawnMoves(int[,] board, Pos[] pawns, int turn, int distance)
        {
            int[,] cloneBoard = (int[,]) board.Clone();
            cloneBoard[pawns[0].X, pawns[0].Y] = 1;
            cloneBoard[pawns[1].X, pawns[1].Y] = 1;

            Func<Node, int> priority = (node) => node.Cost;
            Func<Node, bool> endCondition = (current) => (current.Cost > distance);
            Func<Node, bool, bool> addCondition = (node, wasMetEnd) => (node.Cost == distance);

            foreach(Node node in PathFinder.ExplorePath(cloneBoard, pawns[turn], priority, endCondition, addCondition))
            {
                yield return new Pos(node.X, node.Y);
            }
        }

        public static IEnumerable<Pos> IdentifyWallMoves(int[,] board, Pos[]pawns)
        {
            int boardSize = board.GetLength(1);
            foreach(Pos move in BoardUtil.GetWallCoodinates(boardSize))
            {
                if(JudgeInterferWall(board, move)) continue;
                if(JudgeIsolatedWall(board, move))
                {
                    yield return move;
                    continue;
                }
                if(JudgeReachableWall(board, pawns, move))
                {
                    yield return move;
                    continue;
                }
            }
        }

        private static bool JudgeInterferWall(int[,] board, Pos move)
        {
            List<Pos> walls = BoardUtil.Get3WallCoodinate(move);
            foreach(Pos wall in walls)
            {
                if(board[wall.X, wall.Y] == 1) return true;
            }
            return false;
        }

        private static bool JudgeIsolatedWall(int[,] board, Pos move)
        {
            int col = move.X;
            int row = move.Y;

            int boardSize = board.GetLength(1);
            int isV = (col % 2 == 1) ? 1 : -1; // ? vertical wall : horizontal wall
            int count = 0;

            if(row == boardSize - 3 && isV == 1){
                count++;  // top vertilcal wall
            }
            else if(col == boardSize - 3 && isV == -1)
            {
                count++; // right horizontal wall
            }
            else{
                if(board[col + 2 - isV*2, row + 2 + isV*2] == 1){
                    count++; // directly above vertical or right horizontal wall
                }
                if(board[col + 1 - isV*2, row + 2 + isV] == 1) 
                {
                    count++; // left avove horizontal wall or right avobe vertical wall
                }
                if(board[col + 2 - isV, row + isV*2 + 1] == 1) 
                {
                    count++; // right avove horizontal wall or right bottom vertical wall
                }
            }
            if(count > 1) return false;

            if(row == 0 && isV == 1) 
            {
                count++; // bottom vertical wall
            }
            else if(col == 0 && isV == -1) 
            {
                count++; // left vertilcal wall
            }
            else
            {
                if(board[col - 1 + isV, row - 1 - isV] == 1) 
                {
                    count++; // directly bottom vertical or left horizontal wall
                }
                else if(board[col - 1, row - 1] == 1)
                {
                    count++; // left bottom horizontal wall or left bottom vertical wall
                }
                else if(board[col + isV, row - isV] == 1)
                {
                    count++; // right bottom horizontal wall or left above vertical wall
                }
            }
            if(count > 1) return false;

            if(board[col + 1, row + 1] == 1) count++;
            if(board[col - isV, row + isV] == 1) count++;

            if(count > 1) return false;
            return true;
        }

        private static bool JudgeReachableWall(int[,] board, Pos[]pawns, Pos move)
        {
            bool isReachable = true;
            BoardUtil.ApplyWallMove(board, move, 1);
            try{
                int playerDistance = CalculateDistance(board, pawns[0], board.GetLength(1) - 1);
                if(playerDistance < 0) throw new Exception();
                int cpuDistance = CalculateDistance(board, pawns[1], 0);
                if(cpuDistance < 0) throw new Exception();
            }
            catch
            {
                isReachable = false;
            }
            BoardUtil.ApplyWallMove(board, move, 0);
            return isReachable;
        }

        public static int CalculateDistance(int[,] board, Pos pawn, int goal)
        {
            IEnumerable<Node> nodes = PathFinder.ExploreShortestPathToGoal(board, pawn, goal);
            if(nodes.Count() == 0) return -1;
            return nodes.First().Cost;
        }
    }
}

