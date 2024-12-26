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
    private bool gameOver = false;
    private bool allEnemiesDead = false;

    // Counters for spawned and killed enemies
    private int totalEnemiesSpawned = 0;
    private int totalEnemiesKilled = 0;

    void Start()
    {
        // Ensure game is not over when the scene starts
        gameOver = false;
        allEnemiesDead = false;
    }

    void Update()
    {
        if (gameOver) return;  // Don't spawn enemies if the game is over

        timeSinceLastAction += Time.deltaTime;

        // Only spawn if we have fewer enemies than the maximum allowed
        if (timeSinceLastAction >= spawnRate && spawnedEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
        }

        // Check if all enemies are dead and trigger victory if conditions are met
        if (totalEnemiesSpawned == totalEnemiesKilled && totalEnemiesSpawned > 0 && !allEnemiesDead)
        {
            allEnemiesDead = true;
            Debug.Log("All enemies spawned and killed. Showing victory screen.");
            VictoryManager.instance.ShowVictoryScreen();
        }
    }

    void SpawnEnemy()
    {
        // Spawn the enemy at a random x position
        float xPosition = Random.Range(minXAxispawnValue, maxXAxisSpawnValue);
        Vector2 spawnPosition = new Vector2(xPosition, yAxisSpawnValue);

        // Instantiate the enemy and add it to the list
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, this.transform);

        // Increment the spawned enemy counter
        totalEnemiesSpawned++;

        // Reset the time counter and add the spawned enemy to the list
        timeSinceLastAction = 0f;
        spawnedEnemies.Add(spawnedEnemy);
    }

    public void RemoveDeadEnemies()
    {
        // Log how many enemies are in the list before removal
        Debug.Log($"Enemies before removal: {spawnedEnemies.Count}");

        // Remove enemies from the list when they are destroyed
        spawnedEnemies.RemoveAll(enemy => enemy == null);

        // Log the number of enemies after removal
        Debug.Log($"Enemies after removal: {spawnedEnemies.Count}");

        // Increment the killed enemy counter
        totalEnemiesKilled++;

        // Log how many enemies have been killed
        Debug.Log($"Enemies killed: {totalEnemiesKilled} / {totalEnemiesSpawned}");

        // Check if all enemies are dead and show the victory screen
        if (totalEnemiesSpawned == totalEnemiesKilled && !allEnemiesDead)
        {
            allEnemiesDead = true;
            Debug.Log("All enemies removed. Triggering victory screen.");
            VictoryManager.instance.ShowVictoryScreen();
        }
    }


    // Call this method to reset the counters when starting a new level or game session
    public void ResetCounters()
    {
        totalEnemiesSpawned = 0;
        totalEnemiesKilled = 0;
        allEnemiesDead = false;
        gameOver = false;
    }
}
