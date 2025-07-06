using Quoridor.Board;

namespace Quoridor.Player
{
    public class CPUController
    {
        private IBoard _board;
        private IPlayer _player;
        private IPlayer _opponent;
        private ModifiableInt _aiLevel;
        private SearchMove _searchMove;
        private Move _bestMove;

        private int SearchTime => _aiLevel.Value * 50;

        public CPUController(PlayerConfigSO config, IBoard board, IPlayer player, IPlayer opponent)
        {
            _board = board;
            _player = player;
            _opponent = opponent;
            _aiLevel = new ModifiableInt(config.pCpuLevel);
            _searchMove = new SearchMove(config.pEvaluationFunc);
        }

        public void SetCpuLevel(int relative, int absolute)
        {
            _aiLevel.pRelative += relative;
            _aiLevel.pAbsolute = absolute;
        }

        public void SearchBestMove()
        {
            _searchMove.SetCalculationConditions(_board, _player, _opponent, SearchTime);
            _bestMove = _searchMove.SearchBestMove();
        }

        public void InstructMove()
        {
            _bestMove.pPair.pOrigin.Execute();
            _bestMove.pPair.pOrigin.Execute(_board, _bestMove.pMove);
        }
    }
}