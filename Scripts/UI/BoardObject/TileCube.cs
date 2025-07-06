using UnityEngine;

namespace Quoridor.UI
{
    public class TileCube : BoardCube
    {
        [SerializeField] UIElement pBaseLight;
        public override void AppearanceElement()
        {
            base.AppearanceElement();
            pBaseLight.AppearanceElement();
        }
    }
}
