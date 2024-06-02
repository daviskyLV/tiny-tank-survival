using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class StandardProjectile : Projectile
{
    /// <summary>
    /// How long is the projectile allowed to clip through the shooter to avoid accidental hitting
    /// </summary>
    [SerializeField]
    private double allowedClippingTime = 0.2;

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
    /// How many times can the bullet bounce against the walls
    /// </summary>
    private int bouncesLeft = 0;
    /// <summary>
    /// Used to override aiming rotation when moving the projectile
    /// </summary>
    private Quaternion? overridenMovementRotation = null;

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
        Debug.Log($"Projectile setup position: {transform.position}");
        if (setup)
            return;

        this.shooter = shooter;
        this.projectileSpeed = speed;
        this.bouncesLeft = maxBounces;
        this.heatSeekingRotation = heatSeekingRotation;
        this.targetTank = target;

        setup = true;
    }

    private void FixedUpdate()
    {
        CheckLifetime();

        if (shooter && setup)
            MoveProjectile();
    }

    // Calculating and moving the projectile using a rigidbody
    private void MoveProjectile()
    {
        // Handling initial rotation, in case it was overridden somewhere else
        Quaternion initialRotation = transform.rotation;
        if (overridenMovementRotation != null)
        {
            initialRotation = (Quaternion)overridenMovementRotation;
            overridenMovementRotation = null;
        }

        // Calculating the target position (default 3 units forward)
        var aimTowards = rb.position + (initialRotation * Vector3.forward);
        if (targetTank != null)
        {
            aimTowards = targetTank.transform.position;
        }

        // Heat seeking based off of
        // https://github.com/Matthew-J-Spencer/Homing-Missile/blob/main/Missile.cs
        var heading = aimTowards - rb.position;
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
            // todo:
            // fix randomly going to 0,0,0 on first frame
            rb.position + fwVec,
            finalRotation
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == null)
            return; // dunno what happened

        if (other.gameObject.CompareTag("Tank"))
        {
            // Allowing the shooter to clip through the projectile for a short time
            if (Time.timeAsDouble - startTime <= allowedClippingTime && other.gameObject.Equals(shooter))
                return;

            Destroy(other.transform.parent.gameObject); // destroying the player/enemy
            ExplodeProjectile();
            return;
        }
        else if (other.gameObject.CompareTag("Projectile")) {
            ExplodeProjectile();
            return;
        }

        // Probably a wall or something
        if (bouncesLeft <= 0) {
            ExplodeProjectile();
            return;
        }

        // Bouncing off
        // Creating a ray to get the surface
        RaycastHit hit;
        var tp = transform.position;
        if (Physics.Raycast(tp, transform.forward, out hit, 2.0f + transform.lossyScale.z))
        {
            // really dumb way to ensure Y level stays consistent, im bad at math ok?
            var direction = Vector3.Reflect(transform.forward, hit.normal).normalized; //tp + Vector3.Reflect(transform.forward, hit.normal);
            direction.y = 0; // Allowing bounces only on horizontal axis
            overridenMovementRotation = Quaternion.LookRotation(direction, transform.up);
            bouncesLeft--;
        }
    }
}
