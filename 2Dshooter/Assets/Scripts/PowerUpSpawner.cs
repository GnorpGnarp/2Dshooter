using System.Collections;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs;  // Power-up prefabs (BulletMultiplier, HealthUp, etc.)
    public float spawnRate = 5f;         // Interval between power-up spawns
    public float minXAxispawnValue = -8f;
    public float maxXAxisSpawnValue = 8f;
    public float yAxisSpawnValue = 4f;  // Start at the top of the screen

    private float timeSinceLastAction = 0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPowerUp());
    }

    // This method controls power-up spawning and movement
    IEnumerator SpawnPowerUp()
    {
        while (true)
        {
            timeSinceLastAction += Time.deltaTime;

            if (timeSinceLastAction >= spawnRate)
            {
                // Randomly choose a power-up prefab to spawn
                int randomIndex = Random.Range(0, powerUpPrefabs.Length);
                GameObject powerUpPrefab = powerUpPrefabs[randomIndex];

                // Randomize spawn position
                float randomX = Random.Range(minXAxispawnValue, maxXAxisSpawnValue);
                Vector2 spawnPosition = new Vector2(randomX, yAxisSpawnValue);

                // Instantiate the power-up and set its initial position
                GameObject powerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);

                // Set the power-up to fall down (gravity will make it fall naturally, or we can apply movement)
                PowerUp powerUpScript = powerUp.GetComponent<PowerUp>();  // Assuming PowerUp script exists on power-up prefab

                // You can optionally make the power-up fall at a constant rate
                powerUpScript.GetComponent<Rigidbody2D>().gravityScale = 1f;  // Use gravity for falling

                // Reset the time counter
                timeSinceLastAction = 0f;
            }

            yield return null;
        }
    }
}
