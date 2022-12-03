using System.IO;
using UnityEngine;

public static class SaveManager 
{
    private static string saveFileName = "PlayerSaveData.save";

    public static void Save()
    {
        PlayerSaveData playerSaveData = new PlayerSaveData
        {
            playerLives = PlayerData.PlayerLives,
            playerScore = PlayerData.PlayerScore,
            timeOfAttempt = PlayerData.TimeOfAttempt,
            pushedObjectsAmt = PlayerData.PushedObjectsAmt,
            noOfTries = PlayerData.NoOfTries,

        };

        string jsonSaveString = JsonUtility.ToJson(playerSaveData);
        File.WriteAllText(Application.dataPath + "/" + saveFileName, jsonSaveString);
        Debug.Log("Saved Successfully\n" + jsonSaveString);
    }

    public static void Load()
    {
        if(File.Exists(Application.dataPath+ "/" + saveFileName))
        {
            string jsonSaveString = File.ReadAllText(Application.dataPath + "/" + saveFileName);
            PlayerSaveData playerSaveData = JsonUtility.FromJson<PlayerSaveData>(jsonSaveString);
            PlayerData.LoadPlayerData(playerSaveData);
        }
    }
}

public class PlayerSaveData
{
    public int playerLives; 
    public int playerScore;
    public string timeOfAttempt;
    public int pushedObjectsAmt;
    public int noOfTries;
}