using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    [SerializeField]
    private float rotationsPerSecond; // rotation on X axis
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Vector3 baseRotation;

    /// <summary>
    /// Tank which shot the rocket, if null rocket is disabled
    /// </summary>
    public GameObject Shooter { get; set; } // tank which shot the rocket

    // Upgradeable values
    /// <summary>
    /// Rocket travel speed
    /// </summary>
    public float Speed { get; set; } = 7.0f;
    /// <summary>
    /// How fast the rocket rotates towards its TargetTank. Measured in degrees per second
    /// </summary>
    public float HeatSeekingRotation { get; set; } = 0.0f;
    /// <summary>
    /// Target Tank for the rocket to aim towards, null if heat seeking is disabled
    /// </summary>
    public GameObject TargetTank { get; set; }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Shooter == null)
            return;

        // Handling rotation
        var aimTowards = transform.position + transform.up * 3;
        if (TargetTank != null)
        {
            aimTowards = TargetTank.transform.position;
        }

        // https://discussions.unity.com/t/how-to-slow-down-transform-lookat/126080/2
        Vector3 direction = aimTowards - transform.position;
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, direction);
        var easedHeatRotation = Quaternion.Lerp(transform.rotation, toRotation, HeatSeekingRotation * Time.fixedDeltaTime).eulerAngles;
        var spinRotationX = 360 * rotationsPerSecond * Time.fixedDeltaTime * rb.rotation.eulerAngles.x;
        var finalRotation = Quaternion.Euler(spinRotationX, easedHeatRotation.y, easedHeatRotation.z);
        // Applying movement & rotation
        rb.Move(
            transform.position + transform.up * Speed,
            finalRotation
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tank"))
        {
            print("Hit a tank! KABOOM!");
            Destroy(other.transform.parent.gameObject); // destroying the player/enemy
            Destroy(gameObject);
        }
    }
}
