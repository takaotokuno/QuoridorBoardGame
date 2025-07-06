using System;
using System.Collections.Generic;
using System.Text;
using Quoridor.Board;
using System.Linq;

namespace Quoridor.Logic
{
    public class PathCacheManager
    {
        private Dictionary<string, int[]> _pathCache;
        private Queue<string> _cacheKeys;
        private int _cacheLimit;
        private string _hash;

        public PathCacheManager()
        {
            _pathCache = new Dictionary<string, int[]>();
            _cacheKeys = new Queue<string>();
            _cacheLimit = 1000;
        }

        public bool JudgeCachedDistance(int[,] board, (int, int)[] pawns)
        {
            _hash = ConvertBoard2Cache(board, pawns);
            return _pathCache.ContainsKey(_hash);
        }

        public int[] GetCachedDistance()
        {
            return _pathCache[_hash];
        }

        public void AddToCache(int firstDistance, int secondDistance)
        {
            if(firstDistance < 0 || secondDistance < 0) return;

            if (_pathCache.Count >= _cacheLimit)
            {
                string oldKey = _cacheKeys.Dequeue();
                _pathCache.Remove(oldKey);
            }
            _pathCache[_hash] = new int[2]{firstDistance, secondDistance};
            _cacheKeys.Enqueue(_hash);
        }

        private string ConvertBoard2Cache(int[,] board, (int x, int y)[] pawns)
        {
            return String.Join("", board.Cast<int>()) + (pawns[0].x * 83521 + pawns[0].y * 289 + pawns[1].x * 17 + pawns[1].y).ToString();
        }
    }
}
