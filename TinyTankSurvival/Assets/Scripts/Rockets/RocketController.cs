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
    public GameObject Shooter { get; set; }

    // Upgradeable values
    /// <summary>
    /// Rocket travel speed
    /// </summary>
    public float Speed { get; set; } = 0.0f;
    /// <summary>
    /// How fast the rocket rotates towards its TargetTank. Measured in degrees per second
    /// </summary>
    public float HeatSeekingRotation { get; set; } = 0.0f;
    /// <summary>
    /// Target Tank for the rocket to aim towards, null if heat seeking is disabled
    /// </summary>
    public GameObject TargetTank { get; set; }
    /// <summary>
    /// The lifetime of the rocket
    /// </summary>
    public double Lifetime { get; set; } = 10.0;

    private double startTime = 0;

    private void Start()
    {
        startTime = Time.timeAsDouble;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Shooter == null)
            return;

        CheckLifetime();
        MoveRocket();
    }

    private void CheckLifetime()
    {
        if (Time.timeAsDouble - startTime > Lifetime)
            Destroy(gameObject);
    }

    private void MoveRocket()
    {
        // Handling rotation
        var aimTowards = transform.position + transform.forward * 3;
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
            transform.position + Speed * Time.fixedDeltaTime * transform.up,
            finalRotation
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tank") && !other.gameObject.Equals(Shooter))
        {
            print("Hit a tank! KABOOM!");
            print(other.transform);
            print(other.transform.parent);
            print(other.transform.parent.gameObject);
            Destroy(other.transform.parent.gameObject); // destroying the player/enemy
        }

        Destroy(gameObject);
    }
}
