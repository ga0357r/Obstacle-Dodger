using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ObstacleMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField]private Rigidbody rb;

    private void MoveTowardsPlayer()
    {
        float maxDistanceDelta = moveSpeed * Time.deltaTime; 
        Vector3 position = Vector3.MoveTowards(transform.position, SpawnManager.Instance.Player.position, maxDistanceDelta);
        transform.position = position;
    }

    private void FixedUpdate() 
    {
        MoveTowardsPlayer();
    }
}
