using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour, IObserver
{
    [SerializeField] private GameManager _observerGame = null;
    [SerializeField] private PlayerController _observerPlayer = null;

    [Header("Tutorial Screens")]
    [SerializeField] private TutorialScreen _inputScreen = null;
    [SerializeField] private TutorialScreen _wallScreen = null;
    [SerializeField] private TutorialScreen _enemyScreen = null;
    [SerializeField] private TutorialScreen _foodScreen = null;
    [SerializeField] private TutorialScreen _reachLevel10Screen = null;
    [SerializeField] private TutorialScreen _reachLevel15Screen = null;
    [SerializeField] private TutorialScreen _firstRespawnPoint = null;
    [SerializeField] private TutorialScreen _firstDeathScreen = null;

    private bool _isUnlockInput;
    private bool _isUnlockWall;
    private bool _isUnlockEnemy;
    private bool _isReacheLevel10;
    private bool _isReachLevel15;
    private bool _isUnlockFood;
    private bool _isUnlockRespawn;
    private bool _isUnlockFirstDeath;

    private void Start()
    {
        _observerGame.AddObserverOnPlaceEnemies(this);
        _observerGame.AddObserverOnPlaceFood(this);
        _observerGame.AddObserverOnPlaceWalls(this);
        _observerGame.AddObserverOnReachLevel10(this);
        _observerGame.AddObserverOnReachLevel15(this);        
        _observerGame.AddObserverOnFirstRespawnPoint(this);

        _observerPlayer.AddObserverOnFirstInput(this);
        _observerPlayer.AddObserverOnFirstDeath(this);
    }

    public void OnNotify(ObserverEvent obsEvent)
    {
        switch (obsEvent)
        {
            case ObserverEvent.FirstInput:
                UnlockInput();
                break;
            case ObserverEvent.FirstWall:
                UnlockFirstWall();               
                break;
            case ObserverEvent.FirstEnemy:
                UnlockFirstEnemy();               
                break;
            case ObserverEvent.FirstLevel10:
                UnlockFirstLevel10();               
                break;
            case ObserverEvent.FirstFood:
                UnlockFood();               
                break;
            case ObserverEvent.FirstLevel15:
                UnlockFirstLevel15();                
                break;
            case ObserverEvent.FirstRespawnPoint:
                UnlockRespawnPoint();
                break;
            case ObserverEvent.FirstDeath:
                UnlockFirstDeath();
                break;
        }
    }
    public void OnInput()
    {
        if (_inputScreen.IsEnable)
            _inputScreen.ScreenManager();
        
        else if (_wallScreen.IsEnable)
            _wallScreen.ScreenManager();
        
        else if (_enemyScreen.IsEnable)
            _enemyScreen.ScreenManager();
        
        else if (_foodScreen.IsEnable)
            _foodScreen.ScreenManager();
        
        else if (_reachLevel10Screen.IsEnable)
            _reachLevel10Screen.ScreenManager();
        
        else if (_firstDeathScreen.IsEnable)
            _firstDeathScreen.ScreenManager();
        
        else if (_firstRespawnPoint.IsEnable)
            _firstRespawnPoint.ScreenManager();
    }
    public void ActivePlayerTurn() => _observerGame.PlayerTurnON();

    private void UnlockInput()
    {
        if (_isUnlockInput)
            return;

        _observerGame.PlayerTurnOFF();
        _inputScreen.EnableScreen();

        _observerPlayer.RemoveObserverOnFirstInput(this);
        _isUnlockInput = true;
    }
    private void UnlockFirstWall()
    {
        if (_isUnlockWall)
            return;


        _observerGame.PlayerTurnOFF();
        StartCoroutine(EnableScreenCoroutine(_wallScreen));

        _observerGame.RemoveObserverOnPlaceWalls(this);
        _isUnlockWall = true;
    }
    private void UnlockFirstEnemy()
    {
        if (_isUnlockEnemy)
            return;

        _observerGame.PlayerTurnOFF();
        StartCoroutine(EnableScreenCoroutine(_enemyScreen));

        _observerGame.RemoveObserverOnPlaceEnemies(this);
        _isUnlockEnemy = true;
    }
    private void UnlockFirstLevel10()
    {
        if (_isReacheLevel10)
            return;

        _observerGame.PlayerTurnOFF();
        StartCoroutine(EnableScreenCoroutine(_reachLevel10Screen));

        _observerGame.RemoveObserverOnReachLevel10(this);
        _isReacheLevel10 = true;
    }
    private void UnlockFood()
    {
        if (_isUnlockFood)
            return;

        _observerGame.PlayerTurnOFF();
        StartCoroutine(EnableScreenCoroutine(_foodScreen));

        _observerGame.RemoveObserverOnPlaceFood(this);
        _isUnlockFood = true;
    }
    private void UnlockFirstLevel15()
    {
        if (_isReachLevel15)
            return;

        _observerGame.PlayerTurnOFF();
         StartCoroutine(EnableScreenCoroutine(_reachLevel15Screen));

        _observerGame.RemoveObserverOnReachlevel15(this);
        _isReachLevel15 = true;
    }
    private void UnlockRespawnPoint()
    {
        if (_isUnlockRespawn)
            return;

        _observerGame.PlayerTurnOFF();
        StartCoroutine(EnableScreenCoroutine(_firstRespawnPoint));

        _observerGame.RemoveObserverOnFirstRespawnPoint(this);
        _isUnlockRespawn = true;
    }
    private void UnlockFirstDeath()
    {
        if (_isUnlockFirstDeath)
            return;

        _observerGame.PlayerTurnOFF();
        StartCoroutine(EnableScreenCoroutine(_firstDeathScreen));

        _observerPlayer.RemoveObserverOnFirstDeath(this);
        _isUnlockFirstDeath = true;
    }
    private IEnumerator EnableScreenCoroutine(TutorialScreen screen)
    {
        yield return new WaitForSeconds(1f);

        screen.EnableScreen();
    }
}
