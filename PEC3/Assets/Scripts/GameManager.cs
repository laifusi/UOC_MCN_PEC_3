using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int starsToFill;
    private int starsFilled;

    private void Start()
    {
        Star.OnBoxIn += BoxIn;
        Star.OnBoxOut += BoxOut;
    }

    private void BoxIn()
    {
        starsFilled++;

        if (starsFilled == starsToFill)
            WinLevel();
    }
    private void BoxOut()
    {
        starsFilled--;
    }

    private void WinLevel()
    {
        throw new NotImplementedException();
    }


    private void OnDestroy()
    {
        Star.OnBoxIn -= BoxIn;
        Star.OnBoxOut -= BoxOut;
    }
}
