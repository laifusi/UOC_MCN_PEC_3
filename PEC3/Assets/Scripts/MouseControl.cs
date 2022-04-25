using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class MouseControl : MonoBehaviour
{
    [SerializeField] Camera mainCamera; //Camera

    private Vector3 mousePos; //Vector3 to save the position of the mouse
    private float zOffset; //z offset to the camera
    private SpriteRenderer spriteRenderer;

    public static MouseControl Instance; //Instance of the MouseControl

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        zOffset = -mainCamera.transform.position.z;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangePosition(CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
        transform.position = GetMousePosition();
        int posx = (int)transform.position.x;
        int posy = (int)transform.position.y;
        transform.position = new Vector3(posx, posy, transform.position.z);
    }

    public Vector3 GetMousePosition()
    {        
        mousePos.z += zOffset;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void Scroll(CallbackContext context)
    {
        var value = context.ReadValue<float>();
        if (value > 0)
            mainCamera.orthographicSize += 0.15f;
        else
            mainCamera.orthographicSize -= 0.15f;

        if (mainCamera.orthographicSize < 2)
            mainCamera.orthographicSize = 2;
        else if (mainCamera.orthographicSize > 50)
            mainCamera.orthographicSize = 50;
    }

    public void Place(CallbackContext context)
    {
        if (!context.started)
            return;

        int posx = (int)transform.position.x;
        int posy = (int)transform.position.y;
        EditorManager.Instance.PlaceElement(posx, posy);
    }

    public void EmptyElementToPlace(CallbackContext context)
    {
        if (!context.started)
            return;

        if(spriteRenderer.sprite == null)
        {
            int posx = (int)transform.position.x;
            int posy = (int)transform.position.y;
            EditorManager.Instance.DeleteElement(posx, posy);
        }
        else
        {
            spriteRenderer.sprite = null;
            EditorManager.Instance.DeselectElement();
        }
    }
}
