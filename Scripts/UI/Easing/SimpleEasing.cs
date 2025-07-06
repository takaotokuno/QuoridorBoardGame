using UnityEngine;
using DG.Tweening;

namespace Quoridor.UI
{
    public class SimpleEasing : Easing
    {
        [SerializeField] protected Vector3 pLocalPos;
        [SerializeField] protected Vector3 pLocalAngle;
        protected Vector3 _basePos; 
        protected Vector3 _baseAngle;

        private void Awake()
        {
            _basePos = gameObject.transform.localPosition;
            _baseAngle = gameObject.transform.localEulerAngles;
        }

        public void SetBase(Vector3 pos, Vector3 angle)
        {
            _basePos = pos;
            _baseAngle = angle;
        }

        public override void DOMoveFrom() // 指定ポジションに移動して、そこから現在位置まで戻る
        {
            _basePos = gameObject.transform.localPosition;
            _baseAngle = gameObject.transform.localEulerAngles;
            gameObject.transform.localPosition = pLocalPos;
            gameObject.transform.localEulerAngles = pLocalAngle;
            DOMove(_basePos, _baseAngle);
        }

        public override void DOMoveTo() // 現在位置から指定ポジションまで移動する
        => DOMove(pLocalPos, pLocalAngle);

        protected virtual void DOMove(Vector3 pos, Vector3 angle)
        {
            gameObject.SetActive(true);
            gameObject.transform.DOLocalMove(pos, pTime).SetEase(pEase);
            gameObject.transform.DORotate(angle, pTime).SetEase(pEase);
        }
    }
}