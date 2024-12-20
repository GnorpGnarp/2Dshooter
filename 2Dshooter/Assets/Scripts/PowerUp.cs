using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    HealthUp,
    BulletMultiplier
}

public class PowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;
    public float fallSpeed = 2f;  // Default fall speed, can be modified per power-up

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set Rigidbody2D to Kinematic, as we're controlling movement manually
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Manually move the power-up down by setting its position
        StartCoroutine(FallDown());
    }

    private IEnumerator FallDown()
    {
        // Continue moving the power-up downwards until it reaches the bottom of the screen
        while (transform.position.y > -4f)  // Example: stop when it reaches y = -4
        {
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
            yield return null;
        }

        // Destroy the power-up if it goes off-screen or reaches the bottom
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                // Handle power-up effect
                if (powerUpType == PowerUpType.HealthUp)
                {
                    player.IncreaseHealth();
                }
                else if (powerUpType == PowerUpType.BulletMultiplier)
                {
                    player.ActivateBulletMultiplier();
                }

                // Destroy the power-up object after collection
                Destroy(gameObject);
            }
        }
    }
}
