using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableDetection : MonoBehaviour
{
    private bool usedTile;

    public bool IsAvailable => !usedTile;

    public void UseTile(bool beingUsed)
    {
        usedTile = beingUsed;
    }
}
