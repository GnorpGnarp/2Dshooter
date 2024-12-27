using System.Collections;
using UnityEngine;

public class GlobalEnemyManager : MonoBehaviour
{
    public static GlobalEnemyManager instance;

    [Header("Enemy Settings")]
    public int totalEnemiesToSpawn = 10; // Number of enemies you want to spawn, adjustable in the Unity Inspector

    private int totalEnemiesSpawned = 0;     // Track the total number of spawned enemies
    private int totalEnemiesKilled = 0;      // Track the total number of killed enemies
    private bool allEnemiesSpawned = false; // Track if all enemies have been spawned

    private bool victoryScreenTriggered = false;  // Flag to ensure victory screen is only shown once

    void Awake()
    {
        // Ensure only one instance of the GlobalEnemyManager exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called when an enemy is spawned
    public void IncrementSpawned()
    {
        if (allEnemiesSpawned) return;  // Prevent spawning once all enemies have been spawned.

        totalEnemiesSpawned++;
        Debug.Log($"Total enemies spawned: {totalEnemiesSpawned}");

        if (totalEnemiesSpawned >= totalEnemiesToSpawn)
        {
            allEnemiesSpawned = true;
            Debug.Log("All enemies have been spawned.");
        }
    }

    // Called when an enemy is killed
    public void IncrementKilled()
    {
        // Don't allow scoring if the victory screen has been triggered
        if (victoryScreenTriggered) return;

        totalEnemiesKilled++;
        Debug.Log($"Total enemies killed: {totalEnemiesKilled} / {totalEnemiesSpawned}");

        // Check if all enemies have been spawned and killed
        if (allEnemiesSpawned && totalEnemiesSpawned == totalEnemiesKilled)
        {
            Debug.Log("All enemies killed! Showing victory screen.");
            victoryScreenTriggered = true;

            // Start the coroutine to show the victory screen with a delay
            StartCoroutine(ShowVictoryWithDelay());
        }
    }

    private IEnumerator ShowVictoryWithDelay()
    {
        // Wait for 0.5 seconds before continuing
        yield return new WaitForSeconds(0.5f);

        // Trigger the victory screen
        VictoryManager.instance.ShowVictoryScreen();
    }


}
