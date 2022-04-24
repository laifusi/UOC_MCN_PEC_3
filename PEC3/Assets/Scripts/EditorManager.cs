using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private Element elementToPlace;
    private Sprite spriteToPlace;

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

    public void SetGrid()
    {
        DestroyBackground();
        int x = int.Parse(inputFieldX.text);
        int y = int.Parse(inputFieldY.text);
        grid = new PlaceableDetection[x, y];
        elementsPlaced = new Element[x, y];
        for(int i = -x/2; i <= x/2; i++)
        {
            if (i == x / 2 && x % 2 == 0)
                break;
            for(int j = -y/2; j <= y/2; j++)
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
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
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

    public void PlaceElement(int x, int y)
    {
        int gridX = int.Parse(inputFieldX.text);
        int gridY = int.Parse(inputFieldY.text);
        if (x+gridX/2 >= 0 && y+gridY/2 >= 0 && x + gridX / 2 < gridX && y + gridY / 2 < gridY && grid[x + gridX / 2, y + gridY / 2].IsAvailable)
        {
            GameObject gameObject = Instantiate(placedObjectPrefab, new Vector3(x, y, 0), Quaternion.identity);
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteToPlace;
            grid[x + gridX / 2, y + gridY / 2].UseTile(true);
            elementsPlaced[x + gridX / 2, y + gridY / 2] = elementToPlace;
        }
    }
}

public enum Element
{
    Character, Star, Box, Wall
}