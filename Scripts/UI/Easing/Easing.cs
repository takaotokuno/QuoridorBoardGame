using UnityEngine;
using DG.Tweening;

namespace Quoridor.UI
{
    public abstract class Easing : MonoBehaviour
    {
        [SerializeField] protected float pTime;
        [SerializeField] protected Ease pEase;
        public abstract void DOMoveFrom(); // 指定の状態⇒現在の状態
        public abstract void DOMoveTo(); // 現在の状態⇒指定の状態
    }
}