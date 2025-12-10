using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private SaveData saveData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // Check if current scene index is 0
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                LoadSaveData();
                SceneManager.LoadScene(saveData.currentScene);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadSaveData()
    {
        saveData = SaveSystem.LoadGame();
    }

    public void ReloadSaveData()
    {
        LoadSaveData();
    }

    public void SaveCurrentData()
    {
        if (saveData != null)
        {
            SaveSystem.SaveGame(saveData);
        }
    }

    public void LoadScene()
    {
        if (saveData != null)
        {
            SceneManager.LoadScene(saveData.currentScene);
        }
    }

    public SaveData GetSaveData()
    {
        return saveData;
    }

    public setupNextScene()
    {
        if (saveData != null)
        {
            saveData.currentScene += 1;
            SaveCurrentData();
        }
    }

    public void GotoNextScene()
    {
        if (saveData != null)
        {
            saveData.currentScene += 1;
            SaveCurrentData();
            SceneManager.LoadScene(saveData.currentScene);
        }
    }
}