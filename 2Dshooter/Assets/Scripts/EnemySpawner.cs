using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float spawnRate = 2f;
    public int maxEnemies = 10;

    public float minXAxispawnValue = -8f;
    public float maxXAxisSpawnValue = 8f;

    public float yAxisSpawnValue = 4f;

    private float timeSinceLastAction = 0f;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    // Add a reference to the VictoryManager
    public VictoryManager victoryManager;

    // Start is called before the first frame update
    void Start()
    {
        // Find the VictoryManager in the scene
        victoryManager = FindObjectOfType<VictoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastAction += Time.deltaTime;

        if (timeSinceLastAction >= spawnRate && spawnedEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
        }

        // Clean up dead enemies and check for victory condition
        RemoveDeadEnemies();
        CheckForVictory();
    }

    void SpawnEnemy()
    {
        float xPosition = Random.Range(minXAxispawnValue, maxXAxisSpawnValue);
        Vector2 spawnPosition = new Vector2(xPosition, yAxisSpawnValue);

        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, this.transform);

        timeSinceLastAction = 0f;
        spawnedEnemies.Add(spawnedEnemy);
    }

    void RemoveDeadEnemies()
    {
        spawnedEnemies.RemoveAll(enemy => enemy == null);
    }

    void CheckForVictory()
    {
        // If there are no more enemies left in the scene
        if (spawnedEnemies.Count == 0)
        {
            victoryManager.ShowVictoryScreen();
        }
    }
}
