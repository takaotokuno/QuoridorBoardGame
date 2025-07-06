using UnityEngine;
using TMPro;
using Quoridor.Board;

namespace Quoridor.UI
{
    public class ResultView : UIElement
    {
        [SerializeField] private TextMeshProUGUI pResultScreen;

        public void ShowResult(RESULT result)
        {
            pResultScreen.text 
            = (result == RESULT.P1_WIN) ? "YOU WIN!" : "YOU LOSE!";
        }
    }
}