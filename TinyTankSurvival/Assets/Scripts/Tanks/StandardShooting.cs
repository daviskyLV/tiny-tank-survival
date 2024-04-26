using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StandardShooting : TankShooting
{
    /// <summary>
    /// Shoots a rocket out of the barrel
    /// </summary>
    /// <param name="projectileSpeed">How fast the projectile travels</param>
    /// <param name="heatSeekingSpeed">Projectile heat seeking speed, measured in degrees/s</param>
    /// <param name="target">Heat seeking target, null if disabled</param>
    public void Shoot(float projectileSpeed, float heatSeekingSpeed, GameObject target = null)
    {
        var gt = gun.transform;

        var projectile = Instantiate(projectilePrefab);
        SceneManager.MoveGameObjectToScene(projectile, gameObject.scene);
        var rt = projectile.transform;
        var projectileController = projectile.GetComponent<StandardProjectile>();
        rt.position = gt.position + gt.forward * (gt.lossyScale.z + rt.lossyScale.z);
        projectileController.enabled = true;
        projectileController.Setup(transform.gameObject, projectileSpeed, heatSeekingSpeed, target);
    }
}
