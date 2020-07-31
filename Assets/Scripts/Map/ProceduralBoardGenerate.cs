using UnityEngine;

public class ProceduralBoardGenerate : MonoBehaviour
{
    private static Vector2 _upperRight = new Vector2(8, 8);
    private static Vector2 _upperLeft = new Vector2(1, 8);
    private static Vector2 _bottomRight = new Vector2(8, 1);
    private static Vector2 _bottomLeft = new Vector2(1, 1);

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

    public bool CanDrawWalls { get; set; } = false;
    public bool CanPlaceItems { get; set; } = false;
    public bool CanPlaceEnemies { get; set; } = false;

    public CountScriptable CountWalls { get => _countWalls; }
    public CountScriptable CountTomatoes { get => _countTomatoes; }
    public CountScriptable CountSodas { get => _countSodas; }
    public CountScriptable CountEnemies { get => _countEnemies; }

    private void Awake()
    {                       
        InitBoard();
        
        _drawerBoard = new DrawerBoard(_outerWalls, _walls, _floors, CreateCopy(_countWalls));
        _itemPlacement = new ItemPlacement(_tomatoeModel, _sodaModel, CreateCopy(_countTomatoes), CreateCopy(_countSodas), _parentItems, _maxItems);
        _characterPlacement = new CharacterPlacement(_player, _enemies, CreateCopy(_countEnemies), _width, _heigth);

        GenerateMap();       
    }   
    
    public void GenerateMap()
    {
        _drawerBoard.DrawBoard(_board, CanDrawWalls);
        _characterPlacement.PlaceCharacters(_board, false, CanPlaceEnemies);
        
        if(CanPlaceItems)
            _itemPlacement.PlaceItems(_board);

        if (_characterPlacement.IsEnemiesOnTop)
        {
            if (Random.Range(0, 2) == 0)
                _exit.transform.position = _bottomLeft;
            else
                _exit.transform.position = _bottomRight;
        }
        else
        {
            if (Random.Range(0, 2) == 0)
                _exit.transform.position = _upperRight;
            else
                _exit.transform.position = _upperLeft;
        }
    }
    public bool IsWalkableTile(int x, int y) => OnBounds(x, y) && _board[x, y].IsWalkable;
    public bool IsBreakableTile(int x, int y) => OnBounds(x, y) && _board[x, y].IsBreakable;
    public bool HasEnemy(int x, int y) => _characterPlacement.HasEnemy(x, y);
    public Tile GetTile(int x, int y)
    {
        if (!OnBounds(x, y))
            return null;

        return _board[x, y];
    }
    public void GetPlayerPosition(out int x, out int y) => _characterPlacement.GetPlayerPosition(out x, out y);
    public GameObject[] GetEnemies() => _characterPlacement.EnemiesAtScene.ToArray();    
    
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
    private CountScriptable CreateCopy(CountScriptable count)
    {
        CountScriptable copy = ScriptableObject.CreateInstance<CountScriptable>();

        copy.maximum = count.maximum;
        copy.minimum = count.minimum;
        copy.Count = 0;

        return copy;
    }
    private bool OnBounds(int x, int y) => x >= 0 && y >= 0 || x < _board.GetLength(0) && y < _board.GetLength(1);    
}
