using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameSaver : MonoBehaviour
{
    public static GameSaver Instance { get; private set; }
    private string savePath;
    public int lastPlayedChapter = -1;

    private void Awake()
    {   
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            savePath = Path.Combine(Application.persistentDataPath, "savedata.bin");

            LoadGame();
        }
    }

    public void SaveGame(int value)
    {
        try
        {
            lastPlayedChapter = value;
            using (BinaryWriter writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
            {
                writer.Write(value);
            }
            Debug.Log($"Game saved successfully at {savePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }

    public int LoadGame()
    {
        try
        {
            if (File.Exists(savePath))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
                {
                    int value = reader.ReadInt32();
                    lastPlayedChapter = value;
                    Debug.Log($"Loaded value: {value}");
                    return value;
                }
            }
            Debug.LogWarning("No save file found");
            return lastPlayedChapter;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
            return lastPlayedChapter;
        }
    }

    public bool HasSaveGame()
    {
        return File.Exists(savePath);
    }
}
