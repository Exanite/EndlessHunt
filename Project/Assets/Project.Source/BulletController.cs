using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 1f;
    Rigidbody2D myRigidBody;
    public GameObject playerObject;
    PlayerMovement player;
    float xSpeed;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        player = playerObject.GetComponent<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
        //transform.localScale = new Vector2(player.transform.localScale.x, 1f);
    }

    void Update()
    {
        myRigidBody.velocity = new Vector2(xSpeed, 0f);
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
