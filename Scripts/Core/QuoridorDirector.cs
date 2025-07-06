using System.Collections.Generic;
using UnityEngine;

namespace Quoridor
{
    public enum GameMode {SCENARIO, BOARD_GAME, LERNING, DEBUG}

    public class QuoridorDirector : MonoBehaviour
    {
        // シングルインスタンス
        public static QuoridorDirector s_instance;

        // 外部入力値
        public CommonConfigSO pConfigSO;
        public GameMode pGameMode;

        // コンポジット
        private MatchFactory _matchFactory;
        private List<IMatch> _matches;

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
                DontDestroyOnLoad(gameObject);

                _matchFactory = new MatchFactory(this);
                _matches = new List<IMatch>();
            }
            else Destroy(gameObject);
        }

        private void Start() => CreateMatch();

        public void CreateMatch()
        {
            IMatch match = _matchFactory.CreateMatch(pGameMode);
            if (pGameMode == GameMode.SCENARIO) _matches.Clear();
            _matches.Add(match);
        }
    }
}
