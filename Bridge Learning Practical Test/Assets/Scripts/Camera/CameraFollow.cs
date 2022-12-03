using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    private void LateUpdate() 
    {
        transform.position =  offset + SpawnManager.Instance.Player.position;   
    }
}
