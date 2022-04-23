using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public static Action<Vector2> OnMovingBox;

    private bool wallUp, wallDown, wallLeft, wallRight;
    private bool boxUp, boxDown, boxLeft, boxRight;
    private bool wallSideToCheck, boxSideToCheck;

    private void Start()
    {
        Box.OnNoMovement += UndoMovement;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            var direction = context.ReadValue<Vector2>();
            SetSideToCheck(direction);

            if (!wallSideToCheck)
            {
                Move(direction);
                if (boxSideToCheck)
                {
                    OnMovingBox?.Invoke(direction);
                }
            }
        }
    }

    private void SetSideToCheck(Vector2 direction)
    {
        if (direction.x > 0)
        {
            wallSideToCheck = wallRight;
            boxSideToCheck = boxRight;
        }
        else if (direction.x < 0)
        {
            wallSideToCheck = wallLeft;
            boxSideToCheck = boxLeft;
        }
        else if (direction.y > 0)
        {
            wallSideToCheck = wallUp;
            boxSideToCheck = boxUp;
        }
        else if (direction.y < 0)
        {
            wallSideToCheck = wallDown;
            boxSideToCheck = boxDown;
        }
    }

    private void Move(Vector2 direction)
    {
        transform.position += new Vector3(direction.x, direction.y, 0);
    }

    public void SetBoxSide(Side side, bool isTouching)
    {
        switch(side)
        {
            case Side.Right:
                boxRight = isTouching;
                break;
            case Side.Left:
                boxLeft = isTouching;
                break;
            case Side.Up:
                boxUp = isTouching;
                break;
            case Side.Down:
                boxDown = isTouching;
                break;
        }
    }

    public void SetWallSide(Side side, bool isTouching)
    {
        switch (side)
        {
            case Side.Right:
                wallRight = isTouching;
                break;
            case Side.Left:
                wallLeft = isTouching;
                break;
            case Side.Up:
                wallUp = isTouching;
                break;
            case Side.Down:
                wallDown = isTouching;
                break;
        }
    }
    
    private void UndoMovement(Vector2 directionTaken)
    {
        Move(directionTaken * -1);
    }

    private void OnDestroy()
    {
        Box.OnNoMovement -= UndoMovement;
    }
}
