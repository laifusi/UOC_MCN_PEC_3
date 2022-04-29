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
    [SerializeField] private TMP_InputField levelNameField;

    private PlaceableDetection[,] grid;
    private Element[,] elementsPlaced;
    private Element[,] externalWalls;
    private GameObject[,] GOPlaced;
    private GridData[,] gridData;
    private Element elementToPlace;
    private Sprite spriteToPlace;
    private string levelName;
    private int gridX;
    private int gridY;

    public static EditorManager Instance; //Instance of the MouseControl

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if(MenuManager.PreloadedLevelToEdit)
        {
            PreloadLevel();
        }
        else
        {
            SetGrid();
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
        gridX = int.Parse(inputFieldX.text);
        gridY = int.Parse(inputFieldY.text);
        grid = new PlaceableDetection[gridX, gridY];
        elementsPlaced = new Element[gridX, gridY];
        externalWalls = new Element[gridX + 2, gridY + 2];
        GOPlaced = new GameObject[gridX, gridY];
        for (int i = -gridX / 2; i <= gridX / 2; i++)
        {
            if (i == gridX / 2 && gridX % 2 == 0)
                break;
            for (int j = -gridY / 2; j <= gridY / 2; j++)
            {
                if (j == gridY / 2 && gridY % 2 == 0)
                    break;
                
                GameObject gridElement = Instantiate(backgroundPrefab, new Vector3(i, j, 0), Quaternion.identity, transform);

                int posx = (int)gridElement.transform.position.x;
                int posy = (int)gridElement.transform.position.y;
                gridElement.transform.position = new Vector3(posx, posy, 0);
                grid[i + gridX / 2, j + gridY / 2] = gridElement.GetComponent<PlaceableDetection>();
                gridElement.GetComponent<SpriteRenderer>().sortingOrder = 5;
            }
        }

        for (int i = 0; i < gridX + 2; i++)
        {
            for (int j = 0; j < gridY + 2; j++)
            {
                if (i == 0 || i == gridX + 1 || j == 0 || j == gridY + 1)
                {
                    spriteToPlace = wallSprite;
                    elementToPlace = Element.Wall;
                    PlaceExternalWall(i, j);
                }
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

    public void PreloadLevel()
    {
        SaveData data = MenuManager.DataToLoad;
        levelNameField.text = data.levelName;
        inputFieldX.text = data.width.ToString();
        inputFieldY.text = data.height.ToString();
        inputFieldX.onValueChanged.Invoke(inputFieldX.text); // We force the call of the onValueChanged event in case the saved grid size is the same as the default values

        foreach (GridData grid in data.gridData)
        {
            switch (grid.TypeOfElement)
            {
                case Element.Character:
                    spriteToPlace = characterSprite;
                    elementToPlace = Element.Character;
                    break;
                case Element.Star:
                    spriteToPlace = starSprite;
                    elementToPlace = Element.Star;
                    break;
                case Element.Box:
                    spriteToPlace = boxSprite;
                    elementToPlace = Element.Box;
                    break;
                case Element.Wall:
                    spriteToPlace = wallSprite;
                    elementToPlace = Element.Wall;
                    break;
            }
            
            if(grid.TypeOfElement != Element.Empty)
            {
                PlaceElement((int)grid.Position.x, (int)grid.Position.y);
            }
        }

        spriteToPlace = null;
        elementToPlace = Element.Empty;
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
        if (x+gridX/2 >= 0 && y+gridY/2 >= 0 && x + gridX / 2 < gridX && y + gridY / 2 < gridY && grid[x + gridX / 2, y + gridY / 2].IsAvailable && spriteToPlace != null)
        {
            GameObject gameObject = Instantiate(placedObjectPrefab, new Vector3(x, y, 0), Quaternion.identity);
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteToPlace;
            grid[x + gridX / 2, y + gridY / 2].UseTile(true);
            elementsPlaced[x + gridX / 2, y + gridY / 2] = elementToPlace;
            GOPlaced[x + gridX / 2, y + gridY / 2] = gameObject;
        }
    }

    public void PlaceExternalWall(int x, int y)
    {
        GameObject gameObject = Instantiate(placedObjectPrefab, new Vector3(x - gridX/2 - 1, y - gridY/2 - 1, 0), Quaternion.identity);
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteToPlace;
        Debug.Log(y + 1 + gridY/2);
        Debug.Log(x + 1 + gridX / 2);
        externalWalls[x, y] = elementToPlace;
    }

    public void DeleteElement(int x, int y)
    {
        if (x + gridX / 2 >= 0 && y + gridY / 2 >= 0 && x + gridX / 2 < gridX && y + gridY / 2 < gridY && !grid[x + gridX / 2, y + gridY / 2].IsAvailable)
        {
            grid[x + gridX / 2, y + gridY / 2].UseTile(false);
            elementsPlaced[x + gridX / 2, y + gridY / 2] = Element.Empty;
            Destroy(GOPlaced[x + gridX / 2, y + gridY / 2]);
        }
    }

    public void SaveLevel()
    {
        SaveGrid();

        if (levelName != null && levelName != string.Empty)
            MenuManager.Instance.SaveLevel(gridData, levelName, gridX, gridY);
    }

    public void SaveGrid()
    {
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
    }

    public void TestLevel()
    {
        SaveGrid();
        MenuManager.Instance.TestLevel(gridData, gridX, gridY, levelName);
    }

    public void BackToMenu()
    {
        MenuManager.Instance.Menu();
        MenuManager.PreloadedLevelToEdit = false;
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