using System;
using UnityEngine;

public class Star : MonoBehaviour
{
    public static Action OnBoxIn;
    public static Action OnBoxOut;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Box"))
        {
            OnBoxIn?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Box"))
        {
            OnBoxOut?.Invoke();
        }
    }
}
