using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooting : MonoBehaviour
{
    [SerializeField]
    private GameObject rocketPrefab;
    [SerializeField]
    private GameObject gun;

    /// <summary>
    /// Shoots a rocket out of the tank
    /// </summary>
    public void Shoot()
    {
        var gt = gun.transform;

        var rocket = Instantiate(rocketPrefab);
        var rt = rocket.transform;
        var rocketController = rocket.GetComponent<RocketController>();
        rocketController.Shooter = transform.gameObject;

        rt.position = gt.position + gt.forward * (gt.lossyScale.z + rt.lossyScale.y);
        rocketController.enabled = true;
    }
}
