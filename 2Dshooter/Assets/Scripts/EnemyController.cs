using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 0.6f;

    public Transform playerTransform;

    public float fireRate = 1f;
    private float timeSinceLastAction = 0f;

    public GameObject bulletPrefab;
    public Transform enemyGunEnd;

    public GameObject expolisionEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // transform.Translate(Vector2.down * speed * Time.deltaTime);
        GameObject playerGameObject = GameObject.Find("Player");
        playerTransform = playerGameObject.transform;

       
    }

    // Update is called once per frame
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Instantiate(expolisionEffectPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Player")
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
            timeSinceLastAction = 0;
        }

    }
}
