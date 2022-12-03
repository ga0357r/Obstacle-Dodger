using System;
using UnityEngine;
using UnityEngine.Events;

public sealed class GameManager : MonoBehaviour
{
    [SerializeField] private GameState currentGameState;
    
    public static GameManager Instance { get; private set; }
    public GameState CurrentGameState => currentGameState;

    public UnityEvent OnCompleteCurrentLevel = null;
    public UnityEvent OnPlayerLoses = null;

    private void Awake() 
    {
        if(!Instance)
        {
            Instance = this;
        }

        Time.timeScale = 1f;
        PlayerData.OnScoreChanged += CheckPlayerProgress;
        PlayerData.OnLifeChanged += KeepTrackOfPlayerLife;

    }

    private void CheckPlayerProgress()
    {
        switch (currentGameState)
        {
            case GameState.LEVEL_1:
            {
                if(PlayerData.PlayerScore >= 100)
                {
                    Debug.Log("Player Wins Level 1");
                    OnCompleteCurrentLevel?.Invoke();
                    currentGameState = GameState.LEVEL_2;
                    SpawnManager.Instance.DeactivateAllRigidbodies();
                }

                
            }
            break;
            
            case GameState.LEVEL_2:
            {
                if(PlayerData.PlayerScore >= 200)
                {
                    Debug.Log("Player Wins Level 2");
                    OnCompleteCurrentLevel?.Invoke();
                    currentGameState = GameState.LEVEL_3;
                    SpawnManager.Instance.DeactivateAllRigidbodies();
                }
                
            }
            break;
            
            case GameState.LEVEL_3:
            {
                if(PlayerData.PlayerScore >= 300)
                {
                    Debug.Log("Player Wins Level 3");
                    OnCompleteCurrentLevel?.Invoke();
                    SpawnManager.Instance.DeactivateAllRigidbodies();
                }

            }
            break;
        }
    }

    private void KeepTrackOfPlayerLife()
    {
        if(PlayerData.PlayerLives == 0)
        {
            //player loses
            Debug.Log("Player Loses");
            OnPlayerLoses?.Invoke();
            SpawnManager.Instance.DeactivateAllRigidbodies();
        }
    }

    public void ResetPlayerLives()
    {
        PlayerData.SetDefaultLives();
    }

    public void ResetPlayerScore()
    {
        PlayerData.SetDefaultScore();
    }

    public void SaveData()
    {
        //called when quit button is pressed in main menu
        SaveManager.Save();
    }

    public void GetCurrentTime()
    {
        //set time of attempt
        PlayerData.GetTimeNow();
    }

    public void IncrementNoOfTries()
    {
        PlayerData.IncrementNoOfTries();
    }
    
    private void OnApplicationQuit() 
    {
        SaveData();
    }
}

public enum GameState
{
    NONE,
    LEVEL_1,
    LEVEL_2,
    LEVEL_3,
}