using Quoridor.UI;

namespace Quoridor
{
    public enum GameEvent { READY, START, END, UPDATE, TURN_END }

    public interface IMediator
    {
        public void SetFacade(UIFacade uiFacade, UtageFacade utageFacade);
        public void Notify(IColleague colleague, GameEvent gameEvent, object param);
    }

    public interface IColleague { }
}