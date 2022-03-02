using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBulletController : MonoBehaviour
{
    public float bulletSpeed = 1f;
    Rigidbody2D myRigidbody;
    public GameObject slimeObject;
    SlimeController slime;
    float bulletDamage = 1f;
    float timer = 0;
    float bulletExistence = .5f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        //slime = gameObject.GetComponent<SlimeController>();
        slime = slimeObject.GetComponent<SlimeController>();
        bulletDamage = slime.getBasicAttackDamage();
        bulletSpeed = slime.bulletSpeed;
        bulletExistence = slime.bulletExistence;
        timer = bulletExistence;
    }

    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
            Destroy(gameObject);
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
        Destroy(gameObject);
    }
}
