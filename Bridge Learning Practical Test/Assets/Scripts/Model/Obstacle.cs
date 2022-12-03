using System;
using UnityEngine;

public sealed class Obstacle : Model
{
    protected override void Init()
    {
        OnPlayerCollided.AddListener(PlayerData.IncrementPushedObjAmt);        
    }

    private void OnEnable() 
    {
        BeginCountdownToDeactivation();
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //Update Scores
            OnPlayerCollided?.Invoke();
        }
    }
}