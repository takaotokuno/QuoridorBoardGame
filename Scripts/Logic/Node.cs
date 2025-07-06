namespace Quoridor.Logic
{
    public class Node
    {
        public int X;
        public int Y;
        public int Cost;
        public Node Parent;
        public Node(){}
        public Node(int x, int y, int cost, Node parent)
        {
            X = x;
            Y = y;
            Cost = cost;
            Parent = parent;
        }
    }
}