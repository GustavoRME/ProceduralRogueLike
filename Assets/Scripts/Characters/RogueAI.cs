using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Movement), typeof(Attacker), typeof(Animator))]
public class RogueAI : MonoBehaviour
{
    [Header("Movement Limits")]
    [SerializeField] private int _maxHorizontalMovement = 0;
    [SerializeField] private int _maxVerticalMovement = 0;

    [Space]
    [SerializeField] private int _viewDistance = 3;    
    
    private int _countHorizontalMovement;
    private int _countVerticalMovement;

    private ProceduralBoardGenerate _proceduralBoard = null;
    private Movement _movement;
    private Attacker _attacker;
    private Animator _anim;
    
    private void Start()
    {
        _movement = GetComponent<Movement>();
        _attacker = GetComponent<Attacker>();
        _anim = GetComponent<Animator>();

        _proceduralBoard = FindObjectOfType<ProceduralBoardGenerate>();
        
        _countHorizontalMovement = _maxHorizontalMovement;
        _countVerticalMovement = _maxVerticalMovement;        
    }
    
    public void MakeAction()
    {        
        _proceduralBoard.GetPlayerPosition(out int playerX, out int playerY);

        int x = (int)transform.position.x;
        int y = (int)transform.position.y;

        if(IsNeighbourPlayer(playerX, playerY, x, y))
        {
            _attacker.Attack();
            _anim.SetTrigger("Attack");
        }
        else if(IsSeeingPlayer(playerX, playerY, x, y))
        {            
            if(IsSameWidth(x, playerX))
            {                                
                if (playerY > y) //Player is up at me
                    Move(x, y + 1);
                else //Player is bottom at me
                    Move(x, y - 1);
            }
            else if(IsSameHeight(y, playerY))
            {
                if(playerX > x) //Player is right at me
                    Move(x + 1, y);
                else //Player is left at me 
                    Move(x - 1, y);
            }
        }
        else
        {
            int rand = Random.Range(0, 2);

            //0 -> Move horizontally
            //1 -> Move vertically

            bool canHorizontally = _countHorizontalMovement < _maxHorizontalMovement;
            bool canVertically = _countVerticalMovement < _maxVerticalMovement;

            if(rand == 0 && canHorizontally)
            {
                int xDist = Random.Range(0, 2) == 0 ? -1 : 1;
                Move(xDist + x, y);
            }
            else if(rand == 1 && canVertically)
            {
                int yDist = Random.Range(0, 2) == 0 ? -1 : 1;
                Move(x, yDist + y);
            }
            else
            {
                _countHorizontalMovement = 0;
                _countVerticalMovement = 0;
            }
        }
        
    }

    private bool IsNeighbourPlayer(int playerX, int playerY, int currentX, int currentY)
    {
        bool isNeighbour = false;        
        
        //Neighbour horizontally
        if(IsSameWidth(currentX, playerX))
        {
            int fromUp = currentY + 1;
            int fromDown = currentY - 1;

            if (playerY == fromUp || playerY == fromDown)
                isNeighbour = true;
        }
        //Neighbour vertically
        else if(IsSameHeight(currentY, playerY))
        {
            int fromRight = currentX + 1;
            int fromLeft = currentX - 1;

            if (playerX == fromRight || playerX == fromLeft)
                isNeighbour = true;
        }

        return isNeighbour;
    }
    private bool IsSeeingPlayer(int playerX, int playerY, int currentX, int currentY)
    {
        bool isSeeing = false;
        
        if(IsSameWidth(currentX, playerY))
            isSeeing = Mathf.Abs(currentY - playerY) <= _viewDistance;
        else if(IsSameHeight(currentY, playerY))
            isSeeing = Mathf.Abs(currentX - playerX) <= _viewDistance;

        return isSeeing;
    }
    private void Move(int x, int y)
    {
        if(_proceduralBoard.IsWalkableTile(x, y) && !_proceduralBoard.HasEnemy(x, y))
        {
            _movement.Move(x, y);
        }
    }
    private bool IsSameWidth(int x0, int x1) => x0 == x1;
    private bool IsSameHeight(int y0, int y1) => y0 == y1;
}
