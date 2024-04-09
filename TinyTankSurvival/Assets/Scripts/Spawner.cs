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

    private List<GameObject> enemySpawnpoints = new();
    private double lastSpawntime = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Getting all the enemy spawnpoints
        foreach (Transform t in enemySpawnpointParent.transform)
        {
            enemySpawnpoints.Add(t.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (
            enemyHolder.transform.childCount >= maxEnemiesPerSpawn * enemySpawnpoints.Count ||
            Time.realtimeSinceStartupAsDouble - lastSpawntime < spawningInterval
            ) {
            return;
        }

        // Getting player character
        var playerCharacter = GameObject.Find("PlayerCharacter");
        if (playerCharacter == null)
            return;
        var playerTank = playerCharacter.transform.Find("Tank");
        if (playerTank == null)
            return;

        var enemy = Instantiate(enemyPrefab, enemyHolder.transform);
        var chosenSpawn = enemySpawnpoints[Random.Range(0, enemySpawnpoints.Count)];
        enemy.transform.position = chosenSpawn.transform.position;
        enemy.GetComponent<EnemyMovement>().PlayerTank = playerTank.gameObject;

        lastSpawntime = Time.realtimeSinceStartupAsDouble;
    }
}
