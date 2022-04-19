using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public static Action<Vector2> OnMovingBox;

    [SerializeField] private InputActionAsset playerInputAction;

    private bool touchingBox;
    private bool touchUp, touchDown, touchLeft, touchRight;

    private void Start()
    {
    }
}
