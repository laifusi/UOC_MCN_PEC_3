using System;
using System.Collections;
using UnityEngine;

public class Box : MonoBehaviour
{
    public static Action<Vector2> OnNoMovement;

    private bool wallUp, wallDown, wallLeft, wallRight;
    private bool boxUp, boxDown, boxLeft, boxRight;
    private bool characterUp, characterDown, characterLeft, characterRight;
    private bool wallSideToCheck, boxSideToCheck;

    private void Start()
    {
        CharacterMovement.OnMovingBox += MoveBox;
    }

    public IEnumerator SetBoxSide(Side side, bool isTouching)
    {
        if (isTouching)
            yield return new WaitForSeconds(0.01f);
        else
            yield return new WaitForSeconds(0f);

        switch (side)
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

    public IEnumerator SetWallSide(Side side, bool isTouching)
    {
        if (isTouching)
            yield return new WaitForSeconds(0.01f);
        else
            yield return new WaitForSeconds(0f);

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

    public IEnumerator SetCharacterSide(Side side, bool isTouching)
    {
        if (isTouching)
            yield return new WaitForSeconds(0.01f);
        else
            yield return new WaitForSeconds(0f);

        switch (side)
        {
            case Side.Right:
                characterRight = isTouching;
                break;
            case Side.Left:
                characterLeft = isTouching;
                break;
            case Side.Up:
                characterUp = isTouching;
                break;
            case Side.Down:
                characterDown = isTouching;
                break;
        }
    }

    private void MoveBox(Vector2 direction)
    {
        if(!CompareSideAndDirection(direction))
            return;

        SetSideToCheck(direction);
        if (!wallSideToCheck && !boxSideToCheck)
            transform.Translate(direction);
        else
            OnNoMovement?.Invoke(direction);
    }

    private bool CompareSideAndDirection(Vector2 direction)
    {
        return  characterRight && direction.x < 0 || 
                characterLeft && direction.x > 0 || 
                characterUp && direction.y < 0 || 
                characterDown && direction.y > 0;
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

    private void OnDestroy()
    {
        CharacterMovement.OnMovingBox -= MoveBox;
    }
}
