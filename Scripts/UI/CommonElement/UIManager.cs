using System;
using System.Collections.Generic;

namespace Quoridor.UI
{
    public class UIManager : UIElement
    {
        protected UIFacade _facade;
        public void SetFacade(UIFacade facade) => _facade = facade;
        protected IEnumerable<UIElement> ChildElements => GetComponentsInChildren<UIElement>(includeInactive: true);
        
        public virtual void SetPlayersConfig(IStartConfig config) { }

        public void SetActiveAll(bool isActive)
        => ForEachChild(elm => elm.SetActive(isActive));

        public virtual void TryAnimateInAll()
        => ForEachChild(elm => elm.TryAnimateIn());

        public virtual void TryAnimateOutAll()
        => ForEachChild(elm => elm.TryAnimateOut());

        private void ForEachChild(Action<UIElement> action)
        {
            foreach (UIElement elm in ChildElements) action(elm);
        }
    }
}