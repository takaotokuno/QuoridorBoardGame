using UnityEngine;
using TMPro;

namespace Quoridor.UI
{
    public class TurnView : UIElement
    {
        [SerializeField] private TextMeshProUGUI pTurnLabel;
        [SerializeField] private TextMeshProUGUI pTurnScreen;

        public void UpdateDisplay(TurnManager turnManager)
        {
            pTurnLabel.text = (turnManager.Mode == TurnLimitMode.LIMITED) ? "Limit" : "Turn";
            pTurnScreen.text = turnManager.TurnCount().ToString();
        }
    }
}