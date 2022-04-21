using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public static Action<Vector2> OnMovingBox;

    private bool wallUp, wallDown, wallLeft, wallRight;
    private bool boxUp, boxDown, boxLeft, boxRight;

    public void OnMovement(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            var direction = context.ReadValue<Vector2>();
            transform.position += new Vector3(direction.x, direction.y, 0);
        }
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
}
