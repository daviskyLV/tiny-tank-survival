using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StandardShooting : TankShooting
{
    /// <summary>
    /// Unified method to create and activate the projectile
    /// </summary>
    /// <param name="projectileSpeed">How fast the projectile travels</param>
    /// <param name="bounces">How many times can the projectile bounce off</param>
    /// <param name="heatSeekingSpeed">Projectile heat seeking speed, measured in degrees/s</param>
    /// <param name="target">Heat seeking target, null if disabled</param>
    private void CreateProjectile(float projectileSpeed, int bounces, float heatSeekingSpeed, GameObject target = null)
    {
        var gt = gun.transform;

        var projectile = Instantiate(projectilePrefab);
        var rt = projectile.transform;
        rt.position = gt.position + gt.forward * ((gt.lossyScale.z + rt.lossyScale.z) / 1.9f);
        rt.LookAt(rt.position + gt.forward);
        SceneManager.MoveGameObjectToScene(projectile, gameObject.scene);

        // Managing projectile script
        var projectileController = projectile.GetComponent<StandardProjectile>();
        projectileController.enabled = true;
        projectileController.Setup(transform.gameObject, projectileSpeed, bounces, heatSeekingSpeed, target);
    }

    /// <summary>
    /// Shoots a rocket out of the barrel
    /// </summary>
    /// <param name="projectileSpeed">How fast the projectile travels</param>
    /// <param name="heatSeekingSpeed">Projectile heat seeking speed, measured in degrees/s</param>
    /// <param name="target">Heat seeking target, null if disabled</param>
    public void Shoot(float projectileSpeed, float heatSeekingSpeed, GameObject target = null)
    {
        CreateProjectile(projectileSpeed, 3, heatSeekingSpeed, target);
    }

    public override void Shoot()
    {

        CreateProjectile(5.0f, 3000, 0.0f, null);
    }
}
