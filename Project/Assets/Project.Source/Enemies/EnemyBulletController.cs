using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public EnemyController myController;
    
    float bulletSpeed = 1f;
    Rigidbody2D myRigidbody;
    float bulletDamage = 1f;
    float bulletTime = 3f;
    float timer = 0;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        bulletSpeed = myController.bulletSpeed;
        bulletTime = myController.bulletTime;
        bulletDamage = myController.attackDamage;
        timer = bulletTime;
    }

    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if(timer < 0) 
        {
            Destroy(gameObject);
            timer = bulletTime;
        }
        
        myRigidbody.velocity = transform.right * bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out EnemyController enemyController)) return;
        if(other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out EnemyRangedController enemyRangedController)) return;
        if(other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out PlayerMovement player))
        {
            player.takeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
