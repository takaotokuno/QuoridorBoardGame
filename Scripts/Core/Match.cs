using Quoridor.Board;
using Quoridor.Command;
using Quoridor.Player;

namespace Quoridor
{
    public class Match : IMatch
    {
        private IBoard _board;
        private IPlayer[] _players;
        private TurnManager _turnManager;
        private Mediator _mediator;
        private NormalCommandInvoker _commandInvoker;

        public IBoard BoardRef => _board;
        public IPlayer[] PlayersRef => _players;
        public TurnManager TurnManagerRef => _turnManager;
        public Mediator MediatorRef => _mediator;
        public ICommandInvoker CommandInvokerRef => _commandInvoker;

        public Match(CommonConfigSO configSO)
        {
            _mediator = new Mediator(this);
            _commandInvoker = new NormalCommandInvoker();

            int boardSize = configSO.FullGridSize;
            _board = new BoardState(boardSize);
            _board.Initialize();

            PlayersFactory playersFactory = new PlayersFactory(_board, _mediator, _commandInvoker);
            _players = playersFactory.CreatePlayers();

            _turnManager = new TurnManager(this);
            _turnManager.SetMediator(_mediator);
        }

        public void GameStart()
        {
            _mediator.Notify(this, GameEvent.READY, null);
            _mediator.Notify(this, GameEvent.START, null);
        }
    }
}