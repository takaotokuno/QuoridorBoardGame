using UnityEngine;
using UnityEngine.UI;

namespace Quoridor.UI
{ 
    [RequireComponent(typeof(Button))]
    public class UIButton : UIElement
    {
        public void SetInteractable(bool isInteractable) //活性・非活性を切り替える
        => gameObject.GetComponent<Button>().interactable = isInteractable;
    }
}