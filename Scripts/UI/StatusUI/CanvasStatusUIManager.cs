using System.Collections.Generic;
using UnityEngine;
using Quoridor.Player;

namespace Quoridor.UI
{
    public class CanvasStatusUIManager : UIManager
    {
        // prefab
        [SerializeField] private GameObject _statusPrefab;

        // game objects
        [SerializeField] private UIElement pStatusViewP1;
        [SerializeField] private UIElement pStatusViewP2;
        private Dictionary<string, GameObject> _player1StatusObjects;
        private Dictionary<string, GameObject> _player2StatusObjects;

        protected override void Awake()
        {
            _player1StatusObjects = new Dictionary<string, GameObject>();
            _player2StatusObjects = new Dictionary<string, GameObject>();
            base.Awake();
        }

        public override void AppearanceElement()
        {
            base.AppearanceElement();
            pStatusViewP1.AppearanceElement();
            pStatusViewP2.AppearanceElement();
        }

        // 表示更新
        public void UpdateDisplay(IPlayerView[] players)
        {
            UpdateStatusPanel(pStatusViewP1, _player1StatusObjects, players[0]);
            UpdateStatusPanel(pStatusViewP2, _player2StatusObjects, players[1]);
        }

        private void UpdateStatusPanel(
            UIElement panel
            , Dictionary<string, GameObject> displayStatusObjects
            , IPlayerView player
        )
        {
            List<string> activeStatusList 
            = player.ActiveStatusList.ConvertAll((status) => status.Name);

            // 有効でないステータスは、オブジェクト削除 + Dictionaryから除去
            List<string> displayStatusList = new List<string>(displayStatusObjects.Keys);
            foreach(string statusName in displayStatusList)
            {
                if(!activeStatusList.Contains(statusName))
                {
                    Destroy(displayStatusObjects[statusName]);
                    displayStatusObjects.Remove(statusName);
                }
            }

            // 追加されたステータスは、オブジェクト作成 + Dictionaryに追加
            foreach(string statusName in activeStatusList)
            {
                if(!displayStatusObjects.ContainsKey(statusName))
                {
                    GameObject obj = InstantiateStatusIcon(panel, statusName);
                    displayStatusObjects.Add(statusName, obj);
                }
            }

            // ステータスオブジェクトの位置を調整
            for(int i = 0; i < displayStatusObjects.Count; i++)
            {
                GameObject obj = displayStatusObjects[activeStatusList[i]];
                obj.transform.localPosition
                = new Vector3(20 * (-displayStatusObjects.Count + i * 2 + 1), 0, 0);
            }
        }

        private GameObject InstantiateStatusIcon(UIElement panel, string statusName)
        {
            GameObject obj = Instantiate(_statusPrefab, panel.gameObject.transform);
            // Status別表示の反映 status.Name から画像を取得できるようにする
            return obj;
        }
    }
}
