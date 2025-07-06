using UnityEngine;
using Quoridor.Board;

namespace Quoridor.Player
{
    public interface IEvaluation
    {
        double Evaluate(BoardAnalysis analysis);
    }

    public abstract class EvaluationSO : ScriptableObject, IEvaluation
    {
        public abstract double Evaluate(BoardAnalysis analysis);
    }
}