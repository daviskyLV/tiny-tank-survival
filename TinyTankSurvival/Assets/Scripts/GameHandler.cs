using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

// Singleton to handle some game related stuff
public class GameHandler : MonoBehaviour
{
    /// <summary>
    /// The GameHandler's Singleton instance
    /// </summary>
    public static GameHandler Instance { get; private set; }

    /// <summary>
    /// Current game score
    /// </summary>
    public long Score { get; private set; }
    /// <summary>
    /// Currently played round
    /// </summary>
    public int Round {  get; private set; }
    /// <summary>
    /// Invoked whenever the player spawns. Argument is player's character in the level.
    /// </summary>
    public static event Action<GameObject> OnPlayerSpawned;
    /// <summary>
    /// Invoked whenever the game starts.
    /// </summary>
    public static event Action OnGameStarted;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
