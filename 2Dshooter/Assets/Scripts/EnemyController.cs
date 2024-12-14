using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public string difficulty = "Easy";  // Make this public to show in the Inspector
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
        // Call SetDifficulty with the difficulty set in the inspector or by the spawner
        SetDifficulty(difficulty);

        GameObject playerGameObject = GameObject.Find("Player");
        playerTransform = playerGameObject.transform;
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
}
