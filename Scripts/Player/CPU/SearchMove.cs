using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Quoridor.Board;
using Quoridor.Command;
using Quoridor.Player.Skill;

namespace Quoridor.Player
{
    public class SearchMove
    {
        private List<SkillPair>[] _skillPairsArray;
        private IEvaluation _evaluation;
        private ICommandInvoker _commandInvoker;
        private int _turnIndex;
        private IBoard _board;
        private int _searchTime;

        public SearchMove(IEvaluation evaluation)
        {
            _skillPairsArray = new List<SkillPair>[2];
            _evaluation = evaluation;
            _commandInvoker = new PlaneCommandInvoker();
        }

        public void SetCalculationConditions(IBoard board, IPlayer player, IPlayer opponent, int searchTime)
        {
            _turnIndex = player.TurnIndex;
            _board = board.DeepCopy();
            _skillPairsArray[_turnIndex] = ConvertSlots2SkillPairs(player.SkillSlots);
            
            List<SkillPair> oppSkillSlots = ConvertSlots2SkillPairs(opponent.SkillSlots);
            foreach(SkillPair pair in oppSkillSlots)
            {
                pair.pClone.CountInterval(-1);
                pair.pClone.SetState(SkillState.READY);
            }
            _skillPairsArray[1- _turnIndex] = oppSkillSlots.Where(pair => SkillUtil.CanUse(pair.pClone)).ToList();
            
            _searchTime = searchTime;
        }

        private List<SkillPair> ConvertSlots2SkillPairs(List<ISkill> skillSlots)
        {
            List<SkillPair> skillPairs = new();
            foreach (BoardSkill skill in skillSlots.OfType<BoardSkill>())
            {
                skillPairs.Add(new SkillPair(skill, _commandInvoker));
            }
            return skillPairs;
        }

        public Move SearchBestMove()
        {
            int depth = 1;

            Move firstMove = GetSelectableMoves(_board, _skillPairsArray[_turnIndex]).First();
            (double score, Move move) lastBestMove = (WorstScore(_turnIndex), firstMove);

            _commandInvoker.Initialize();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while(stopwatch.ElapsedMilliseconds < _searchTime && depth < 20)
            {
                var bestMove = AlphaBetaSearch(depth, int.MinValue, int.MaxValue, _turnIndex, stopwatch);
                if(_turnIndex == 0 && lastBestMove.score < bestMove.score) lastBestMove = bestMove;
                if(_turnIndex == 1 && lastBestMove.score > bestMove.score) lastBestMove = bestMove;
                
                depth++;
            }
            stopwatch.Stop();

            return lastBestMove.move;
        }

        private (double score, Move move) AlphaBetaSearch(int depth, double alpha, double beta, int turnIndex, Stopwatch stopwatch)
        {
            Move bestMove = new Move();
            if(depth == 0 || stopwatch.ElapsedMilliseconds > _searchTime)
            {
                return (_evaluation.Evaluate(new BoardAnalysis(turnIndex, _board, _skillPairsArray)), bestMove);
            }

            Board.RESULT result = BoardUtil.JudgeResult(_board);
            if(result == Board.RESULT.P1_WIN) return (1000, bestMove);
            if(result == Board.RESULT.P2_WIN) return (-1000, bestMove);

            double bestScore = WorstScore(turnIndex);
            List<SkillPair> nowSkiliPairs = _skillPairsArray[turnIndex];

            foreach(Move move in GetSelectableMoves(_board, nowSkiliPairs).ToList())
            {
                move.Skill.Execute(_board, move.pMove);

                double score = AlphaBetaSearch(depth - 1, alpha, beta, 1-turnIndex, stopwatch).score;
                
                _commandInvoker.UndoLastCommand();

                if(turnIndex == 0)
                {
                    if(bestScore < score){
                        bestScore = score;
                        bestMove = move;
                    }
                    alpha = Math.Max(alpha, score);
                }
                else
                {
                    if(bestScore > score){
                        bestScore = score;
                        bestMove = move;
                    }
                    beta = Math.Min(beta, score);
                }
                if(alpha >= beta) break;
            }
            
            return (bestScore, bestMove);
        }

        private static double WorstScore(int turnIndex) => (turnIndex == 0 ? -1000 : 1000);
        private static IEnumerable<Move> GetSelectableMoves(
            IBoard board, IEnumerable<SkillPair> skillPairs
        )
        {
            foreach(SkillPair pair in skillPairs)
            {
                pair.pClone.SetState(SkillState.SELECTED);
                foreach(Pos move in pair.pClone.GetValidMoves(board))
                {
                    yield return new Move(move, pair);
                }
            }
        }
    }
    
    public struct SkillPair
    {
        public BoardSkill pOrigin;
        public BoardSkill pClone;
        public SkillPair(BoardSkill skill, ICommandInvoker commandInvoker)
        {
            pOrigin = skill;
            pClone = skill.Convert2VirtualSkill(commandInvoker);
        }
    }

    public struct Move
    {
        public Pos pMove;
        public SkillPair pPair;
        public BoardSkill Skill => pPair.pClone;
        public Move(Pos move, SkillPair pair)
        {
            pMove = move;
            pPair = pair;
        }
    }
}
