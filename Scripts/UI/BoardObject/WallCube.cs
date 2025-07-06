using UnityEngine;

namespace Quoridor.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class WallCube : BoardCube
    {
        [SerializeField] private UIElement _wall;

        protected override void Awake()
        {
            _hightLightCube.SetActive(false);
            base.Awake();
        }

        public void BuildWall()
        {
            if(!_wall.gameObject.activeSelf)
            {
                gameObject.GetComponent<AudioSource>().Play();
                _wall.AppearanceElement();
            }
        }

        public void DownWall()
        {
            _wall.ExitElement();
        }
    }
}
