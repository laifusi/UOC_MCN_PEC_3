using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EditorManager : MonoBehaviour
{
    [SerializeField] private Sprite characterSprite;
    [SerializeField] private Sprite starSprite;
    [SerializeField] private Sprite boxSprite;
    [SerializeField] private Sprite wallSprite;
    [SerializeField] private GameObject backgroundPrefab;
    [SerializeField] private GameObject placedObjectPrefab;
    [SerializeField] private TMP_InputField inputFieldX;
    [SerializeField] private TMP_InputField inputFieldY;

    private PlaceableDetection[,] grid;
    private Element[,] elementsPlaced;
    private GameObject[,] GOPlaced;
    private GridData[,] gridData;
    private Element elementToPlace;
    private Sprite spriteToPlace;
    private string levelName;

    public static EditorManager Instance; //Instance of the MouseControl

    private void Start()
    {
        SetGrid();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeLevelName(string value)
    {
        levelName = value;
    }

    public void SetGrid()
    {
        DestroyBackground();
        DestroyElements();
        int x = int.Parse(inputFieldX.text);
        int y = int.Parse(inputFieldY.text);
        grid = new PlaceableDetection[x, y];
        elementsPlaced = new Element[x, y];
        GOPlaced = new GameObject[x, y];
        for (int i = -x / 2; i <= x / 2; i++)
        {
            if (i == x / 2 && x % 2 == 0)
                break;
            for (int j = -y / 2; j <= y / 2; j++)
            {
                if (j == y / 2 && y % 2 == 0)
                    break;

                GameObject gridElement = Instantiate(backgroundPrefab, new Vector3(i, j, 0), Quaternion.identity, transform);

                int posx = (int)gridElement.transform.position.x;
                int posy = (int)gridElement.transform.position.y;
                gridElement.transform.position = new Vector3(posx, posy, 0);
                grid[i + x / 2, j + y / 2] = gridElement.GetComponent<PlaceableDetection>();
                gridElement.GetComponent<SpriteRenderer>().sortingOrder = 5;
            }
        }
    }

    private void DestroyBackground()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void DestroyElements()
    {
        if (GOPlaced != null)
        {
            foreach (GameObject go in GOPlaced)
            {
                Destroy(go);
            }
        }
    }

    public void SelectElement(string elementChosen)
    {
        switch(elementChosen)
        {
            case "Character":
                MouseControl.Instance.SetSprite(characterSprite);
                spriteToPlace = characterSprite;
                elementToPlace = Element.Character;
                break;
            case "Star":
                MouseControl.Instance.SetSprite(starSprite);
                spriteToPlace = starSprite;
                elementToPlace = Element.Star;
                break;
            case "Box":
                MouseControl.Instance.SetSprite(boxSprite);
                spriteToPlace = boxSprite;
                elementToPlace = Element.Box;
                break;
            case "Wall":
                MouseControl.Instance.SetSprite(wallSprite);
                spriteToPlace = wallSprite;
                elementToPlace = Element.Wall;
                break;
        }
    }

    public void DeselectElement()
    {
        spriteToPlace = null;
        elementToPlace = Element.Empty;
    }

    public void PlaceElement(int x, int y)
    {
        int gridX = int.Parse(inputFieldX.text);
        int gridY = int.Parse(inputFieldY.text);
        if (x+gridX/2 >= 0 && y+gridY/2 >= 0 && x + gridX / 2 < gridX && y + gridY / 2 < gridY && grid[x + gridX / 2, y + gridY / 2].IsAvailable && spriteToPlace != null)
        {
            GameObject gameObject = Instantiate(placedObjectPrefab, new Vector3(x, y, 0), Quaternion.identity);
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteToPlace;
            grid[x + gridX / 2, y + gridY / 2].UseTile(true);
            elementsPlaced[x + gridX / 2, y + gridY / 2] = elementToPlace;
            GOPlaced[x + gridX / 2, y + gridY / 2] = gameObject;
        }
    }

    public void DeleteElement(int x, int y)
    {
        int gridX = int.Parse(inputFieldX.text);
        int gridY = int.Parse(inputFieldY.text);
        if (x + gridX / 2 >= 0 && y + gridY / 2 >= 0 && x + gridX / 2 < gridX && y + gridY / 2 < gridY && !grid[x + gridX / 2, y + gridY / 2].IsAvailable)
        {
            grid[x + gridX / 2, y + gridY / 2].UseTile(false);
            elementsPlaced[x + gridX / 2, y + gridY / 2] = Element.Empty;
            Destroy(GOPlaced[x + gridX / 2, y + gridY / 2]);
        }
    }

    public void SaveGrid()
    {
        int gridX = int.Parse(inputFieldX.text);
        int gridY = int.Parse(inputFieldY.text);
        gridData = new GridData[gridX, gridY];
        for (int i = -gridX / 2; i <= gridX / 2; i++)
        {
            if (i == gridX / 2 && gridX % 2 == 0)
                break;
            for (int j = -gridY / 2; j <= gridY / 2; j++)
            {
                if (j == gridY / 2 && gridY % 2 == 0)
                    break;

                GridData elementAdded = new GridData();
                elementAdded.TypeOfElement = elementsPlaced[i + gridX / 2, j + gridY / 2];
                elementAdded.Position = new Vector3(i, j, 0);
                gridData[i + gridX / 2, j + gridY / 2] = elementAdded;
            }
        }

        if(levelName != null && levelName != string.Empty)
            MenuManager.Instance.SaveLevel(gridData, levelName);
    }

    public void BackToMenu()
    {
        MenuManager.Instance.Menu();
    }
}

[Serializable]
public enum Element
{
    Empty, Character, Star, Box, Wall
}

[Serializable]
public struct GridData
{
    public Element TypeOfElement;
    public Vector3 Position;
}