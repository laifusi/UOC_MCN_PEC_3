using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance; //Instance of the MenuManager
    public static SaveData DataToLoad;
    public static bool PreloadedLevelToEdit;
    public static bool TestingMode;

    private static string LevelsPath;

    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform levelsPanel;

    private void Awake()
    {
        #if UNITY_EDITOR
                LevelsPath = Application.dataPath + "/Resources/Levels/";
        #else
                LevelsPath = Application.dataPath + "/Levels/";
        
        #endif
        if (!Directory.Exists(LevelsPath))
        {
            Directory.CreateDirectory(LevelsPath);
        }
    }

    private void Start()
    {
        TextAsset[] premadeLevels = Resources.LoadAll<TextAsset>("Levels/");
        foreach(TextAsset level in premadeLevels)
        {
            File.WriteAllText(LevelsPath + level.name + ".txt", level.text);
        }
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveLevel(GridData[,] allGridData, string name, int width, int height)
    {
        SaveData allData = TransformMatrixToArray(allGridData);
        allData.width = width;
        allData.height = height;
        allData.levelName = name;
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

    public void FindLevels(string type)
    {
        foreach(Transform child in levelsPanel)
        {
            Destroy(child.gameObject);
        }

        DirectoryInfo directoryInfo = new DirectoryInfo(LevelsPath);
        FileInfo[] levelsSaved = directoryInfo.GetFiles();
        foreach(FileInfo level in levelsSaved)
        {
            Debug.Log(level.Name + " " + level.FullName);
            if(level.Name.Split(".txt")[1] == "")
            {
                Button levelButton = Instantiate(levelButtonPrefab, levelsPanel).GetComponent<Button>();
                TMP_Text levelButtonText = levelButton.GetComponentInChildren<TMP_Text>();
                levelButtonText.text = level.Name.Split(".txt"[0])[0];
                if (type == "Play")
                    levelButton.onClick.AddListener(() => LoadLevel(level.Name));
                else
                    levelButton.onClick.AddListener(() => EditLevel(level.Name));
            }
        }
    }

    public void LoadLevel(string levelName)
    {
        if(File.Exists(LevelsPath + levelName))
        {
            string savedLevel = File.ReadAllText(LevelsPath + levelName);
            DataToLoad = JsonUtility.FromJson<SaveData>(savedLevel);
            SceneManager.LoadScene("Game");
        }
    }

    public void LoadNextLevel(string levelName)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(LevelsPath);
        FileInfo[] filesFound = directoryInfo.GetFiles();
        List<FileInfo> levelsSaved = new List<FileInfo>();
        string levelToLoad = "";
        for (int i = 0; i < filesFound.Length; i++)
        {
            if (filesFound[i].Name.Split(".txt")[1] == "")
            {
                levelsSaved.Add(filesFound[i]);
            }
        }
        for (int i = 0; i < levelsSaved.Count; i++)
        {
            if (i < levelsSaved.Count - 1 && levelsSaved[i].Name.Split(".txt")[0] == levelName)
            {
                levelToLoad = levelsSaved[i + 1].Name;
            }
        }
        if (levelToLoad != "")
            LoadLevel(levelToLoad);
        else
            Menu();
    }

    private void EditLevel(string levelName)
    {
        if (File.Exists(LevelsPath + levelName))
        {
            string savedLevel = File.ReadAllText(LevelsPath + levelName);
            DataToLoad = JsonUtility.FromJson<SaveData>(savedLevel);
            PreloadedLevelToEdit = true;
            SceneManager.LoadScene("Editor");
        }
    }

    public void TestLevel(GridData[,] allGridData, int width, int height, string levelName)
    {
        DataToLoad = TransformMatrixToArray(allGridData);
        DataToLoad.width = width;
        DataToLoad.height = height;
        DataToLoad.levelName = levelName;
        TestingMode = true;
        SceneManager.LoadScene("TestGame");
    }

    public void Play()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(LevelsPath);
        FileInfo[] levelsSaved = directoryInfo.GetFiles();
        LoadLevel(levelsSaved[0].Name);
    }

    public void CreateLevel()
    {
        SceneManager.LoadScene("Editor");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Game");
    }

    public void ReloadTestLevel()
    {
        SceneManager.LoadScene("TestGame");
    }

    public void BackToEditMode()
    {
        PreloadedLevelToEdit = true;
        TestingMode = false;
        CreateLevel();
    }

    public void Exit()
    {
        Application.Quit();
    }
}

[Serializable]
public struct SaveData
{
    public GridData[] gridData;
    public int width;
    public int height;
    public string levelName;
}
