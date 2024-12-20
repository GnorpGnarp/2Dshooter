using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public string difficulty = "Easy";  // Difficulty of the enemy
    public float speed = 0.6f;
    public float fireRate = 1f;
    public float health = 1f;

    public Transform playerTransform;
    public GameObject bulletPrefab;
    public Transform enemyGunEnd;
    public GameObject explosionEffectPrefab;

    private float timeSinceLastAction = 0f;

    // Define score based on difficulty
    private int scoreValue;

    void Start()
    {
        SetDifficulty(difficulty);

        // Find player transform (assumes player object is in the scene)
        GameObject playerGameObject = GameObject.Find("Player");
        if (playerGameObject != null)
        {
            playerTransform = playerGameObject.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found! Please ensure the 'Player' GameObject exists in the scene.");
        }

        // Set score based on difficulty
        SetScoreBasedOnDifficulty();
    }

    public void SetDifficulty(string difficulty)
    {
        switch (difficulty)
        {
            case "Easy":
                speed = 0.6f;
                fireRate = 3f;
                health = 1f;
                break;
            case "Medium":
                speed = 1.0f;
                fireRate = 2f;
                health = 2f;
                break;
            case "Hard":
                speed = 1.5f;
                fireRate = 1f;
                health = 3f;
                break;
        }
    }

    void SetScoreBasedOnDifficulty()
    {
        switch (difficulty)
        {
            case "Easy":
                scoreValue = 100;  // Score for easy enemies
                break;
            case "Medium":
                scoreValue = 200;  // Score for medium enemies
                break;
            case "Hard":
                scoreValue = 300;  // Score for hard enemies
                break;
        }
    }

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y > -2)
            Shoot();

        if (transform.position.y < -5.5f)
        {
            GameManager.playerController.HittedByBullet();  // Handle player hit when enemy passes below screen
            Destroy(gameObject);
        }
    }

    void Shoot()
    {
        timeSinceLastAction += Time.deltaTime;

        if (timeSinceLastAction >= fireRate)
        {
            Instantiate(bulletPrefab, enemyGunEnd.position, Quaternion.identity);
            timeSinceLastAction = 0f;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemy health: " + health);  // Log health after taking damage
        if (health <= 0)
        {
            Die();  // Enemy dies if health reaches 0
        }
    }

    private void Die()
    {
        // Instantiate explosion effect
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Debug.Log("Explosion instantiated!");
        }

        // Add score to the player when the enemy dies
        ScoreManager.instance.AddScore(scoreValue);

        // Destroy the enemy object
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name);

        if (other.CompareTag("PlayerBullet"))
        {
            Debug.Log("Bullet hit enemy!");

            BulletController bulletController = other.GetComponent<BulletController>();
            if (bulletController != null)
            {
                Debug.Log("Damage applied to enemy, health before damage: " + health);
                TakeDamage(bulletController.damage);  // Apply damage from bulletController
                Destroy(other.gameObject);  // Destroy bullet after collision
            }
        }
    }
}
