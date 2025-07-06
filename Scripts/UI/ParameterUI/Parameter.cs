using UnityEngine;
using TMPro;
using Quoridor.Player;

namespace Quoridor.UI
{
    public class Parameter : UIElement
    {
        [SerializeField] private TextMeshProUGUI pDistance;
        [SerializeField] private TextMeshProUGUI pDefence;

        public void UpdateDisplay(int distance, IPlayerView player)
        {
            pDistance.text = distance.ToString();
            pDefence.text = player.RemainWalls.ToString();
        }
    }
}