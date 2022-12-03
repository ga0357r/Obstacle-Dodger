using System;

public static class PlayerData 
{
    public static int PlayerLives{get;  private set;} = 3;
    public static int PlayerScore { get;  private set; } = 0;
    public static string TimeOfAttempt { get; private set; }
    public static int PushedObjectsAmt { get; private set; }
    public static int NoOfTries { get; private set; }
    public static event Action OnScoreChanged = null;
    public static event Action OnLifeChanged = null;

    public static void IncreaseScore(int addedPoints)
    {
        PlayerScore += addedPoints;
        OnScoreChanged?.Invoke();
    }

    public static void SetDefaultScore()
    {
        PlayerScore = 0;
        OnScoreChanged?.Invoke();
    }

    public static void DecreaseLife(int byAmount)
    {
        PlayerLives -= byAmount;
        OnLifeChanged?.Invoke();
    }

    public static void SetDefaultLives()
    {
        PlayerLives = 3;
        OnLifeChanged?.Invoke();
    }

    public static void LoadPlayerData(PlayerSaveData playerSaveData)
    {
        PlayerLives = playerSaveData.playerLives;
        PlayerScore = playerSaveData.playerScore;
        TimeOfAttempt = playerSaveData.timeOfAttempt;
        PushedObjectsAmt = playerSaveData.pushedObjectsAmt;
        NoOfTries = playerSaveData.noOfTries;
    }

    public static void IncrementPushedObjAmt()
    {
        PushedObjectsAmt += 1;
    }

    public static void GetTimeNow()
    {
        TimeOfAttempt = DateTime.Now.ToString();
    }

    public static void IncrementNoOfTries()
    {
        NoOfTries += 1;
    }
}