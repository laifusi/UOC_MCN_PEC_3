using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private void Start()
    {
        CharacterMovement.OnMovingBox += MoveBox;
    }

    private void MoveBox(Vector2 direction)
    {
        transform.Translate(direction);
    }

    private void OnDestroy()
    {
        CharacterMovement.OnMovingBox -= MoveBox;
    }
}
