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
    [SerializeField] private TMP_InputField inputFieldX;
    [SerializeField] private TMP_InputField inputFieldY;

    private int[,] grid;

    private void Start()
    {
        SetGrid();
    }

    public void SetGrid()
    {
        DestroyBackground();
        int x = int.Parse(inputFieldX.text);
        int y = int.Parse(inputFieldY.text);
        grid = new int[x, y];
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
                break;
            case "Star":
                MouseControl.Instance.SetSprite(starSprite);
                break;
            case "Box":
                MouseControl.Instance.SetSprite(boxSprite);
                break;
            case "Wall":
                MouseControl.Instance.SetSprite(wallSprite);
                break;
        }
    }
}