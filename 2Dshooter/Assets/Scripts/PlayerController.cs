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

        GameManager.uiManager.DisableHpSprite(hp);
        hp = hp - 1;
        Debug.Log("Player Hit! HP: " + hp);

        StartCoroutine(InvincibilityFrames());  // Start the invincibility frames after being hit
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
