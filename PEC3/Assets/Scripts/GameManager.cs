using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int starsToFill;
    private int starsFilled;

    private void Awake()
    {
        Star.OnStar += CountStar;
    }

    private void Start()
    {
        Star.OnBoxIn += BoxIn;
        Star.OnBoxOut += BoxOut;
    }

    private void CountStar()
    {
        starsToFill++;
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
        Debug.Log("Level won!");
    }


    private void OnDestroy()
    {
        Star.OnBoxIn -= BoxIn;
        Star.OnBoxOut -= BoxOut;
        Star.OnStar -= CountStar;
    }
}
