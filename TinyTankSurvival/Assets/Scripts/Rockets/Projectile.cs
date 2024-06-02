using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class Projectile : MonoBehaviour
{
    /// <summary>
    /// Spinning rotation on Z axis
    /// </summary>
    [SerializeField]
    protected float rotationsPerSecondZ;
    [SerializeField]
    protected Rigidbody rb;
    /// <summary>
    /// The projectile's max lifetime before it automatically explodes
    /// </summary>
    [SerializeField]
    private double lifetime;
    /// <summary>
    /// How long is the projectile allowed to clip through the shooter to avoid accidental hitting
    /// </summary>
    [SerializeField]
    protected double allowedClippingTime = 0.2;

    /// <summary>
    /// Tank which shot the projectile, if null projectile is disabled
    /// </summary>
    protected GameObject shooter;
    /// <summary>
    /// How many times can the bullet bounce against the walls
    /// </summary>
    protected int bouncesLeft = 0;

    protected double startTime = 0.0;

    protected void CheckLifetime()
    {
        if (Time.timeAsDouble - startTime > lifetime)
            ExplodeProjectile();
    }

    /// <summary>
    /// Play the explosion animation and clean up the projectile
    /// </summary>
    protected void ExplodeProjectile()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Does the basic collision checks (null, hitting another tank/projectile)
    /// </summary>
    /// <param name="other">The other collider that was hit</param>
    /// <returns>False if no basic collisions passed</returns>
    protected bool CheckForBasicCollisions(Collider other)
    {
        if (other.gameObject == null)
            return true; // dunno what happened

        if (other.gameObject.CompareTag("Tank"))
        {
            // Allowing the shooter to clip through the projectile for a short time
            if (Time.timeAsDouble - startTime <= allowedClippingTime && other.gameObject.Equals(shooter))
                return true;

            Destroy(other.transform.parent.gameObject); // destroying the player/enemy
            ExplodeProjectile();
            return true;
        }
        else if (other.gameObject.CompareTag("Projectile"))
        {
            ExplodeProjectile();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Applies ricochet rotation only on horizontal axis (x and z)
    /// </summary>
    /// <returns></returns>
    protected Quaternion? ApplyHorizontalRicochet()
    {
        // Probably a wall or something
        if (bouncesLeft <= 0)
        {
            ExplodeProjectile();
            return null;
        }

        // Bouncing off
        // Creating a ray to get the surface
        RaycastHit hit;
        var tp = transform.position;
        if (Physics.Raycast(tp, transform.forward, out hit, 2.0f + transform.lossyScale.z))
        {
            var direction = Vector3.Reflect(transform.forward, hit.normal).normalized; //tp + Vector3.Reflect(transform.forward, hit.normal);
            direction.y = 0; // Allowing bounces only on horizontal axis
            bouncesLeft--;
            return Quaternion.LookRotation(direction, transform.up);
        }

        return null;
    }
}
