using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tankPrefabs;
    [SerializeField]
    private GameObject playerCharacterPrefab;

    private GameObject playerCharacter;
    private AsyncOperation asyncLoad;

    /// <summary>
    /// Invoked whenever the player spawns. Argument is player's character in the level.
    /// </summary>
    public static event Action<GameObject> OnPlayerSpawned;

    // Start is called before the first frame update
    void Start()
    {
        asyncLoad = SceneManager.LoadSceneAsync("SandyPingPong", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && asyncLoad.isDone) {
            if (playerCharacter != null)
                Destroy(playerCharacter);

            Scene pingPongScene = SceneManager.GetSceneByName("SandyPingPong");
            // Instantiating the new player character
            playerCharacter = Instantiate(playerCharacterPrefab);
            playerCharacter.name = "PlayerCharacter";
            var playerTank = Instantiate(tankPrefabs[0], playerCharacter.transform);
            playerTank.name = "Tank";

            // Moving the player character to level scene and setting it up
            SceneManager.MoveGameObjectToScene(playerCharacter, pingPongScene);
            playerCharacter.transform.position = GameObject.Find("PlayerSpawn").transform.position;

            // Enabling necessary components
            playerCharacter.GetComponent<PlayerMovement>().enabled = true;
            playerCharacter.GetComponent<PlayerShooting>().enabled = true;

            // Letting others know that we just spawned the player
            OnPlayerSpawned?.Invoke(playerCharacter);
        }
    }
}
