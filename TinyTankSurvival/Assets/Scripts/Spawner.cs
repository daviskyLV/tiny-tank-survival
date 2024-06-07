using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private int maxEnemiesPerSpawn = 1;
    [SerializeField]
    private GameObject enemySpawnpointParent;
    [SerializeField]
    private GameObject enemyHolder;
    [SerializeField]
    private double spawningInterval = 3.5;
    [SerializeField]
    private GameObject enemyPrefab;

    private GameHandler gHandler;

    private List<GameObject> enemySpawnpoints = new();
    private double lastSpawntime = 0;

    private GameObject playerCharacter;
    private Transform playerTank;

    // Events
    /// <summary>
    /// Invoked whenever an enemy spawns. Argument is enemy's character in the level.
    /// </summary>
    public static event Action<GameObject> OnEnemySpawned;

    // Start is called before the first frame update
    void Start()
    {
        gHandler = GameHandler.Instance;
        StartCoroutine(WaitForPlayerCharacter());
        // Getting all the enemy spawnpoints
        foreach (Transform t in enemySpawnpointParent.transform)
        {
            enemySpawnpoints.Add(t.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCharacter == null)
            return;

        SpawnEnemy();
    }

    private IEnumerator WaitForPlayerCharacter() {
        while (gHandler.PlayerCharacter == null)
        {
            yield return null;
        }
        var character = gHandler.PlayerCharacter;

        playerCharacter = character;
        playerTank = playerCharacter.transform.Find(Constants.TankName);
    }

    private void SpawnEnemy()
    {
        if (
            enemyHolder.transform.childCount >= maxEnemiesPerSpawn * enemySpawnpoints.Count ||
            Time.timeAsDouble - lastSpawntime < spawningInterval
            ) {
            return;
        }

        // Getting player character
        if (playerTank == null)
            return;

        var enemy = Instantiate(enemyPrefab, enemyHolder.transform);
        var chosenSpawn = enemySpawnpoints[UnityEngine.Random.Range(0, enemySpawnpoints.Count)];
        enemy.transform.position = chosenSpawn.transform.position;
        enemy.GetComponent<EnemyMovement>().Setup(playerTank);

        // Invoking events
        OnEnemySpawned?.Invoke(enemy);

        lastSpawntime = Time.timeAsDouble;
    }
}
