using UnityEngine;
using DG.Tweening;

namespace Quoridor.UI
{
    public class TileBaseLightEasing : Easing
    {
        [SerializeField] Material pLightMaterial;
        private Color _color;
        private float _intensity;

        private void Awake()
        {
            _color = pLightMaterial.GetColor("_EmissionColor");
        }

        public override void DOMoveFrom()
        {
            gameObject.SetActive(true);
            SetColor(0, 1, pTime).SetEase(pEase);
        } 
        
        public override void DOMoveTo()
        {
            SetColor(1, 0, pTime).SetEase(pEase);
        }

        private Tweener SetColor(float from, float to, float time)
        {
            return DOVirtual.Float(
                from ,to, time
                , value => pLightMaterial.SetColor("_EmissionColor", _color * value)
            );
        }
    }
}