using UnityEngine;

public class CharacterTriggerDetector : MonoBehaviour
{
    [SerializeField] CharacterMovement characterMovement;
    [SerializeField] Side side;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall"))
        {
            StartCoroutine(characterMovement.SetWallSide(side, true));
        }
        else if (collision.CompareTag("Box"))
        {
            StartCoroutine(characterMovement.SetBoxSide(side, true));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            StartCoroutine(characterMovement.SetWallSide(side, false));
        }
        else if (collision.CompareTag("Box"))
        {
            StartCoroutine(characterMovement.SetBoxSide(side, false));
        }
    }
}

public enum Side
{
    Right, Left, Up, Down
}
