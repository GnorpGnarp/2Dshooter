using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int hp = 3; // Player health points

    public float moveSpeed = 2f;

    public Transform minXValue;
    public Transform maxXValue;

    public GameObject bulletPrefab;
    public Transform gunEndPosition;

    public float fireRate = 0.2f;
    private float timeSinceLastAction = 0f;
    public Animator anim;

    private bool isInvincible = false;  // Flag to check if the player is invincible after being hit
    public float invincibilityDuration = 1f; // Duration of invincibility after getting hit

    public GameObject funnyExplosionPrefab; // The explosion effect when the player dies
    public float explosionDuration = 2f; // Duration of the explosion effect before showing the game over screen

    // Start is called before the first frame update
    void Start()
    {
        GameManager.playerController = this;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }

        if (hp <= 0)
        {
            // If player is out of health, trigger the death sequence
            StartCoroutine(PlayerDeathSequence());
        }
    }

    void PlayerMovement()
    {
        float horizontalInputValue = Input.GetAxis("Horizontal");
        Vector2 movementVector = new Vector2(horizontalInputValue, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movementVector);

        if (transform.position.x > maxXValue.position.x)
        {
            transform.position = new Vector2(maxXValue.position.x, transform.position.y);
        }

        if (transform.position.x < minXValue.position.x)
        {
            transform.position = new Vector2(minXValue.position.x, transform.position.y);
        }
        anim.SetFloat("Horizontal", horizontalInputValue);
    }

    void Shoot()
    {
        timeSinceLastAction += Time.deltaTime;

        if (timeSinceLastAction >= fireRate)
        {
            Instantiate(bulletPrefab, gunEndPosition.position, Quaternion.identity);
            timeSinceLastAction = 0;
        }
    }

    public void HittedByBullet()
    {
        if (isInvincible) return;  // Prevent further damage if the player is in invincibility frame

        // Only disable HP sprite if the player has health (hp > 0)
        if (hp > 0)
        {
            GameManager.uiManager.DisableHpSprite(hp);  // Disable sprite based on current HP
        }

        hp = hp - 1;  // Decrease HP
        Debug.Log("Player Hit! HP: " + hp);

        // If hp is less than or equal to 0, trigger death sequence
        if (hp <= 0)
        {
            StartCoroutine(PlayerDeathSequence());
        }
        else
        {
            StartCoroutine(InvincibilityFrames());  // Start the invincibility frames after being hit
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;  // Prevent further damage if the player is in invincibility frame

        hp -= (int)damage;  // Decrease HP by the damage value

        if (hp > 0)
        {
            GameManager.uiManager.DisableHpSprite(hp);  // Update UI with the new HP
            StartCoroutine(InvincibilityFrames());  // Start the invincibility frames after being hit
        }
        else
        {
            StartCoroutine(PlayerDeathSequence());  // Trigger player death if HP reaches zero
        }
    }

    private IEnumerator InvincibilityFrames()
    {
        isInvincible = true;

        // Flash player or add visual effect for invincibility (optional)
        // Example: Add sprite flickering or change player color briefly

        yield return new WaitForSeconds(invincibilityDuration);  // Wait for the invincibility duration

        isInvincible = false;  // Reset invincibility after duration ends
    }

    // Coroutine for handling the death sequence and playing the funny explosion
    private IEnumerator PlayerDeathSequence()
    {
        // Instantiate the funny explosion at the player's position
        if (funnyExplosionPrefab != null)
        {
            Instantiate(funnyExplosionPrefab, transform.position, Quaternion.identity);
        }

        // Disable the player object so it doesn't interact with the game anymore
        gameObject.SetActive(false);

        // Wait for the explosion to finish before showing the Game Over screen
        yield return new WaitForSeconds(explosionDuration);

        // Now show the Game Over screen
        SceneManager.LoadScene("GameOverScene");
        GetComponent<Canvas>().gameObject.SetActive(true);
    }
}
