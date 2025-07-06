using UnityEngine;
using Quoridor.Board;

namespace Quoridor.UI
{
    public class BoardObjectManager : UIManager
    {
        // 外部入力値
        public CommonConfigSO _configSO;

        // prefab
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private GameObject _wallHorizontalPrefab;
        [SerializeField] private GameObject _wallVirticalPrefab;
        [SerializeField] private GameObject _frontPlayerPawnPrefab;
        [SerializeField] private GameObject _backPlayerPawnPrefab;

        // game board object
        private TileCube[,] _tileCubes;
        private WallCube[,] _wallCubes;
        private Pawn[] _pawns;

        private int BoardSize => _configSO.FullGridSize;

        protected override void Awake()
        => SetActive(true);

        private void Start()
        {
            InstantiateBoard();
            InstantiatePawns();
        }
        
        // ボード用オブジェクト作成
        private void InstantiateBoard()
        {
            int boardSize = BoardSize;

            // define tile and wall objects
            _wallCubes = new WallCube[boardSize, boardSize];
            _tileCubes = new TileCube[boardSize, boardSize];

            float BOARD_OFFSET = -(boardSize - 1)/8 - 1.75f;

            for(int row = 0; row < boardSize; row++)
            {
                for(int col = 0; col < boardSize; col++)
                {
                    bool IsTileCoordinates = BoardUtil.IsTileCoordinates(col, row);
                    bool IsWallCoordinates = BoardUtil.IsWallCoordinates(col, row);

                    if(IsTileCoordinates || IsWallCoordinates)
                    {
                        BoardCube cube;
                        if(IsTileCoordinates)
                        {
                            cube = InstantiateTile(col, row, BOARD_OFFSET);
                        }
                        else
                        {
                            cube = InstantiateWall(col, row, BOARD_OFFSET);
                        }
                        
                        cube.SetCoordinate(col, row);
                        cube.SetManager(this);
                    }
                }
            }
        }

        private BoardCube InstantiateTile(int col, int row, float adjustment)
        {
            string name = $"Tile_{col}_{row}";
            BoardCube tile = InstantiateCube(_tilePrefab, _tileCubes, col, row, name);
            tile.gameObject.transform.localPosition = new Vector3(col/2 + adjustment, row/2 + adjustment, 0);
            return tile;
        }

        private BoardCube InstantiateWall(int col, int row, float adjustment)
        {
            string name = $"Wall_{col}_{row}";
            BoardCube wall;
            if(col % 2 == 1 && row % 2 == 0)
            {
                wall = InstantiateCube(_wallVirticalPrefab, _wallCubes, col, row, name);
                wall.gameObject.transform.localPosition = new Vector3(col/2 + adjustment + 0.5f, row/2 + adjustment, 0);
            }
            else
            {
                wall = InstantiateCube(_wallHorizontalPrefab, _wallCubes, col, row, name);
                wall.gameObject.transform.localPosition = new Vector3(col/2 + adjustment, row/2 + adjustment + 0.5f, 0);
            }
            return wall;
        }

        private BoardCube InstantiateCube(GameObject prefab, BoardCube[,] list, int col, int row, string name)
        {
            GameObject obj = Instantiate(prefab, gameObject.transform);
            BoardCube cube = obj.GetComponent<BoardCube>();
            list[col, row] = cube;
            obj.name = name;
            return cube;
        }

        private void InstantiatePawns()
        {
            _pawns = new Pawn[2];
            InstantiatePawn(_frontPlayerPawnPrefab, 0, "Pawn_FrontPlayer");
            InstantiatePawn(_backPlayerPawnPrefab, 1, "Pawn_BackPlayer");
        }

        private void InstantiatePawn(GameObject prefab, int index, string name)
        {
            GameObject obj = Instantiate(prefab, gameObject.transform);
            _pawns[index] = obj.GetComponent<Pawn>();
            obj.name = name;
        }

        public void AppearanceElement(IBoard board)
        {
            AppearanceElement();
            ShowPawn1(board);
            ShowPawn2(board);
            ShowBoard(board);
        }

        public void ShowBoard(IBoard board)
        {
            int boardSize = BoardSize;

            for(int row = 0; row < boardSize; row++)
            {
                for(int col = 0; col < boardSize; col++)
                {
                    bool IsTileCoordinates = BoardUtil.IsTileCoordinates(col, row);
                    bool IsWallCoordinates = BoardUtil.IsWallCoordinates(col, row);

                    if(IsTileCoordinates)
                    {
                        _tileCubes[col, row].AppearanceElement();
                    }

                    if(IsWallCoordinates)
                    {
                        _wallCubes[col, row].AppearanceElement();
                    }
                }
            }
        }

        public void ShowPawn1(IBoard board)
        => ShowPawn(board, 0);

        public void ShowPawn2(IBoard board)
        => ShowPawn(board, 1);

        private void ShowPawn(IBoard board, int index)
        {
            Pos[] pawns = board.Pawns;
            TileCube tile = _tileCubes[pawns[index].X, pawns[index].Y];
            _pawns[index].AppearanceElement(tile);
        }

        // ボード表示更新
        public void UpdateDisplay(IBoard board)
        {
            int boardSize = BoardSize;

            for(int row = 0; row < boardSize; row++)
            {
                for(int col = 0; col < boardSize; col++)
                {
                    if(BoardUtil.IsWallCoordinates(col, row))
                    {
                        if(board.Grid[col, row] == 0) _wallCubes[col, row].DownWall();
                        else _wallCubes[col, row].BuildWall();
                    }
                }
            }

            Pos[] pawns = board.Pawns;
            for(int i = 0; i < 2; i++)
            {
                _pawns[i].MovePawn(_tileCubes[pawns[i].X, pawns[i].Y]);
            }
        }

        // Mediatorに通知
        public bool CanMove(BoardCube obj, Pos move)
        => _facade.CanMove(this, move);

        public void OnMove(BoardCube obj, Pos move)
        => _facade.OnMove(this, move);
    }
}

