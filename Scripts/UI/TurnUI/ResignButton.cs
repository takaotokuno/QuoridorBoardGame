using UnityEngine.UI;

namespace Quoridor.UI
{
    public class ResignButton : TurnUIButton
    {
        public void OnResign() => _manager?.OnResign(this);

        public override void AppearanceElement()
        {
            gameObject.GetComponent<Button>().interactable  = true;
            base.AppearanceElement();
        }
    }
}