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
    public AudioClip playerExplosionSound; // Explosion sound for the player
    private AudioSource audioSource;  // The AudioSource component
    private int extraBullets = 0;  // Number of extra bullets (multiplier)

    // Start is called before the first frame update
    void Start()
    {
        GameManager.playerController = this;
        anim = GetComponent<Animator>();
        UpdateHealthUI();

        // Initialize AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    private bool isDead = false;  // Track whether the player is dead

    void Update()
    {
        if (isDead) return;  // Skip the update if the player is dead

        PlayerMovement();

        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }

        if (hp <= 0)
        {
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
            // Log how many bullets will be instantiated
            Debug.Log("Shooting " + (extraBullets + 1) + " bullets");

            // Create a spread based on the number of extra bullets
            float spreadAngle = 20f;  // Angle of the V-shape spread
            if (extraBullets == 2)
            {
                // 3 bullets in a V shape
                Instantiate(bulletPrefab, gunEndPosition.position, Quaternion.Euler(0, 0, -spreadAngle));  // Left bullet
                Instantiate(bulletPrefab, gunEndPosition.position, Quaternion.identity);  // Center bullet
                Instantiate(bulletPrefab, gunEndPosition.position, Quaternion.Euler(0, 0, spreadAngle));  // Right bullet
            }
            else
            {
                // Just the normal bullet
                Instantiate(bulletPrefab, gunEndPosition.position, Quaternion.identity);
            }

            timeSinceLastAction = 0;
        }
    }


    public void IncreaseHealth()
    {
        if (hp < 3)  // Ensure health doesn’t go beyond the max
        {
            hp += 1;
            GameManager.uiManager.UpdateHpSprites(hp);  // Update hearts when health increases
            Debug.Log("Health increased: " + hp);
        }
    }

    public void ActivateBulletMultiplier()
    {
        extraBullets = 2;
        Debug.Log("Bullet multiplier activated");
        StartCoroutine(DeactivateBulletMultiplier());
    }

    private IEnumerator DeactivateBulletMultiplier()
    {
        yield return new WaitForSeconds(5f);
        extraBullets = 0;
        Debug.Log("Bullet multiplier deactivated, extraBullets: " + extraBullets);
    }


    public void HittedByBullet()
    {
        if (isInvincible) return;  // Prevent further damage if the player is in invincibility frame

        if (hp > 0)
        {
            hp -= 1;  // Decrease HP
            Debug.Log("Player Hit! HP: " + hp);
            GameManager.uiManager.UpdateHpSprites(hp);  // Update health UI after taking damage
        }

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
            UpdateHealthUI();  // Update UI with the new HP
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
        // If the player is already dead, stop the coroutine to avoid running it multiple times
        if (isDead) yield break;

        isDead = true;  // Set the player to dead, preventing this coroutine from running again

        Debug.Log("Player died and explosion should play.");

        // Disable the sprite renderer to hide the player during the explosion
        GetComponent<SpriteRenderer>().enabled = false; // Disable sprite immediately

        // Ensure audioSource is unmuted and sound plays immediately
        if (audioSource != null)
        {
            audioSource.mute = false;  // Ensure it is not muted
            if (playerExplosionSound != null)
            {
                Debug.Log("Playing player explosion sound");
                audioSource.PlayOneShot(playerExplosionSound);  // Play sound immediately
            }
        }

        // Instantiate the explosion effect (only once)
        if (funnyExplosionPrefab != null)
        {
            Instantiate(funnyExplosionPrefab, transform.position, Quaternion.identity);
        }

        // Wait for the explosion sound and effect to finish
        yield return new WaitForSeconds(explosionDuration);  // Wait for explosion to finish

        // Finally, deactivate the player object after the explosion effect and sound finish
        gameObject.SetActive(false);  // Hide the player object

        // Wait just a bit more, if needed (in case you want to add some delay before game over scene)
        yield return new WaitForSeconds(0.1f);  // Optional small delay for smoother transition

        // Show the Game Over screen
        SceneManager.LoadScene("GameOverScene");
        GetComponent<Canvas>().gameObject.SetActive(true);  // Show game over UI
    }




    // Function to update the health UI (hearts) based on current HP
    private void UpdateHealthUI()
    {
        GameManager.uiManager.UpdateHpSprites(hp);
    }
}
