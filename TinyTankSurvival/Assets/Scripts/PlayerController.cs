using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tankPrefabs;
    [SerializeField]
    private GameObject playerCharacterPrefab;
    [SerializeField]
    private CameraScript mainCameraScript;

    private GameObject playerCharacter;
    private AsyncOperation asyncLoad;

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

            mainCameraScript.PlayerTank = playerTank;
            // Movement stuff
            var movementScript = playerCharacter.GetComponent<PlayerMovement>();
            movementScript.Tank = playerTank;
            playerCharacter.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
