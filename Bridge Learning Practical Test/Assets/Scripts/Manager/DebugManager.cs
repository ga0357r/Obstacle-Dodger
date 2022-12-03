using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DebugManager : MonoBehaviour
{
    [SerializeField] private bool IsLogEnabled = false;
    
    private void Awake() 
    {
        ToggleLogging();
    }

    private void ToggleLogging()
    {
        Debug.unityLogger.logEnabled = IsLogEnabled;
        Debug.Log($"Debugging is {Debug.unityLogger.logEnabled}");
    }
}
