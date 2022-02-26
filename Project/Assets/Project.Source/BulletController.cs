using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 1f;
    Rigidbody2D myRigidbody;
    public GameObject playerObject;
    PlayerMovement player;
    float xSpeed;
    Vector2 movement;
    public Transform attackPivot;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = playerObject.GetComponent<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
        //transform.localScale = new Vector2(player.transform.localScale.x, 1f);


        var offset = transform.position - attackPivot.transform.position;
        var direction = offset.normalized;
        movement = direction;
        // this
    }

    void FixedUpdate()
    {
        myRigidbody.velocity = (movement * bulletSpeed);
        //myRigidbody.AddForce(movement * bulletSpeed * myRigidbody.mass * myRigidbody.drag);
        //myRigidbody.velocity = new Vector2(xSpeed, 0f);
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
