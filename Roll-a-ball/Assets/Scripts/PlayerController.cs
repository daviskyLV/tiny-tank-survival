using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    [SerializeField]
    private float jumpPower = 300.0f;
    [SerializeField]
    public float speed = 6.0f;

    private double lastJump = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement*speed);
    }

    void OnMove(InputValue movementValue)
    {
        Debug.Log("OnMove triggered!");
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnFire(InputValue fireValue)
    {
        Debug.Log("OnFire triggered!");
    }

    void OnJump(InputValue jumpValue)
    {
        Debug.Log("OnJump triggered!");
        Debug.Log("Velocity: " + Mathf.Abs(rb.velocity.y));
        if (Time.realtimeSinceStartupAsDouble - lastJump >= 1)
        {
            rb.AddForce(new Vector3(0, jumpPower, 0));
            lastJump = Time.realtimeSinceStartupAsDouble;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            rb.AddExplosionForce(jumpPower*2, other.transform.position, 7.27f);
            other.gameObject.SetActive(false);
        }
    }
}
