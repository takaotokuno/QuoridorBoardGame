using System.Collections.Generic;
using Quoridor.Player;

namespace Quoridor.Board
{
    public class BoardAnalysis
    {
        public int pTurnIndex;
        public IBoard pBoard;
        public int[] pDistances;
        List<SkillPair>[] pSkillPairsArray;

        private BoardAnalysis(){}
        public BoardAnalysis(int turnIndex, IBoard board, List<SkillPair>[] skillPairsArray)
        {
            pTurnIndex = turnIndex;
            pBoard = board;
            pSkillPairsArray = skillPairsArray;
            UpdateDistance();
        }

        public void UpdateDistance() => pDistances = BoardUtil.CalculateDistances(pBoard);
    }
}
