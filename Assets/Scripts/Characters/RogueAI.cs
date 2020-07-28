using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Movement), typeof(Attacker))]
public class RogueAI : MonoBehaviour
{
    [SerializeField] private ProceduralBoardGenerate _proceduralBoard = null;

    [Header("Movement Limits")]
    [SerializeField] private int _maxHorizontalMovement = 0;
    [SerializeField] private int _maxVerticalMovement = 0;

    [Space]
    [SerializeField] private int _viewDistance = 3;

    [SerializeField] private UnityEvent _OnAction = null;
    
    private int _countHorizontalMovement;
    private int _countVerticalMovement;

    private Movement _movement;
    private Attacker _attacker;
    private PlayerController _player;

    int _x;
    int _y;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _attacker = GetComponent<Attacker>();

        _player = FindObjectOfType<PlayerController>();
    }
    private void OnEnable()
    {
        _countHorizontalMovement = _maxHorizontalMovement;
        _countVerticalMovement = _maxVerticalMovement;

        _x = (int)transform.position.x;
        _y = (int)transform.position.y;
    }

    public void MakeAction()
    {        
        if(IsPlayerClose())
        {
            _attacker.Attack();
        }
        else if(IsSight())
        {

        }

        _OnAction?.Invoke();
    }

    private bool IsSight() => Distance(_x, _player.X) <= _viewDistance || Distance(_y, _player.Y) <= _viewDistance;
    private bool IsPlayerClose() => Distance(_x, _player.X) <= 1 || Distance(_y, _player.Y) <= 1;
    private int Distance(int a, int b) => Mathf.Abs(a - b);
}
