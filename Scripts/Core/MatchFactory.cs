using System.Threading.Tasks;
using Quoridor.UI;

namespace Quoridor
{
    public class MatchFactory
    {
        public QuoridorDirector _director;
        private UIFacade UIFacadeRef => ConcreteUIFacade.p_sInstance;
        private UtageFacade UtageFacadeRef => UtageFacade.p_sInstance;

        public MatchFactory(QuoridorDirector director)
        => _director = director;

        public IMatch CreateMatch(GameMode gameMode)
        {
            Match match = new Match(_director.pConfigSO);

            if (gameMode == GameMode.LERNING)
            {
                Task.Run(() => match.GameStart());
            }
            else
            {
                match.MediatorRef.SetFacade(UIFacadeRef, UtageFacadeRef);

                UIFacadeRef?.SetMatch(match);
                UIFacadeRef?.SetDirector(_director);

                UtageFacadeRef?.SetMatch(match);
                UtageFacadeRef?.SetDirector(_director);
            }

            if(gameMode == GameMode.BOARD_GAME)
            {
                UtageFacadeRef?.JumpScenario("Quoridor");
            }
            
            return match;
        }
    }
}