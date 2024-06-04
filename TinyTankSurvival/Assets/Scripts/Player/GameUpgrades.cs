using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameHandler))]
public class GameUpgrades : MonoBehaviour
{
    /// <summary>
    /// The GameUpgrades's Singleton instance
    /// </summary>
    public static GameUpgrades Instance { get; private set; }
    [SerializeField]
    private GameHandler gameHandler;

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

    void Awake()
    {
        if (Instance != null)
            return;
        
        Instance = this;
        GameHandler.OnGameStarted += ResetUpgrades;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetUpgrades();
    }

    /// <summary>
    /// Resets all upgrade values to their defaults
    /// </summary>
    private void ResetUpgrades()
    {
        ReloadTime = DefaultReloadTime;
    }
}
