using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Singleton to handle some game related stuff
public class GameHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tankPrefabs;
    [SerializeField]
    private GameObject playerCharacterPrefab;
    [SerializeField]
    private GameObject gameHUDPrefab;

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

    private GameObject playerCharacter;
    private AsyncOperation loadingMap;
    private GameObject gameHUD;
    private Scene mapPlayed;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) &&
            (loadingMap == null || loadingMap.isDone))
        {
            if (mapPlayed == null || !mapPlayed.IsValid())
            {
                StartNewGame();
                return;
            }

            // Unloading currently played game
            if (gameHUD != null)
                Destroy(gameHUD);

            loadingMap = SceneManager.UnloadSceneAsync(mapPlayed);
            // How to use AsyncOperation.completed
            // https://discussions.unity.com/t/how-to-use-asyncoperation-completed/226417
            loadingMap.completed += (asyncOperation) =>
            {
                StartNewGame();
            };
        }
    }

    private void StartNewGame()
    {
        OnGameStarted?.Invoke();
        loadingMap = SceneManager.LoadSceneAsync("SandyPingPong", LoadSceneMode.Additive);
        gameHUD = Instantiate(gameHUDPrefab);
        loadingMap.completed += (asyncOperation) =>
        {
            mapPlayed = SceneManager.GetSceneByName("SandyPingPong");
            SpawnPlayer();
        };
    }

    private void SpawnPlayer()
    {
        // Instantiating the new player character
        playerCharacter = Instantiate(playerCharacterPrefab);
        playerCharacter.name = Constants.PlayerCharacterName;
        var playerTank = Instantiate(tankPrefabs[0], playerCharacter.transform);
        playerTank.name = Constants.TankName;

        // Moving the player character to level scene and setting it up
        SceneManager.MoveGameObjectToScene(playerCharacter, mapPlayed);
        playerCharacter.transform.position = GameObject.Find(Constants.PlayerSpawnName).transform.position;

        // Enabling necessary components
        playerCharacter.GetComponent<PlayerMovement>().enabled = true;
        playerCharacter.GetComponent<PlayerShooting>().enabled = true;
        playerCharacter.GetComponent<PlayerAiming>().enabled = true;

        // Letting others know that we just spawned the player
        OnPlayerSpawned?.Invoke(playerCharacter);
    }
}
