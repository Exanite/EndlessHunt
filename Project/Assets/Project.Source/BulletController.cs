using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 1f;
    Rigidbody2D myRigidbody;
    public GameObject playerObject;
    PlayerMovement player;
    float bulletDamage = 1f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = playerObject.GetComponent<PlayerMovement>();
        bulletDamage = player.getBasicAttackDamage();
    }

    void FixedUpdate()
    {
        myRigidbody.velocity = transform.right * bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.TryGetComponent(out EnemyController enemyController))
        {
            enemyController.takeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.TryGetComponent(out EnemyController enemyController)) 
        {
            enemyController.takeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
