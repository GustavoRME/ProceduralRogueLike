using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Amount food the player start with it")]
    [SerializeField] private int _startFood = 100;

    [Tooltip("Value for each action player do")]
    [SerializeField] private int _eatFood = 1;

    [Space]
    [SerializeField] private ProceduralBoardGenerate _proceduralBoard = null;

    [Space]

    [SerializeField] private UnityEvent _OnAction = null;    
    [SerializeField] private LoseFoodEvent _OnChangeFood = null;
    [SerializeField] private UnityEvent _OnStarve = null;
    [SerializeField] private UnityEvent _OnExit = null;

    private Movement _movement;
    private Breaker _breaker;

    private int _food;

    public int X { get; private set; }
    public int Y { get; private set; }

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _breaker = GetComponent<Breaker>();

        _food = _startFood;

        X = (int)transform.position.x;
        Y = (int)transform.position.y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Soda") || collision.CompareTag("Food"))
        {
            var food = collision.GetComponent<Food>();

            EatFood(food.Eat());
        }
        else if(collision.CompareTag("Exit"))
        {
            _OnExit?.Invoke();
        }
    }

    public void InputHandle(InputDirection direction)
    {
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

            Action(x, y);
            _OnAction?.Invoke();

            X = (int)transform.position.x;
            Y = (int)transform.position.y;
        }
    }
    public void LoseFood(int amount)
    {
        _food -= amount;
        ChangeFoodAmount();
    }
        
    private void Action(int x, int y)
    {
        if(_proceduralBoard.IsWalkableTile(x, y))
        {
            _movement.Move(x, y);
            LoseFood(_eatFood);
        }
        else if(_proceduralBoard.IsBreakableTile(x, y))
        {
            _breaker.BreakTile(x, y);
            LoseFood(_eatFood);
        }
    }
    private void ChangeFoodAmount()
    {
        _OnChangeFood?.Invoke(_food);

        Debug.Log("Food " + _food);

        if (_food <= 0)
            _OnStarve?.Invoke();
    }
    private void EatFood(int amount)
    {
        _food += amount;
        ChangeFoodAmount();
    }

}

[Serializable]
public class LoseFoodEvent : UnityEvent<int> { }
