namespace Quoridor.Board
{
    public interface IBoard
    {
        public int[,] Grid {get;}
        public Pos[] Pawns {get;}
        public void Initialize();
        public IBoard DeepCopy();
    }
}

