using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance; //Instance of the MenuManager

    private static string LevelsPath;

    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform levelsPanel;

    private void Awake()
    {
        LevelsPath = Application.dataPath + "/Levels/";
        if (!Directory.Exists(LevelsPath))
        {
            Directory.CreateDirectory(LevelsPath);
        }
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveLevel(GridData[,] allGridData, string name)
    {
        SaveData allData = TransformMatrixToArray(allGridData);
        string json = JsonUtility.ToJson(allData);
        File.WriteAllText(LevelsPath + name + ".txt", json);
    }

    private SaveData TransformMatrixToArray(GridData[,] matrix)
    {
        SaveData array = new SaveData { gridData = new GridData[matrix.Length] };
        int id = 0;
        foreach(GridData data in matrix)
        {
            array.gridData[id] = data;
            id++;
        }
        return array;
    }

    public void FindLevels()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(LevelsPath);
        FileInfo[] levelsSaved = directoryInfo.GetFiles();
        foreach(FileInfo level in levelsSaved)
        {
            Button levelButton = Instantiate(levelButtonPrefab, levelsPanel).GetComponent<Button>();
            TMP_Text levelButtonText = levelButton.GetComponentInChildren<TMP_Text>();
            levelButtonText.text = level.Name.Split(".txt"[0])[0];
            levelButton.onClick.AddListener(() => LoadLevel(level.Name));
        }
    }

    public void LoadLevel(string levelName)
    {
        if(File.Exists(LevelsPath + levelName))
        {
            string savedLevel = File.ReadAllText(LevelsPath + levelName);
            SaveData data = JsonUtility.FromJson<SaveData>(savedLevel);
            Debug.Log(data.gridData[5].Position);
            Debug.Log(savedLevel);
        }
    }
}

[Serializable]
public struct SaveData
{
    public GridData[] gridData;
}
