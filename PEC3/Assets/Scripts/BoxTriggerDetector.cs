using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTriggerDetector : MonoBehaviour
{
    [SerializeField] Box boxController;
    [SerializeField] Side side;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            StartCoroutine(boxController.SetWallSide(side, true));
        }
        else if (collision.CompareTag("Box"))
        {
            StartCoroutine(boxController.SetBoxSide(side, true));
        }
        else if(collision.CompareTag("Player"))
        {
            StartCoroutine(boxController.SetCharacterSide(side, true));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            StartCoroutine(boxController.SetWallSide(side, false));
        }
        else if (collision.CompareTag("Box"))
        {
            StartCoroutine(boxController.SetBoxSide(side, false));
        }
        else if (collision.CompareTag("Player"))
        {
            StartCoroutine(boxController.SetCharacterSide(side, false));
        }
    }
}
