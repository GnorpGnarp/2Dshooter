using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Rigidbody2D rb;
    public float damage = 1f; // Damage value for player bullet

    public float destroyYValue = 6f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        DestroyAfterLeftScreen();
    }

    void DestroyAfterLeftScreen()
    {
        if (transform.position.y > destroyYValue)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // If the bullet hits an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(damage);  // Apply damage to the enemy
            }
        }

        // Destroy the bullet regardless of the collision
        Destroy(gameObject);
    }
}
