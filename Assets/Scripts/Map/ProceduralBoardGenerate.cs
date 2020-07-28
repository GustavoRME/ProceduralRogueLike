using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralBoardGenerate : MonoBehaviour
{
    private Tile[,] _board;
    
    [Header("Board Settings")]
    [SerializeField] private int _width = 0;
    [SerializeField] private int _heigth = 0;
    [SerializeField] private int _maxItems = 2;

    [Header("Characters")]
    [SerializeField] private GameObject _player = null;
    [SerializeField] private GameObject[] _enemies = null;

    [Header("Models")]
    [SerializeField] private GameObject _tileModel = null;
    [SerializeField] private GameObject _tomatoeModel = null;
    [SerializeField] private GameObject _sodaModel = null;
    [SerializeField] private GameObject _exit = null;

    [Header("Counts")]
    [SerializeField] private CountScriptable _countWalls = null;
    [SerializeField] private CountScriptable _countTomatoes = null;
    [SerializeField] private CountScriptable _countSodas = null;
    [SerializeField] private CountScriptable _countEnemies = null;

    [Header("Tiles")]
    [SerializeField] private TileScriptable[] _outerWalls = null;
    [SerializeField] private TileScriptable[] _walls = null;
    [SerializeField] private TileScriptable[] _floors = null;

    [Space]
    [SerializeField] private Transform _parentTile = null;
    [SerializeField] private Transform _parentItems = null;
    
    private DrawerBoard _drawerBoard;
    private ItemPlacement _itemPlacement;
    private CharacterPlacement _characterPlacement;
    
    public bool aa = false;    

    private void Awake()
    {                       
        InitBoard();        

        _drawerBoard = new DrawerBoard(_outerWalls, _walls, _floors, _countWalls);
        _itemPlacement = new ItemPlacement(_tomatoeModel, _sodaModel, _countTomatoes, _countSodas, _parentItems, _maxItems);
        _characterPlacement = new CharacterPlacement(_player, _enemies, _countEnemies, _width, _heigth);

        _drawerBoard.DrawBoard(_board);
        _itemPlacement.PlaceItems(_board);
        _characterPlacement.PlaceCharacters(_board, false);

        //Place exit according with player position
        if(_characterPlacement.IsTop)
        {
            //Thinking...
        }
        else
        {
            _exit.transform.position = new Vector3(8, 8);
        }
    }
   
    private void OnEnable()
    {
        if(aa)
        {
            _drawerBoard.DrawBoard(_board);
            _itemPlacement.PlaceItems(_board);
            _characterPlacement.PlaceCharacters(_board, false);
        }
    }   

    private void InitBoard()
    {
        _board = new Tile[_width, _heigth];

        for (int x = 0; x < _board.GetLength(0); x++)
        {
            for (int y = 0; y < _board.GetLength(1); y++)
            {
                var instace = Instantiate(_tileModel, new Vector3(x, y, 0f), Quaternion.identity, _parentTile);

                _board[x, y] = new Tile(instace.GetComponent<SpriteRenderer>(), instace.GetComponent<AudioSource>());
            }
        }
    }

    private bool OnBounds(int x, int y) => x >= 0 && y >= 0 || x < _board.GetLength(0) && y < _board.GetLength(1);

    public bool IsWalkableTile(int x, int y) => OnBounds(x, y) && _board[x, y].IsWalkable;

    public bool IsBreakableTile(int x, int y) => OnBounds(x, y) && _board[x, y].IsBreakable;

    public Tile GetTile(int x, int y)
    {
        if (!OnBounds(x, y))
            return null;

        return _board[x, y];
    }
    
}
