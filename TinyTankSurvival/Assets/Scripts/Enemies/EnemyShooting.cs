using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AmmoManager))]
public class EnemyShooting : MonoBehaviour
{
    [SerializeField]
    private Transform tank;
    [SerializeField]
    private Transform gun;
    [SerializeField]
    private TankAiming tankAiming;
    [SerializeField]
    private TankShooting tankShooting;
    [SerializeField]
    [Range(0.1f, 5f)]
    private float shootingInterval = 1f;
    [SerializeField]
    private AmmoManager ammoManager;
    [SerializeField]
    private float range = 25f;

    private Transform playerTank;
    private bool setup;
    private double lastShot;

    // Update is called once per frame
    void Update()
    {
        if (!setup || playerTank == null)
            return;

        AimAtPlayer();
        ShootAtPlayer();
    }

    public void Setup(Transform playerTank)
    {
        this.playerTank = playerTank;
        setup = true;
    }

    private void ShootAtPlayer()
    {
        if (lastShot + shootingInterval > Time.timeAsDouble)
            return;

        // Time to shoot
        // Checking if something is in front of player or out of range
        if (Vector3.Distance(tank.position, playerTank.position) > range)
        {
            // Out of range
            return;
        }

        // Close to player, but is it in line of sight?
        RaycastHit hit;
        Ray ray = new Ray(tank.position, gun.forward);
        // https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
        // Bit shift the index of the layer (8) to get a bit mask
        // This would cast rays only against colliders in layer 8 (character layer).
        int layerMask = 1 << 9;
        if (!Physics.Raycast(ray, out hit, range))
        {
            // Ray detection out of range (nothing hit)
            return;
        }

        if (playerTank != hit.collider.transform)
        {
            // Something is blocking the view
            return;
        }

        // Can see player, time to shoot
        if (!ammoManager.SpendAmmo())
            return;

        // Successful shot
        lastShot = Time.timeAsDouble;
        tankShooting.Shoot();
    }

    private void AimAtPlayer()
    {
        tankAiming.AimPosition = playerTank.position;
    }
}
