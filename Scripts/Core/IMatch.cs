using Quoridor.Board;
using Quoridor.Command;
using Quoridor.Player;

namespace Quoridor
{
    public interface IMatch : IColleague
    {
        public IBoard BoardRef { get; }
        public IPlayer[] PlayersRef { get; }
        public TurnManager TurnManagerRef { get; }
        public Mediator MediatorRef { get; }
        public ICommandInvoker CommandInvokerRef { get; }
        public void GameStart();
    }
}