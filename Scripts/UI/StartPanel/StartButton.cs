using UnityEngine;

namespace Quoridor.UI
{
    public class StartButton : UIElement
    {
        private StartPanelManager _manager;
        public void SetManager(StartPanelManager manager) =>  _manager = manager;
        public void StartGame() => _manager?.InformGameStart();
    }
}
