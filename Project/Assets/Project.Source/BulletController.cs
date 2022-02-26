using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 1f;
    Rigidbody2D myRigidbody;
    public GameObject playerObject;
    PlayerMovement player;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = playerObject.GetComponent<PlayerMovement>();
        myRigidbody.velocity = (player.rotationTransform.right * bulletSpeed);
    }

    void FixedUpdate()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.TryGetComponent(out EnemyController enemyController))
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.TryGetComponent(out EnemyController enemyController)) 
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}
