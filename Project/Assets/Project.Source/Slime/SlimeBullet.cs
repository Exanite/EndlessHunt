using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBullet : MonoBehaviour
{
    public float bulletSpeed = 1f;
    Rigidbody2D myRigidbody;
    public GameObject slimeObject;
    Slime slime;
    float bulletDamage = 1f;
    float timer = 0;
    float bulletExistence = .5f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        slime = slimeObject.GetComponent<Slime>();
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
        if(other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Player player)) return;
        if(other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Slime slime)) return;
        if(other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Enemy enemy))
        {
            enemy.takeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
