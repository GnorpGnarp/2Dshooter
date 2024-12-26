using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;           // The enemy prefab to spawn
    public float spawnRate = 2f;             // The rate at which enemies spawn (in seconds)
    public int maxEnemies = 10;              // Max number of enemies in the scene at once
    public float minXAxispawnValue = -8f;   // Minimum spawn X position
    public float maxXAxisSpawnValue = 8f;   // Maximum spawn X position
    public float yAxisSpawnValue = 4f;      // Fixed Y position for enemy spawn

    private float timeSinceLastAction = 0f;  // Time accumulator for spawn rate
    private List<GameObject> spawnedEnemies = new List<GameObject>();  // List to track all spawned enemies

    void Start()
    {
        // Optionally, you can initialize anything here if needed
        Debug.Log("Spawner initialized");
    }

    void Update()
    {
        // Accumulate time since the last spawn
        timeSinceLastAction += Time.deltaTime;

        // Check if we need to spawn a new enemy
        if (spawnedEnemies.Count < maxEnemies && timeSinceLastAction >= spawnRate)
        {
            SpawnEnemy();
        }

        // Check for dead enemies in the list and remove them
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] == null)  // Enemy is dead
            {
                // Remove the dead enemy from the list
                spawnedEnemies.RemoveAt(i);

                // Notify the global manager that an enemy was killed
                GlobalEnemyManager.instance.IncrementKilled();
            }
        }
    }

    // Method to spawn a new enemy
    void SpawnEnemy()
    {
        // Randomize the spawn position within the given x range
        float xPosition = Random.Range(minXAxispawnValue, maxXAxisSpawnValue);
        Vector2 spawnPosition = new Vector2(xPosition, yAxisSpawnValue);

        // Instantiate the new enemy at the spawn position
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Add the new enemy to the list of spawned enemies
        spawnedEnemies.Add(spawnedEnemy);

        // Notify the global manager that a new enemy was spawned
        GlobalEnemyManager.instance.IncrementSpawned();

        // Reset the spawn timer to control spawn rate
        timeSinceLastAction = 0f;
    }
public void RemoveDeadEnemies()
    {
        // Iterate through the list of spawned enemies in reverse to avoid index issues while removing
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] == null)  // Check if the enemy is dead
            {
                spawnedEnemies.RemoveAt(i);  // Remove the dead enemy from the list
            }
        }
    }

}
