using UnityEngine;
using TMPro;
using Quoridor.Board;
using Quoridor.Player;

namespace Quoridor.UI
{
    public class CanvasParameterUIManager : UIManager
    {
        [SerializeField] private Parameter pParameterP1;
        [SerializeField] private Parameter pParameterP2;

        public override void AppearanceElement()
        {
            gameObject.SetActive(true);
            pParameterP1.AppearanceElement();
            pParameterP2.AppearanceElement();
        }

        public void UpdateDisplay(IBoard board, IPlayerView[] players)
        {
            int[] distances = BoardUtil.CalculateDistances(board);
            pParameterP1.UpdateDisplay(distances[0], players[0]);
            pParameterP2.UpdateDisplay(distances[1], players[1]);
        }
    }
}