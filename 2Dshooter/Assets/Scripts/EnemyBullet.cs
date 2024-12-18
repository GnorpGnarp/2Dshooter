using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public float speed = 3f;
    public Rigidbody2D rb;

    // Direction of bullet movement
    private Vector3 direction;

    public GameObject bulletExplosionEffect;
    public float damage = 1f; // Damage value for enemy bullet

    // Y position threshold where the bullet will be destroyed
    public float destroyYValue = -6f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector3.down; // Enemy bullets move downwards
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        if (transform.position.y < destroyYValue)
        {
            Destroy(gameObject);
        }
    }

    // Change this to OnTriggerEnter2D instead of OnCollisionEnter2D
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // If the bullet hits the player
        if (collider.gameObject.CompareTag("Player"))
        {
            // Get the PlayerController and apply damage
            PlayerController playerController = collider.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage); // Apply damage to the player
            }

            // Destroy the bullet after the explosion effect
            Destroy(gameObject);
        }

        // If the bullet hits another bullet (explosion effect)
        if (collider.gameObject.CompareTag("Bullet"))
        {
            Instantiate(bulletExplosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
