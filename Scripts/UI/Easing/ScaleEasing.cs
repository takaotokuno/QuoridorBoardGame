using UnityEngine;
using DG.Tweening;

namespace Quoridor.UI
{
    public class ScaleEasing : Easing
    {
        [SerializeField] protected Vector3 pLocalScale;
        protected Vector3 _baseScale; 

        public override void DOMoveFrom() // 指定ポジションに移動して、そこから現在位置まで戻る
        {
            _baseScale = gameObject.transform.localScale;
            gameObject.transform.localScale = pLocalScale;
            DOMove(_baseScale);
        }

        public override void DOMoveTo() // 現在位置から指定ポジションまで移動する
        {
            _baseScale = gameObject.transform.localScale;
            DOMove(pLocalScale);
        }

        protected virtual void DOMove(Vector3 scale)
        {
            gameObject.SetActive(true);
            gameObject.transform.DOScale(scale, pTime).SetEase(pEase);
        }
    }
}