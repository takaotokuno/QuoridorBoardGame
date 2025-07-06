using Quoridor.Board;
using Quoridor.Player;
using Quoridor.UI;

namespace Quoridor
{
    public class Mediator : IMediator
    {
        private Match _match;
        private IPlayer[] PlayersRef => _match.PlayersRef;
        private TurnManager TurnManagerRef => _match.TurnManagerRef;
        private UIFacade _uiFacade;
        private UtageFacade _utageFacade;

        private Mediator() { }
        public Mediator(Match match) => _match = match;

        public void SetFacade(UIFacade uiFacade, UtageFacade utageFacade)
        {
            _uiFacade = uiFacade;
            _utageFacade = utageFacade;
        }

        public void Notify(IColleague colleague, GameEvent gameEvent, object param)
        {
            switch (gameEvent)
            {
                case GameEvent.READY:
                    if (param is IStartConfig config) GameReady(config);
                    break;

                case GameEvent.START:
                    TurnManagerRef.OnGameStart();
                    _uiFacade?.OnGameStart();
                    break;

                case GameEvent.END:
                    if (param is RESULT result) GameEnd(result);
                    break;

                case GameEvent.TURN_END:
                    TurnEnd();
                    break;

                case GameEvent.UPDATE:
                    _uiFacade?.UpdateUI();
                    break;

                default:
                    UnityEngine.Debug.LogWarning($"Unhandled GameEvent: {gameEvent}");
                    break;
            }
        }

        private void GameReady(IStartConfig config)
        {
            PlayerInitializer.SetPlayersConfig(PlayersRef, config);
            TurnManagerRef.Initialize(config);
            _uiFacade?.SetPlayersConfig(config);
        }

        private void GameEnd(RESULT result)
        {
            PlayersRef[0].OnGameEnd();
            PlayersRef[1].OnGameEnd();
            TurnManagerRef.OnGameEnd();
            _uiFacade?.OnGameEnd(result);
            _utageFacade?.OnGameEnd(result);
        }

        private void TurnEnd()
        {
            _utageFacade?.OnTurnEnd();
            TurnManagerRef.OnTurnEnd();
            _uiFacade?.UpdateUI();
        }
    }
}
