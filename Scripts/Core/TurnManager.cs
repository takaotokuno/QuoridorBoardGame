using Quoridor.Board;

namespace Quoridor
{
    public enum TurnLimitMode {LIMITED, UNLIMITED}

    public class TurnManager : IColleague
    {
        private Match _match;

        private bool _inGame;
        private int _turnIndex;
        private int _turnCount;
        private int _maxTurn;
        private TurnLimitMode _turnLimitMode;
        private RESULT _result;
        private IMediator _mediator;

        // Getter
        public int TurnIndex => _turnIndex;
        public TurnLimitMode Mode => _turnLimitMode;
        public RESULT ResultRef => _result;

        private TurnManager() { }
        public TurnManager(Match match) => _match = match;

        public void SetMediator(IMediator mediator) => _mediator = mediator;

        public void Initialize(IStartConfig config)
        {
            _turnIndex = 1 - config.FirstTurnIndex;
            _turnCount = 0;
            _maxTurn = config.MaxTurn;
            _turnLimitMode = (_maxTurn > 0) ? TurnLimitMode.LIMITED : TurnLimitMode.UNLIMITED;
        }

        public void OnGameStart()
        {
            _inGame = true;
            OnTurnEnd();
        }

        public void OnGameEnd() => _inGame = false;

        public int TurnCount()
        {
            int count = (_turnCount + 1) / 2;
            return (_turnLimitMode == TurnLimitMode.LIMITED) ? _maxTurn - count + 1 : count;
        }

        public void OnTurnEnd()
        {
            if (!_inGame) return;

            _result = BoardUtil.JudgeResult(_match.BoardRef, _match.PlayersRef);

            if (_result == RESULT.P1_WIN)
            {
                _mediator.Notify(this, GameEvent.END, RESULT.P1_WIN);
            }
            else if (_result == RESULT.P2_WIN || TurnCount() < 0)
            {
                _mediator.Notify(this, GameEvent.END, RESULT.P2_WIN);
            }
            else
            {
                NextTurn();
                _match.PlayersRef[_turnIndex].OnTurnStart();
            }
        }

        public void NextTurn() => SwitchTurn(1);
        public void BackTurn() => SwitchTurn(-1);
        private void SwitchTurn(int amount)
        {
            _turnIndex = 1 - _turnIndex;
            _turnCount += amount;
        }

        public void SwitchInGame(bool inGame) => _inGame = inGame;
    }
}