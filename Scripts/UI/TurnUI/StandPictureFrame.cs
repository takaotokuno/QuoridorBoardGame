using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Quoridor.UI
{
    public class StandPictureFrame : UIElement
    {
        [SerializeField] private Image pThinkingGage;
        private float _thinkingTime;

        public void SetThinkingTime(IStartConfig config)
        {
            _thinkingTime = config.PlayerConfigs[1].pCpuLevel * 50/1000;
        }

        public void TurnStart()
        {
            DOVirtual.Float(
                from: 0
                , to: 1
                , duration: _thinkingTime
                , value => pThinkingGage.fillAmount = value
            );
        }
    }
}