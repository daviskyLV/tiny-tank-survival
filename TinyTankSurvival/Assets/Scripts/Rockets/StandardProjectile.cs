using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

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
    private float heatSeekingRotation = 17.27f;
    /// <summary>
    /// Target Tank for the rocket to aim towards, null if heat seeking is disabled
    /// </summary>
    private GameObject targetTank;
    /// <summary>
    /// Used to override aiming rotation when moving the projectile
    /// </summary>
    private Quaternion? overridenMovementRotation = null;

    // Last recorded position, hack to fix teleporting
    private Vector3 lastPosition = Vector3.zero;
    // Last recorded rotation, hack to fix direction
    private Quaternion lastRotation = Quaternion.Euler(0,0,0);

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
    /// <param name="maxBounces">How many times can the bullet bounce against the walls</param>
    /// <param name="heatSeekingRotation">How fast can the projectile adjust its trajectory, degrees/s</param>
    /// <param name="target">Target for heat seeking, null if disabled</param>
    public void Setup(GameObject shooter, float speed, int maxBounces, float heatSeekingRotation, GameObject target = null)
    {
        if (setup)
            return;

        lastPosition = transform.position;
        lastRotation = transform.rotation;
        this.shooter = shooter;
        this.projectileSpeed = speed;
        this.bouncesLeft = maxBounces;
        this.heatSeekingRotation = heatSeekingRotation;
        this.targetTank = target;

        setup = true;
    }

    private void FixedUpdate()
    {
        // TODO:
        // For some GOD KNOWS WHAT reason, sometimes projectile glitches out to position 0,0,0
        CheckLifetime();

        if (shooter && setup)
            MoveProjectile();
    }

    // Calculating and moving the projectile using a rigidbody
    private void MoveProjectile()
    {
        var tPos = lastPosition;
        if (transform.position.y > 0)
            tPos = transform.position;

        var tRot = lastRotation;
        if (transform.rotation.eulerAngles.y != 0)
            tRot = transform.rotation;

        // Handling initial rotation, in case it was overridden somewhere else
        Quaternion initialRotation = tRot;
        if (overridenMovementRotation != null)
        {
            initialRotation = (Quaternion)overridenMovementRotation;
            overridenMovementRotation = null;
        }

        // Calculating the target position (default 3 units forward)
        var aimTowards = tPos + (initialRotation * Vector3.forward);
        if (targetTank != null)
        {
            aimTowards = targetTank.transform.position;
        }

        // Heat seeking based off of
        // https://github.com/Matthew-J-Spencer/Homing-Missile/blob/main/Missile.cs
        var heading = aimTowards - tPos;
        var heatRotation = Quaternion.LookRotation(heading, transform.up);
        // lerping heat rotation
        heatRotation = Quaternion.RotateTowards(initialRotation, heatRotation, heatSeekingRotation * Time.deltaTime);

        // Applying spin and calculating final rotation
        var spinRotation = 360 * rotationsPerSecondZ * Time.fixedDeltaTime;
        var finalRotation = Quaternion.Euler(heatRotation.eulerAngles + new Vector3(0, 0, spinRotation));

        // Calculating forward vector
        var fwVec = projectileSpeed * Time.fixedDeltaTime * heading.normalized;

        // Applying movement & rotation
        rb.Move(
            tPos + fwVec,
            finalRotation
        );
        lastPosition = tPos + fwVec;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckForBasicCollisions(other))
            return;

        overridenMovementRotation = ApplyHorizontalRicochet();
    }
}
