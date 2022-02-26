using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    float bulletSpeed = 1f;
    Rigidbody2D myRigidbody;
    public GameObject myObject;
    EnemyRangedController myController;
    float bulletDamage = 1f;
    Vector2 targetPosition;
    float bulletTime = 3f;
    float timer = 0;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myController = myObject.GetComponent<EnemyRangedController>();
        bulletSpeed = myController.bulletSpeed;
        bulletTime = myController.bulletTime;
        bulletDamage = myController.basicAttackDamage;
        timer = bulletTime;
        //targetPosition = myController.targetLocation;
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
        //Destroy(gameObject);
    }
}
