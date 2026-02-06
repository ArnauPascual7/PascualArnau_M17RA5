using System;
using System.IO;
using UnityEngine;

public class SaveSystem
{
    private static SaveData _saveData = new SaveData();

    [Serializable]
    public struct SaveData
    {
        public PlayerSaveData playerSaveData;
    }

    public static string GetFilePath()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".save";
        return saveFile;
    }

    public static void Save()
    {
        HandleSaveData();

        File.WriteAllText(GetFilePath(), JsonUtility.ToJson(_saveData, true));
    }

    private static void HandleSaveData()
    {
        GameManager.Instance.Save(ref _saveData.playerSaveData);
    }

    public static void Load()
    {
        string content = File.ReadAllText(GetFilePath());

        _saveData = JsonUtility.FromJson<SaveData>(content);

        HandleLoadData();
    }

    private static void HandleLoadData()
    {
        GameManager.Instance.Load(_saveData.playerSaveData);
    }
}
