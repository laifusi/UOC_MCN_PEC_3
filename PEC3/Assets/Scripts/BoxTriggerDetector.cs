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
            boxController.SetWallSide(side, true);
        }
        else if (collision.CompareTag("Box"))
        {
            boxController.SetBoxSide(side, true);
        }
        else if(collision.CompareTag("Player"))
        {
            boxController.SetCharacterSide(side, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            boxController.SetWallSide(side, false);
        }
        else if (collision.CompareTag("Box"))
        {
            boxController.SetBoxSide(side, false);
        }
        else if (collision.CompareTag("Player"))
        {
            boxController.SetCharacterSide(side, false);
        }
    }
}
