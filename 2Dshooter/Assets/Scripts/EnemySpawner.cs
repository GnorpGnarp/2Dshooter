using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float spawnRate = 2f;
    public int maxEnemies = 10;  // Maximum number of enemies in the scene, exposed in the Inspector

    public float minXAxispawnValue = -8f;
    public float maxXAxisSpawnValue = 8f;

    public float yAxisSpawnValue = 4f;

    private float timeSinceLastAction = 0f;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    private bool gameOver = false; // Flag to indicate the game is over

    private bool allEnemiesDead = false; // Track if all enemies are dead

    void Start()
    {
        // Ensure game is not over when the scene starts
        gameOver = false;
        allEnemiesDead = false;
    }

    void Update()
    {
        // Don't spawn enemies or trigger victory if the game is over
        if (gameOver) return;

        timeSinceLastAction += Time.deltaTime;

        // Only spawn if we have fewer enemies than the maximum allowed
        if (timeSinceLastAction >= spawnRate && spawnedEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
        }

        // Check if all enemies are dead and no more are spawning
        if (spawnedEnemies.Count == 0 && !gameOver && !allEnemiesDead)
        {
            allEnemiesDead = true;  // Mark that all enemies are dead
            VictoryManager.instance.ShowVictoryScreen();  // Trigger victory screen
        }
    }

    void SpawnEnemy()
    {
        // Spawn the enemy at a random x position
        float xPosition = Random.Range(minXAxispawnValue, maxXAxisSpawnValue);
        Vector2 spawnPosition = new Vector2(xPosition, yAxisSpawnValue);

        // Instantiate the enemy and add it to the list
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, this.transform);

        // Reset the time counter and add the spawned enemy to the list
        timeSinceLastAction = 0f;
        spawnedEnemies.Add(spawnedEnemy);
    }

    // Optionally, you could clean up the list of enemies if they are destroyed:
    public void RemoveDeadEnemies()
    {
        // Remove enemies from the list when they are destroyed
        spawnedEnemies.RemoveAll(enemy => enemy == null);

        // If all enemies are removed, then trigger the victory screen if conditions are met
        if (spawnedEnemies.Count == 0 && !gameOver && !allEnemiesDead)
        {
            allEnemiesDead = true;  // Mark that all enemies are dead
            VictoryManager.instance.ShowVictoryScreen();  // Trigger victory screen
        }
    }
}
