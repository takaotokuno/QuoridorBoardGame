using UnityEngine;
using Quoridor.Board;
using Quoridor.Player;

namespace Quoridor
{
    public abstract class Facade : MonoBehaviour, IColleague
    {
        protected QuoridorDirector _director;
        public GameMode GameModeRef => _director.pGameMode;
        public bool IsScenarioMode => GameModeRef == GameMode.SCENARIO;

        protected IMatch _match;
        protected IBoard BoardRef => _match.BoardRef;
        protected IPlayer[] PlayersRef => _match.PlayersRef;
        protected TurnManager TurnManagerRef => _match.TurnManagerRef;
        protected Mediator MediatorRef => _match.MediatorRef;
        
        public void SetMatch(IMatch match)
        => _match = match;

        public void SetDirector(QuoridorDirector director)
        => _director = director;
    }
}