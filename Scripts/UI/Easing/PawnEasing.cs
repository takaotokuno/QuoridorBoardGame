using DG.Tweening;
using UnityEngine;

namespace Quoridor.UI
{
    public class PawnEasing : SimpleEasing
    {
        protected override void DOMove(Vector3 pos, Vector3 angle)
        {
            gameObject.SetActive(true);
            gameObject.transform.DOLocalJump(
                pos
                , 1f
                , 1
                , pTime
            );
        }
    }
}