using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public float speed = 3f;
    public Rigidbody2D rb;

    // Direction variable
    private Vector3 direction;

    public GameObject bulletExplosionEffect;

    // Y position threshold where the bullet will be destroyed (adjust for your screen size)
    public float destroyYValue = -6f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // If you want to track the player, leave this as is
        // playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        // direction = (playerTransform.position - transform.position).normalized;

        // For downward movement, we'll just set the direction to be straight down
        direction = Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the bullet downward with the specified speed
        transform.Translate(direction * speed * Time.deltaTime);

        // Destroy the bullet when it moves below the screen
        if (transform.position.y < destroyYValue)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision with the player
        if (collision.gameObject.tag == "Player")
        {
            GameManager.playerController.HittedByBullet();
            Destroy(gameObject);
        }

        // Handle collision with another bullet (creating explosion effect)
        if (collision.gameObject.tag == "Bullet")
        {
            Instantiate(bulletExplosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
