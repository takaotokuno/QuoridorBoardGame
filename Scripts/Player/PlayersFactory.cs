using Quoridor.Board;
using Quoridor.Command;

namespace Quoridor.Player
{
    public class PlayersFactory
    {
        private PlayerController[] _players;
        private IBoard _board;
        private IMediator _mediator;
        private ICommandInvoker _commandInvoker;

        private PlayersFactory() { }
        public PlayersFactory(IBoard board, IMediator mediator, ICommandInvoker commandInvoker)
        {
            _board = board;
            _mediator = mediator;
            _commandInvoker = commandInvoker;
        }

        public IPlayer[] CreatePlayers()
        {
            _players = new PlayerController[2];

            SetPlayer(0);
            SetPlayer(1);

            return _players;
        }

        public void SetPlayer(int index)
        {
            _players[index] = new PlayerController(_board, index);
            _players[index].SetExternalInfo(_players[1 - index], _commandInvoker);
            _players[index].SetMediator(_mediator);
        }
    }
}
