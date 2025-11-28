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

    public void ChangeWYRRoomCompletionStatus(bool status)
    {
        if (saveData != null)
        {
            saveData.hasCompletedWYrRoom = status;
            SaveSystem.SaveGame(saveData);
        }
    }

    public void ChangeRoom1CompletionStatus(bool status)
    {
        if (saveData != null)
        {
            saveData.hasCompletedRoom1 = status;
            SaveSystem.SaveGame(saveData);
        }
    }

    public void ChangeRoom2CompletionStatus(bool status)
    {
        if (saveData != null)
        {
            saveData.hasCompletedRoom2 = status;
            SaveSystem.SaveGame(saveData);
        }
    }

    public void ChangeRoom3CompletionStatus(bool status)
    {
        if (saveData != null)
        {
            saveData.hasCompletedRoom3 = status;
            SaveSystem.SaveGame(saveData);
        }
    }

    public void ChangeHadBeatenGameStatus(bool status)
    {
        if (saveData != null)
        {
            saveData.hadBeatenGame = status;
            SaveSystem.SaveGame(saveData);
        }
    }
    public SaveData GetSaveData()
    {
        return saveData;
    }
}