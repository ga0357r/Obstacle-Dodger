using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake() 
    {
        if(!Instance)
        {
            Instance = this;
        }    
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();

        #if(UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
