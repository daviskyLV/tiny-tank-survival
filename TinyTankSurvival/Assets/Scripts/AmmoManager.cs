using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    /// <summary>
    /// How long does it take for 1 bullet to replenish
    /// </summary>
    public float ReloadTime { get; private set; }
    /// <summary>
    /// Minimum reload time for 1 bullet
    /// </summary>
    [SerializeField]
    [Min(0.0f)]
    private float MinReloadTime = 0.1f;
    /// <summary>
    /// Added reload time per each upgrade
    /// </summary>
    [SerializeField]
    private float ReloadTimeUpgrade = -0.2f;
    [SerializeField]
    [Range(0.1f, 10f)]
    private float DefaultReloadTime = 2.5f;

    /// <summary>
    /// How many bullets can the player/enemy have at any given time
    /// </summary>
    public int AmmoCapacity { get; private set; }
    /// <summary>
    /// Maximum ammo capacity for the player/enemy
    /// </summary>
    [SerializeField]
    [Range(1, 10)]
    private int MaxAmmoCapacity = 10;
    /// <summary>
    /// Added ammo capacity per each upgrade
    /// </summary>
    [SerializeField]
    private int AmmoCapacityUpgrade = 1;
    [SerializeField]
    [Range(1, 10)]
    private int DefaultAmmoCapacity = 3;

    public int Ammo {  get; private set; }
    /// <summary>
    /// Invoked whenever 1 ammo has been reloaded
    /// </summary>
    public event Action OnAmmoReloaded;

    // Start is called before the first frame update
    void Start()
    {
        ReloadTime = DefaultReloadTime;
        AmmoCapacity = DefaultAmmoCapacity;
        Ammo = AmmoCapacity;
    }

    /// <summary>
    /// Spend X amount of ammo. On success automatically refills ammo 1 by 1 after reload time
    /// </summary>
    /// <param name="amount">How much ammo to spend</param>
    /// <returns>True if successful, false if failed</returns>
    public bool SpendAmmo(int amount) {
        if (Ammo < amount)
            return false;

        Ammo -= amount;
        StartCoroutine(ReloadCoroutine(amount));

        return true;
    }

    /// <summary>
    /// Spend 1 ammo. On success automatically refills 1 ammo after reload time
    /// </summary>
    /// <returns>True if successful, false if failed</returns>
    public bool SpendAmmo() {
        return SpendAmmo(1);
    }

    /// <summary>
    /// Reloads X amount of ammo, one by one
    /// </summary>
    /// <param name="amount">How much ammo to reload</param>
    private IEnumerator ReloadCoroutine(int amount)
    {
        var reloaded = 0;
        var lastReload = Time.timeAsDouble;
        while (reloaded < amount && Ammo < AmmoCapacity) {
            if (lastReload + ReloadTime <= Time.timeAsDouble) {
                reloaded++;
                OnAmmoReloaded?.Invoke();
            } else
            {
                yield return null;
            }
        }
    }
}
