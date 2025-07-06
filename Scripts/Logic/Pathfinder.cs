using System;
using System.Collections.Generic;
using Quoridor.Board;

namespace Quoridor.Logic
{
    public class PathFinder
    {
        private static readonly (int dx, int dy)[] DIRECTIONS = new (int, int)[]
        {
            (0, 1), (0, -1), (1, 0), (-1, 0)
        };
        
        public static IEnumerable<Node> ExploreShortestPathToGoal(int[,] board, Pos pos, int goal)
        {
            Func<Node, int> priority = (node) => (node.Cost + Math.Abs(node.Y - goal));
            Func<Node, bool> endCondition = (current) => current.Y == goal;
            Func<Node, bool, bool> addCondition = (node, reachedGoal) => reachedGoal;

            return ExplorePath(board, pos, priority, endCondition, addCondition);
        }

        public static IEnumerable<Node> ExplorePath(
            int[,] board
            , Pos pos
            , Func<Node, int> priority
            , Func<Node, bool> endCondition
            , Func<Node, bool, bool> addCondition
        )
        {
            List<Node> openList = new List<Node>();

            int boardSize = board.GetLength(1);
            bool[,] visited = new bool[boardSize, boardSize];

            openList.Add(new Node(pos.X, pos.Y, 0, null));
            while(openList.Count > 0)
            {
                openList.Sort((a, b) => priority(a) - priority(b));
                Node current = openList[0];
                openList.RemoveAt(0);

                if(endCondition(current)){
                    if(addCondition(current, true)) yield return current;
                    break;
                }
                if(addCondition(current, false)) yield return current;

                visited[current.X, current.Y] = true;

                foreach(Node neighbor in GetNeighbors(board, current))
                {
                    if(visited[neighbor.X, neighbor.Y]) continue;

                    Node existingNode = openList.Find(eNode => eNode.X == neighbor.X && eNode.Y == neighbor.Y);
                    if(existingNode == null)
                    {
                        openList.Add(neighbor);
                    }
                    else if(neighbor.Cost < existingNode.Cost)
                    {
                        openList.Remove(existingNode);
                        openList.Add(neighbor);
                    }
                }
            }
        }

        private static IEnumerable<Node> GetNeighbors(int[,] board, Node current)
        {
            int boardSize = board.GetLength(1);

            foreach((int dx, int dy) in DIRECTIONS)
            {
                (int x, int y) = MovePosition(current.X, current.Y, dx, dy);

                if(CheckOutside(boardSize, x, y)) continue;

                bool isBlocked = (board[x, y] == 1);
                if(ChackBlocked(board, x, y)) continue;

                (x, y) = MovePosition(x, y, dx, dy);

                // there is a blank space.
                if(board[x, y] == 0)
                {
                    yield return new Node(x, y, current.Cost + 1, current);
                }
                // there is the opponent's pawn.
                else
                {
                    (int x2, int y2) = MovePosition(x, y, dx, dy);
                    // neither outside nor blocked
                    if(!CheckOutside(boardSize, x2, y2) && !ChackBlocked(board, x2, y2))
                    {
                        (x2, y2) = MovePosition(x2, y2, dx, dy);
                        yield return new Node(x2, y2, current.Cost + 1, current);
                    }
                    else
                    {
                        // at the end of the 90 degree turn
                        (int x3, int y3) = MovePosition(x, y, dy, dx);
                        if(!CheckOutside(boardSize, x3, y3) && !ChackBlocked(board ,x3, y3)){
                            (x3, y3) = MovePosition(x3, y3, dy, dx);
                            yield return new Node(x3, y3, current.Cost + 1, current);
                        }

                        (int x4, int y4) = MovePosition(x, y, -dy, -dx);
                        if(!CheckOutside(boardSize, x4, y4) && !ChackBlocked(board, x4, y4)){
                            (x4, y4) = MovePosition(x4, y4, -dy, -dx);
                            yield return new Node(x4, y4, current.Cost + 1, current);
                        }
                    }
                }
            }
            yield break;
        }

        private static (int, int) MovePosition(int x, int y, int dx, int dy)
        {
            return (x + dx, y + dy);
        }

        private static bool CheckOutside(int boardSize, int x, int y){
            return (x < 0 || x > boardSize - 1 || y < 0 || y > boardSize - 1);
        }

        private static bool ChackBlocked(int[,] board, int x, int y)
        {
            return (board[x, y] == 1);
        }
    }
}

