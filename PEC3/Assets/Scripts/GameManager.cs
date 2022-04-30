using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int starsToFill;
    private int starsFilled;

    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject backgroundPrefab;

    private void Awake()
    {
        Star.OnStar += CountStar;
    }

    private void Start()
    {
        Star.OnBoxIn += BoxIn;
        Star.OnBoxOut += BoxOut;

        LoadLevel();
    }

    private void LoadLevel()
    {
        SaveData data = MenuManager.DataToLoad;
        foreach(GridData grid in data.gridData)
        {
            switch(grid.TypeOfElement)
            {
                case Element.Character:
                    Instantiate(characterPrefab, grid.Position, Quaternion.identity);
                    Instantiate(backgroundPrefab, grid.Position, Quaternion.identity);
                    break;
                case Element.Star:
                    Instantiate(starPrefab, grid.Position, Quaternion.identity);
                    Instantiate(backgroundPrefab, grid.Position, Quaternion.identity);
                    break;
                case Element.Box:
                    Instantiate(boxPrefab, grid.Position, Quaternion.identity);
                    Instantiate(backgroundPrefab, grid.Position, Quaternion.identity);
                    break;
                case Element.Wall:
                    Instantiate(wallPrefab, grid.Position, Quaternion.identity);
                    Instantiate(backgroundPrefab, grid.Position, Quaternion.identity);
                    break;
                case Element.Empty:
                    Instantiate(backgroundPrefab, grid.Position, Quaternion.identity);
                    break;
            }
        }

        int width = data.width;
        int height = data.height;
        for (int i = 0; i < width + 2; i++)
        {
            for (int j = 0; j < height + 2; j++)
            {
                if (i == 0 || i == width + 1 || j == 0 || j == height + 1)
                {
                    Instantiate(wallPrefab, new Vector3(i -  width / 2 - 1, j - height / 2 - 1, 0), Quaternion.identity, transform);
                }
            }
        }
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
