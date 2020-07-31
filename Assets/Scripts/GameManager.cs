using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    private const int LEVEL_PLACE_WALLS = 3;
    private const int LEVEL_PLACE_FOOD = 4;
    private const int LEVEL_PLACE_ENEMIES = 5;
    private const int LEVEL_INCREASE_MAX = 10;
    private const int LEVEL_INCREASE_MIN = 15;
    private const int LEVEL_RESPAWN_POINT = 20;

    [SerializeField] private ProceduralBoardGenerate _proceduralBoard = null;
    [SerializeField] private ScreenFader _fader = null;
    [SerializeField] private PlayerController _player = null;

    [Header("Game Settings")]
    [Tooltip("Max from enemies can have on scene during the entire game")]
    [SerializeField] private int _maxEnemies = 6;

    [Tooltip("Max from foods can have on scene during the entire game")]
    [SerializeField] private int _maxFoods = 4;   

    [Tooltip("Each 10 levels increase the max amount of enemies and food")]
    [SerializeField] private int _maxAmountIncrease = 1;

    [Tooltip("Each 15 levels increase the min amount of enemies nad food")]
    [SerializeField] private int _minAmountIncrease = 1;

    [Tooltip("Which level start a randomize player and exit sign position")]
    [SerializeField] private int _levelNumberRandomize = 30;

    private static readonly Subject _subject = new Subject();

    private List<RogueAI> _rogues;

    private CountScriptable _countWalls;
    private CountScriptable _countEnemies;
    private CountScriptable[] _countFoods;

    private int _currentLevel;
    private int _respawnPoint;

    private void Start()
    {
        _currentLevel = 1;
        _respawnPoint = 1;

        _countWalls = _proceduralBoard.CountWalls;
        _countEnemies = _proceduralBoard.CountEnemies;
        _countFoods = new CountScriptable[]
        {
            _proceduralBoard.CountTomatoes,
            _proceduralBoard.CountSodas
        };
        
        _rogues = new List<RogueAI>();

        GetRoguesAtScene();
    }

    #region Called By Events
    public void TurnControl()
    {
        foreach (RogueAI rogue in _rogues)
            rogue.MakeAction();

        _player.SetTurn(true);
    }
    public void GenereteMap()
    {
        _currentLevel++;

        _player.SetTurn(false);
        
        _fader.FadeOut(_currentLevel);
        
        UpdateLevel();

        _proceduralBoard.GenerateMap();
     
        GetRoguesAtScene();
    }
    public void RestartMap()
    {
        _currentLevel = _respawnPoint;
        
        _fader.FadeOut(_currentLevel);
        
        _proceduralBoard.GenerateMap();
        
        GetRoguesAtScene();

        _player.gameObject.SetActive(true);
        PlayerTurnOFF();
    }
    #endregion

    public void PlayerTurnON() => _player.SetTurn(true);
    public void PlayerTurnOFF() => _player.SetTurn(false);
    public void AddObserverOnPlaceWalls(IObserver observer) => AddObserver(observer);
    public void AddObserverOnPlaceEnemies(IObserver observer) => AddObserver(observer);
    public void AddObserverOnPlaceFood(IObserver observer) => AddObserver(observer);
    public void AddObserverOnReachLevel10(IObserver observer) => AddObserver(observer);
    public void AddObserverOnReachLevel15(IObserver observer) => AddObserver(observer);
    public void AddObserverOnFirstRespawnPoint(IObserver observer) => AddObserver(observer);
    public void RemoveObserverOnPlaceWalls(IObserver observer) => RemoveObserver(observer);
    public void RemoveObserverOnPlaceEnemies(IObserver observer) => RemoveObserver(observer);
    public void RemoveObserverOnPlaceFood(IObserver observer) => RemoveObserver(observer);
    public void RemoveObserverOnReachLevel10(IObserver observer) => RemoveObserver(observer);
    public void RemoveObserverOnReachlevel15(IObserver observer) => RemoveObserver(observer);
    public void RemoveObserverOnFirstRespawnPoint(IObserver observer) => RemoveObserver(observer);
    

    private void UpdateLevel()
    {
        if (_currentLevel == LEVEL_PLACE_WALLS)
        {
            _subject.Notify(ObserverEvent.FirstWall);
            _proceduralBoard.CanDrawWalls = true;
        }
        else if (_currentLevel == LEVEL_PLACE_FOOD)
        {
            _subject.Notify(ObserverEvent.FirstFood);
            _proceduralBoard.CanPlaceItems = true;

        }
        else if (_currentLevel == LEVEL_PLACE_ENEMIES)
        {
            _subject.Notify(ObserverEvent.FirstEnemy);
            _proceduralBoard.CanPlaceEnemies = true;
        }
        else if(_currentLevel == LEVEL_INCREASE_MAX)
        {
            _subject.Notify(ObserverEvent.FirstLevel10);            
        }
        else if(_currentLevel == LEVEL_INCREASE_MIN)
        {
            _subject.Notify(ObserverEvent.FirstLevel15);
        }
        else if(_currentLevel % LEVEL_RESPAWN_POINT == 0)
        {
            _respawnPoint = _currentLevel;
            _subject.Notify(ObserverEvent.FirstRespawnPoint);
        }

        if(!IsArriveMaximunFoodAndItems())
        {
            //Each 10 levels increase the MAX amount of enemies and food
            if(_currentLevel % LEVEL_INCREASE_MAX == 0)
            {
                _countEnemies.maximum += _maxAmountIncrease;

                foreach (CountScriptable food in _countFoods)
                    food.maximum += _maxAmountIncrease;
            }

            //Each 15 levels increase the min amount of enemies and food
            if(_currentLevel % LEVEL_INCREASE_MIN == 0)
            {
                _countEnemies.minimum += _minAmountIncrease;

                foreach (CountScriptable food in _countFoods)
                    food.minimum += _minAmountIncrease;
            }

            if(_currentLevel == _levelNumberRandomize)
            {
                //Thinking about logic
            }
        }
    }
    private bool IsArriveMaximunFoodAndItems()
    {
        bool arrived = false;

        if(_countEnemies.maximum == _maxEnemies)
        {
            foreach (CountScriptable food in _countFoods)
                arrived = food.maximum == _maxFoods ? true : false;
        }

        return arrived;
    }
    private void GetRoguesAtScene()
    {
        if (_rogues.Count > 0)
            _rogues.Clear();

        foreach (var go in _proceduralBoard.GetEnemies())
        {
            RogueAI rogue = go.GetComponent<RogueAI>();

            _rogues.Add(rogue);
        }
    }
    private void AddObserver(IObserver observer) => _subject.AddObserver(observer);
    private void RemoveObserver(IObserver observer) => _subject.RemoveObserver(observer);
}
