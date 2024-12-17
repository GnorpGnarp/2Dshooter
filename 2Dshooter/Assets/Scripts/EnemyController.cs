using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public string difficulty = "Easy";
    public float speed = 0.6f;
    public float fireRate = 1f;
    public float health = 1f;

    public Transform playerTransform;
    public GameObject bulletPrefab;
    public Transform enemyGunEnd;
    public GameObject expolisionEffectPrefab;

    private float timeSinceLastAction = 0f;

    void Start()
    {
        SetDifficulty(difficulty);

        GameObject playerGameObject = GameObject.Find("Player");

        if (playerGameObject != null)
        {
            playerTransform = playerGameObject.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found! Please ensure the 'Player' GameObject exists in the scene.");
        }
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

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y > -2)
            Shoot();

        if (transform.position.y < -5.5f)
        {
            GameManager.playerController.HittedByBullet();
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
        if (health <= 0)
        {
            Die();  // Handle enemy death (e.g., trigger explosion or removal)
        }
    }

    private void Die()
    {
        // Add death logic here (e.g., instantiate explosion effect, remove enemy)
        Destroy(gameObject);  // For now, we simply destroy the enemy object
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name);

        if (other.CompareTag("PlayerBullet"))
        {
            PlayerBullet playerBullet = other.GetComponent<PlayerBullet>();
            if (playerBullet != null)
       
            Destroy(other.gameObject);  // Destroy the player bullet
        }
    }
}
