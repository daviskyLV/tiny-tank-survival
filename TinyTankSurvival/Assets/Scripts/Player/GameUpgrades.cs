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
        //ReloadTime = DefaultReloadTime;
        //AmmoCapacity = DefaultAmmoCapacity;
    }
}
