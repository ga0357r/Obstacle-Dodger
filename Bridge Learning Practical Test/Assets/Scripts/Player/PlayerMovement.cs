using UnityEngine;

public sealed class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float moveSpeed = 3f;
    [SerializeField]private bool usePhysics = false;
    [SerializeField]private Rigidbody rb;


    private void Awake() 
    {
        if(usePhysics)
        {
            moveSpeed = 6f;
            return;
        }

        moveSpeed = 3f;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float xVal = Input.GetAxis("Horizontal") * moveSpeed;
        float zVal = Input.GetAxis("Vertical")  * moveSpeed;

        if(!usePhysics)
        {
            Vector3 currentPos = transform.position;
            xVal *= Time.deltaTime;
            zVal *= Time.deltaTime;
            transform.position = new Vector3(currentPos.x + xVal, currentPos.y, currentPos.z + zVal);
            return;
        }

        rb.AddForce(xVal, 0f, zVal, ForceMode.Force);
    }
}
