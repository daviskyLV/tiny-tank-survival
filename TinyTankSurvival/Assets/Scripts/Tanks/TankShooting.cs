using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankShooting : MonoBehaviour
{
    [SerializeField]
    protected GameObject projectilePrefab;
    [SerializeField]
    protected GameObject gun;

    /// <summary>
    /// Simplified shooting method that checks if the player or enemy shot the projectile based on the tank's parent name.
    /// </summary>
    public abstract void Shoot();
}
