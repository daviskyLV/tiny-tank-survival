using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class Projectile : MonoBehaviour
{
    [SerializeField]
    protected float rotationsPerSecondZ; // rotation on Z axis
    [SerializeField]
    protected Rigidbody rb;
    [SerializeField]
    private double lifetime;

    /// <summary>
    /// Tank which shot the projectile, if null projectile is disabled
    /// </summary>
    protected GameObject shooter;

    protected double startTime = 0.0;

    protected void CheckLifetime()
    {
        if (Time.timeAsDouble - startTime > lifetime)
            Destroy(gameObject);
    }
}
