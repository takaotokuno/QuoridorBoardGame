using UnityEngine;
using Quoridor.Board;

namespace Quoridor.UI
{
    public abstract class BoardCube : UIElement
    {
        [SerializeField] protected GameObject _hightLightCube;
        private BoardObjectManager _manager;
        private Pos _pos;
        protected bool IsSelectable => _manager.CanMove(this, _pos);

        protected BoardCube(){}

        protected override void Awake()
        {
            _hightLightCube.SetActive(false);
            base.Awake();
        }

        public void SetCoordinate(int col, int row)
        => _pos = new Pos(col, row);

        public void SetManager(BoardObjectManager manager)
        => _manager = manager;

        public void Highlight()
        {
            if(IsSelectable) _hightLightCube.SetActive(true);
        }

        public void Lowlight()
        =>  _hightLightCube.SetActive(false);

        public void OnMove()
        => _manager.OnMove(this, _pos);
    }
}
