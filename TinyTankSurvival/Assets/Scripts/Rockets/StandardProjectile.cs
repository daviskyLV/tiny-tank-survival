using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardProjectile : Projectile
{
    private bool setup = false;

    // Upgradeable values
    /// <summary>
    /// Projectile travel speed
    /// </summary>
    private float projectileSpeed = 3.0f;
    /// <summary>
    /// How fast the rocket rotates towards its TargetTank. Measured in degrees per second
    /// </summary>
    private float heatSeekingRotation = 72.7f;
    /// <summary>
    /// Target Tank for the rocket to aim towards, null if heat seeking is disabled
    /// </summary>
    private GameObject targetTank;

    // Start is called before the first frame update
    private void Start()
    {
        startTime = Time.timeAsDouble;
    }

    /// <summary>
    /// Sets up the projectile, can be only called once
    /// </summary>
    /// <param name="shooter">Tank which shot the projectile, if null projectile is destroyed</param>
    /// <param name="speed">The speed of the projectile</param>
    /// <param name="heatSeekingRotation">How fast can the projectile adjust its trajectory, degrees/s</param>
    /// <param name="target">Target for heat seeking, null if disabled</param>
    public void Setup(GameObject shooter, float speed, float heatSeekingRotation, GameObject target = null)
    {
        if (setup)
            return;

        setup = true;

        this.shooter = shooter;
        this.projectileSpeed = speed;
        this.heatSeekingRotation = heatSeekingRotation;
        this.targetTank = target;
    }

    private void FixedUpdate()
    {
        CheckLifetime();

        if (shooter)
            MoveProjectile();
    }

    // Calculating and moving the projectile using a rigidbody
    private void MoveProjectile()
    {
        // handling rotation
        var aimTowards = transform.position + transform.forward * 3;
        if (targetTank != null)
        {
            aimTowards = targetTank.transform.position;
        }

        // https://www.youtube.com/watch?v=0v_H3oOR0aU
        // Comment by @maxokaan that mentions how to do it for 3D
        Vector3 direction = aimTowards - transform.position;
        direction.Normalize();

        // Heat seeking rotation
        Vector3 amountToRotate = Vector3.Cross(direction, transform.forward) * Vector3.Angle(transform.forward, direction);
        var easedHeatRotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(amountToRotate),
            heatSeekingRotation * Time.fixedDeltaTime
        ).eulerAngles;

        // Applying spin and calculating final rotation
        var spinRotation = 360 * rotationsPerSecondZ * Time.fixedDeltaTime;
        var finalRotation = Quaternion.Euler(easedHeatRotation + new Vector3(0, 0, spinRotation));

        // Applying movement & rotation
        rb.Move(
            transform.position + projectileSpeed * Time.fixedDeltaTime * transform.forward,
            finalRotation
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tank") && !other.gameObject.Equals(null))
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
