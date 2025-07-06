using System;

namespace Quoridor.Board
{
    public class BoardState : IBoard
    {
        protected int[,] _grid;
        protected Pos[] _pawns;

        public int[,] Grid => _grid;
        public Pos[] Pawns => _pawns;

        private BoardState(){}
        public BoardState(int boardSize)
        {
            _grid = new int[boardSize, boardSize];
            _pawns = new Pos[2];
        }

        public void Initialize()
        {
            int boardSize = _grid.GetLength(1);
            _grid = new int[boardSize, boardSize];
            int boardCenter = BoardUtil.CalculateCenter(boardSize);
            _pawns[0] = new Pos(boardCenter, 0);
            _pawns[1] = new Pos(boardCenter, boardSize - 1);
        }

        public IBoard DeepCopy()
        {
            BoardState clone = new BoardState(_grid.GetLength(1));
            Array.Copy(_grid, clone.Grid, _grid.Length);
            Array.Copy(_pawns, clone.Pawns, _pawns.Length);
            return clone;
        }
    }
}