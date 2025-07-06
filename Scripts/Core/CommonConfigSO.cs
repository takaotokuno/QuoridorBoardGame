using UnityEngine;

namespace Quoridor
{
    [CreateAssetMenu(fileName = "CommonConfig", menuName = "Config/Create New Common Config")]
    public class CommonConfigSO : ScriptableObject
    {
        public int pTileGridSize;
        public int FullGridSize => pTileGridSize * 2 - 1;
    }
}