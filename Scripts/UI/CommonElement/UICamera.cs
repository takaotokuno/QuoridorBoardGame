using UnityEngine;
using UnityEngine.EventSystems;

namespace Quoridor.UI
{ 
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(Physics2DRaycaster))]
    public class UICamera : UIElement
    {
        public void SetRaycastEnabled(bool isEnabled) //活性・非活性を切り替える
        => gameObject.GetComponent<Physics2DRaycaster>().enabled = isEnabled;
    }
}