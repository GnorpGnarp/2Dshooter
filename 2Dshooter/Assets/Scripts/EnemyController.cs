using UnityEngine;
using UnityEngine.Audio;
public class EnemyController : MonoBehaviour
{
    public string difficulty = "Easy";  // Difficulty of the enemy
    public float speed = 0.6f;
    public float fireRate = 1f;
    public float health = 1f;

    private static int deathCount = 0;  // Keep track of how many enemies are dying at once
    public Transform playerTransform;
    public GameObject bulletPrefab;
    public Transform enemyGunEnd;
    public GameObject explosionEffectPrefab;
    public AudioClip explosionSound; // The explosion sound clip
    private AudioSource audioSource;  // The AudioSource component
    private Animator animator;  // Animator for death animation
    public AudioMixerGroup masterVolumeGroup;
    private float timeSinceLastAction = 0f;

    // Define score based on difficulty
    private int scoreValue;

    private bool isDead = false;  // Flag to track if the enemy is dead

    void Start()
    {
        // Initialize AudioSource (only once)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign the AudioSource to the AudioMixerGroup
        if (masterVolumeGroup != null)
        {
            audioSource.outputAudioMixerGroup = masterVolumeGroup;
        }
        SetDifficulty(difficulty);

        // Initialize AudioSource (only once)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Find player transform
        GameObject playerGameObject = GameObject.Find("Player");
        if (playerGameObject != null)
        {
            playerTransform = playerGameObject.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found! Please ensure the 'Player' GameObject exists in the scene.");
        }

        // Get the Animator component for death animation (if any)
        animator = GetComponent<Animator>();

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
        if (isDead) return;  // Prevent any movement or actions if the enemy is dead

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
        if (isDead) return;  // Prevent shooting if the enemy is dead

        timeSinceLastAction += Time.deltaTime;

        if (timeSinceLastAction >= fireRate)
        {
            Instantiate(bulletPrefab, enemyGunEnd.position, Quaternion.identity);
            timeSinceLastAction = 0f;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;  // If the enemy is already dead, do not process any more damage

        health -= damage;
        Debug.Log("Enemy health: " + health);  // Log health after taking damage
        if (health <= 0)
        {
            Die();  // Enemy dies if health reaches 0
        }
    }


    private void Die()
    {
        if (isDead) return;  // Prevent calling Die() multiple times

        isDead = true;  // Set the enemy to dead to stop it from shooting or moving

        // Increase the death count (used to adjust volume for multiple deaths)
        deathCount++;

        // Disable collider to prevent blocking bullets
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Adjust the explosion sound volume dynamically based on the death count
        float volume = 0.6f;  // Default volume

        // If more than 1 enemy dies at once, reduce the volume
        if (deathCount > 1)
        {
            volume = 0.3f;  // Reduce volume when more than one enemy dies
        }

        // Play the explosion sound with adjusted volume
        if (explosionSound != null)
        {
            audioSource.volume = volume;  // Adjust volume dynamically
            audioSource.PlayOneShot(explosionSound);
        }

        // Instantiate explosion effect
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Add score to the player
        ScoreManager.instance.AddScore(scoreValue);

        // If there is an animator, play the death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");  // Assuming you have a "Die" trigger in your Animator
        }

        // Disable the enemy's sprite renderer immediately
        GetComponent<SpriteRenderer>().enabled = false;

        // Destroy the enemy object after some delay to allow the explosion sound and animation to play
        Destroy(gameObject, explosionSound.length);  // Ensure object is destroyed after the sound plays

        // After the enemy dies, decrease the death count after a delay (or when explosion finishes)
        deathCount--;  // Reset the count for the next enemy death

        // Remove this enemy from the spawner's list
        FindObjectOfType<EnemySpawner>().RemoveDeadEnemies();
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
