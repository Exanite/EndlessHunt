using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBulletController : MonoBehaviour
{
    public float bulletSpeed = 1f;
    Rigidbody2D myRigidbody;
    public GameObject playerObject;
    SlimeController slime;
    float bulletDamage = 1f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        slime = playerObject.GetComponent<SlimeController>();
        bulletDamage = slime.getBasicAttackDamage();
    }

    void FixedUpdate()
    {
        myRigidbody.velocity = transform.right * bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out PlayerMovement player)) return;
        if(other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out SlimeController slime)) return;
        if(other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out EnemyController enemyController))
        {
            enemyController.takeDamage(bulletDamage);
        }
        if(other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out EnemyRangedController enemyRangedController))
        {
            enemyRangedController.takeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
