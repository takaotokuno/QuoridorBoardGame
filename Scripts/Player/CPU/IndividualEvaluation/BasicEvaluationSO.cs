using UnityEngine;
using Quoridor.Board;

namespace Quoridor.Player
{
    [CreateAssetMenu(menuName = "Evaluation/Basic")]
    public class BasicEvaluationSO : EvaluationSO
    {
        public override double Evaluate(BoardAnalysis analysis)
        {
            double self = 1.01 * (analysis.pDistances[analysis.pTurnIndex] - 0.5);
            double opp = analysis.pDistances[1 - analysis.pTurnIndex];

            double distanceDiff = opp - self;

            return distanceDiff * (analysis.pTurnIndex == 0 ? 1 : -1);
        }
    }
}