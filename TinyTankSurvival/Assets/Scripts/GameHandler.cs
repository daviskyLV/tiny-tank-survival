using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField]
    private GameObject menuTutorialPrefab;
    [SerializeField]
    private GameObject menuTutorial;
    [SerializeField]
    private string mapSceneName = "SandyPingPong";

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

    public GameObject PlayerCharacter { get; private set; }
    private AsyncOperation loadingMap;
    private GameObject gameHUD;
    private Scene mapPlayed;

    private PlayerInputActions playerControls;
    private InputAction fireAction;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playerControls = new PlayerInputActions();
        }
    }

    private void OnEnable()
    {
        fireAction = playerControls.Player.Fire;
        fireAction.Enable();
        fireAction.performed += InitiateGameStart;
    }

    private void OnDisable()
    {
        fireAction.Disable();
        fireAction.performed -= InitiateGameStart;
    }

    private void InitiateGameStart(InputAction.CallbackContext context)
    {
        if (!(loadingMap == null || loadingMap.isDone))
            return;

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

    private void StartNewGame()
    {
        OnGameStarted?.Invoke();
        loadingMap = SceneManager.LoadSceneAsync(mapSceneName, LoadSceneMode.Additive);
        gameHUD = Instantiate(gameHUDPrefab);
        loadingMap.completed += (asyncOperation) =>
        {
            mapPlayed = SceneManager.GetSceneByName(mapSceneName);
            SpawnPlayer();
            if (menuTutorial != null)
                Destroy(menuTutorial);
        };
    }

    private void SpawnPlayer()
    {
        // Instantiating the new player character
        var pChar = Instantiate(playerCharacterPrefab);
        pChar.name = Constants.PlayerCharacterName;
        var playerTank = Instantiate(tankPrefabs[0], pChar.transform);
        playerTank.name = Constants.TankName;

        // Moving the player character to level scene and setting it up
        SceneManager.MoveGameObjectToScene(pChar, mapPlayed);
        pChar.transform.position = GameObject.Find(Constants.PlayerSpawnName).transform.position;

        // Enabling necessary components
        pChar.GetComponent<PlayerMovement>().enabled = true;
        pChar.GetComponent<PlayerShooting>().enabled = true;
        pChar.GetComponent<PlayerAiming>().enabled = true;

        // Letting others know that we just spawned the player
        PlayerCharacter = pChar;
        OnPlayerSpawned?.Invoke(PlayerCharacter);
    }
}
