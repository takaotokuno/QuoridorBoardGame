using UnityEngine;
using DG.Tweening;

namespace Quoridor.UI
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(PawnEasing))]
    public class Pawn : UIElement
    {
        public void AppearanceElement(TileCube tileCube)
        {
            Vector3 basePos = ConvertCube2Pos(tileCube);
            Vector3 angle = tileCube.gameObject.transform.localEulerAngles;
            GetEasing().SetBase(basePos, angle);
            AppearanceElement();
        }

        public void MovePawn(TileCube tileCube)
        {
            Vector3 pos = ConvertCube2Pos(tileCube);
            if(gameObject.transform.localPosition != pos)
            {
                gameObject.transform.DOLocalMove(pos, 0.1f).SetDelay(0.05f).SetEase(Ease.InOutElastic);
                gameObject.GetComponent<AudioSource>().Play();
            }
        }

        private PawnEasing GetEasing() => gameObject.GetComponent<PawnEasing>();

        private Vector3 ConvertCube2Pos(TileCube tileCube)
        {
            Vector3 pos = tileCube.gameObject.transform.localPosition;
            pos.z -= 0.18f;
            pos.y += 0.05f;
            return pos;
        }
    }
}