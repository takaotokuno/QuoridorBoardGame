using System;
using System.Collections.Generic;
using Quoridor.Logic;
using Quoridor.Player;

namespace Quoridor.Board
{
    public enum RESULT { P1_WIN ,P2_WIN ,PROGRESS, P1_CHECKMATE, P2_CHECKMATE }
    public class BoardUtil
    {
        private static BoardUtil s_instance;
        private int _boardSize;
        private List<Pos> _wallCoodinates;

        private BoardUtil(){}
        public BoardUtil(int boardSize)
        {
            if (s_instance == null || _boardSize != boardSize)
            {
                s_instance = this;
                _boardSize = boardSize;
                _wallCoodinates = new List<Pos>();
                SetWallCoodinates(boardSize);
            }
        }

        public static List<Pos> GetWallCoodinates(int boardSize)
        {
            new BoardUtil(boardSize);
            GameUtils.Shuffle(s_instance._wallCoodinates);
            return s_instance._wallCoodinates;
        }

        private void SetWallCoodinates(int boardSize)
        {
            for(int row = 0; row < boardSize - 1; row++)
            {
                for(int col = 0; col < boardSize - 1; col++)
                {
                    if(IsWallCoordinates(col, row)) _wallCoodinates.Add(new Pos(col, row));
                }
            }
        }

        public static bool IsWallCoordinates(int col, int row)
        {
            return (col % 2 == 1 ^ row % 2 == 1);
        }
        
        public static bool IsWallCoordinates((int x,int y) move)
        {
            return (move.x % 2 == 1 ^ move.y % 2 == 1);
        }

        public static bool IsTileCoordinates(int col, int row)
        {
            return (col % 2 == 0 && row % 2 == 0);
        }

        public static void ApplyWallMove(int[,] board, Pos move, int d)
        {
            ApplyWallMove(board, move.X, move.Y, d);
        }

        public static void ApplyWallMove(int[,] board, int x, int y, int d)
        {
            int isV = (x % 2 == 1) ? 1 : -1; // ? vertical wall : horizontal wall
            board[x, y] = d;
            board[x - isV + 1, y + isV + 1] = d;
        }

        public static RESULT JudgeResult(IBoard board)
        {
            if(board.Pawns[0].Y == board.Grid.GetLength(1) - 1) return RESULT.P1_WIN;
            if(board.Pawns[1].Y == 0) return RESULT.P2_WIN;
            return RESULT.PROGRESS;
        }

        public static RESULT JudgeResult(IBoard board, IPlayer[] players)
        {
            RESULT result = JudgeResult(board);
            if(result != RESULT.PROGRESS) return result;

            int[] distances = BoardUtil.CalculateDistances(board);
            if(players[1].RemainWalls == 0 && distances[0] < distances[1])
            {
                return RESULT.P1_CHECKMATE;
            }
            if(players[0].RemainWalls == 0 && distances[0] > distances[1])
            {
                return RESULT.P2_CHECKMATE;
            }
            return result;
        }

        public static int[] CalculateDistances(IBoard board)
        {
            int[] distances = new int[2];
            distances[0] = MoveValidator.CalculateDistance(board.Grid, board.Pawns[0], board.Grid.GetLength(1) - 1);
            distances[1] = MoveValidator.CalculateDistance(board.Grid, board.Pawns[1], 0);
            return distances;
        }

        public static int CalculateCenter(int boardSize)
        {
            return Decimal.ToInt32((boardSize - 1) / 2);
        }

        public static Pos GetNeighborCoodinate(Pos wall, int amount)
        {
            if(wall.X % 2 == 1) wall.Y += amount;
            else wall.X += amount;
            return wall;
        }

        public static List<Pos> Get3WallCoodinate(Pos firstWall)
        {
            Pos middleWall = GetNeighborCoodinate(firstWall, 1);
            Pos endWall = GetNeighborCoodinate(firstWall, 2);
            return new(){firstWall, middleWall, endWall};
        }
    }
}