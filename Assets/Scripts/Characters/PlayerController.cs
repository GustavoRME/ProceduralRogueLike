using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Food Settings")]
    [Tooltip("Amount food the player start with it")]
    [SerializeField] private int _startFood = 100;

    [Tooltip("Value for each action player do")]
    [SerializeField] private int _walkFood = 1;

    [Tooltip("Value for broke a wall")]
    [SerializeField] private int _brokeWallFood = 2;

    [Header("Clips")]
    [SerializeField] private AudioClip _dieClip = null;

    [Space]
    [SerializeField] private ProceduralBoardGenerate _proceduralBoard = null;

    [Space]

    [SerializeField] private UnityEvent _OnAction = null;    
    [SerializeField] private LoseFoodEvent _OnChangeFood = null;
    [SerializeField] private UnityEvent _OnStarve = null;
    [SerializeField] private UnityEvent _OnExit = null;

    private Subject _subject;

    private Movement _movement;
    private Breaker _breaker;
    private Animator _anim;    
    
    private bool isLive;
    private bool _isMyTurn;

    public int Food { get ; private set; }

    private void Awake()
    {
        _subject = new Subject();
        
        _movement = GetComponent<Movement>();
        _breaker = GetComponent<Breaker>();
        _anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {        
        isLive = true;
        Food = _startFood;
        
        ChangeFoodAmount();        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Soda") || collision.CompareTag("Food"))
        {
            var food = collision.GetComponent<Food>();

            TakeFood(food.Eat());
        }
        else if(collision.CompareTag("Exit"))
        {
            _OnExit?.Invoke();
        }
    }

    public void InputHandle(InputDirection direction)
    {
        if (!_isMyTurn) //Only play if its turn of player
            return;

        if(direction != InputDirection.None)
        {
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;

            if (direction == InputDirection.Right)
                x++;
            else if (direction == InputDirection.Left)
                x--;
            else if (direction == InputDirection.Up)
                y++;
            else if(direction == InputDirection.Down)
                y--;

            if(MakeAction(x, y))
            {
                _isMyTurn = false;
                
                _OnAction?.Invoke();
            }

            _subject?.Notify(ObserverEvent.FirstInput);
        }
    }
    public void TakeDamage(int amount)
    {
        _anim.SetTrigger("Hit");        
        LostFood(amount);
    }
    public void AddObserverOnFirstInput(IObserver observer) => _subject.AddObserver(observer);
    public void AddObserverOnFirstDeath(IObserver observer) => _subject.AddObserver(observer);
    public void RemoveObserverOnFirstInput(IObserver observer) => _subject.RemoveObserver(observer);
    public void RemoveObserverOnFirstDeath(IObserver observer) => _subject.RemoveObserver(observer);
    public void OnDisappear()
    {
        //This method is called when the die animations is end.
        gameObject.SetActive(false);
        _OnStarve?.Invoke();
        _subject.Notify(ObserverEvent.FirstDeath);
    }    
    public void SetTurn(bool isMyTurn)
    {
        if (isLive)
            _isMyTurn = isMyTurn;
    }
    
    private bool MakeAction(int x, int y)
    {
        bool makeAction = false;

        if(!_proceduralBoard.HasEnemy(x, y))
        {
            if(_proceduralBoard.IsWalkableTile(x, y))
            {
                _movement.Move(x, y);
                LostFood(_walkFood);
                makeAction = true;
            }
            else if(_proceduralBoard.IsBreakableTile(x, y))
            {
                _breaker.BreakTile(x, y);
                _anim.SetTrigger("Chop");
                LostFood(_brokeWallFood);
                makeAction = true;
            }
        }

        return makeAction;
    }
    private void ChangeFoodAmount()
    {
        _OnChangeFood?.Invoke(Food);        

        if (Food <= 0)
            Die();
    }
    private void Die()
    {
        isLive = false;
        _isMyTurn = false;
        GetComponent<AudioSource>().PlayOneShot(_dieClip);
        _anim.SetTrigger("Die");
    }
    private void TakeFood(int amount)
    {
        Food += amount;
        ChangeFoodAmount();
    }
    private void LostFood(int amount)
    {
        Food -= amount;
        ChangeFoodAmount();
    }
}

[Serializable]
public class LoseFoodEvent : UnityEvent<int> { }
