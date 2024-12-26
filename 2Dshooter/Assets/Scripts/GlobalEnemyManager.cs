using UnityEngine;

public class GlobalEnemyManager : MonoBehaviour
{
    public static GlobalEnemyManager instance;

    [Header("Enemy Settings")]
    public int totalEnemiesToSpawn = 10; // Number of enemies you want to spawn, adjustable in the Unity Inspector

    private int totalEnemiesSpawned = 0;     // Track the total number of spawned enemies
    private int totalEnemiesKilled = 0;      // Track the total number of killed enemies

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
        totalEnemiesSpawned++;
        Debug.Log($"Total enemies spawned: {totalEnemiesSpawned}");

        // Check if the number of spawned enemies has reached the specified limit
        if (totalEnemiesSpawned >= totalEnemiesToSpawn)
        {
            Debug.Log("All enemies have been spawned.");
        }
    }

    // Called when an enemy is killed
    public void IncrementKilled()
    {
        totalEnemiesKilled++;
        Debug.Log($"Total enemies killed: {totalEnemiesKilled} / {totalEnemiesSpawned}");

        // Check if all enemies have been killed
        if (totalEnemiesSpawned == totalEnemiesKilled)
        {
            Debug.Log("All enemies killed! Showing victory screen.");
            VictoryManager.instance.ShowVictoryScreen();  // Trigger victory screen
        }
    }

    // Getter methods (optional)
    public int GetTotalEnemiesSpawned() => totalEnemiesSpawned;
    public int GetTotalEnemiesKilled() => totalEnemiesKilled;
}
