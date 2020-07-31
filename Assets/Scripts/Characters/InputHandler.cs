using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Movement), typeof(Breaker))]
public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera = null;
    [SerializeField] private InputEvent _OnInput = null;

    Vector2 _inputPos;
    bool _isPressed;

    private void Update()
    {
        _inputPos = transform.position;
        _isPressed = false;

#if UNITY_STANDALONE || UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
        {
            _inputPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            _isPressed = true;
        }
#elif UNITY_ANDROID
        if(Input.touchCount > 0)
        {
            inputPos = _camera.ScreenToWorldPoint(Input.GetTouch(0).position);
            isPressed = true;
        }
#endif

        if (_isPressed)
        {            
            InputDirection direction = InputDirection.None;

            if (IsRight(transform.position, _inputPos)) 
            { 
                direction = InputDirection.Right;
            }                         
            else if(IsUpper(transform.position, _inputPos))
            {
                direction = InputDirection.Up;
            }
            else if(IsLeft(transform.position, _inputPos))
            {
                direction = InputDirection.Left;
            }
            else if(IsBottom(transform.position, _inputPos))
            {
                direction = InputDirection.Down;
            }

            _OnInput?.Invoke(direction);
        }        
    }
        
    private bool IsRight(Vector2 pawn, Vector2 input) => IsMinimunHorizontal(pawn, input) && input.x > pawn.x;
    private bool IsLeft(Vector2 pawn, Vector2 input) =>  IsMinimunHorizontal(pawn, input) && input.x < pawn.x;
    private bool IsUpper(Vector2 pawn, Vector2 input) => IsMinimunVertical(pawn, input) && input.y > pawn.y;
    private bool IsBottom(Vector2 pawn, Vector2 input) => IsMinimunVertical(pawn, input) && input.y < pawn.y;
    private bool IsMinimunHorizontal(Vector2 pawn, Vector2 input)
    {
        float diffX = Mathf.Abs(pawn.x - input.x);
        float diffY = Mathf.Abs(pawn.y - input.y);

        return diffX >= diffY;
    } 
    private bool IsMinimunVertical(Vector2 pawn, Vector2 input)
    {
        float diffX = Mathf.Abs(pawn.x - input.x);
        float diffY = Mathf.Abs(pawn.y - input.y);

        return diffY >= diffX;
    }
}

[Serializable]
public class InputEvent : UnityEvent<InputDirection> { }

public enum InputDirection
{
    None,
    Left,
    Right,
    Up,
    Down
}
