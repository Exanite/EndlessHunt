using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public float bulletSpeed = 1f;
    Rigidbody2D myRigidbody;
    public GameObject myObject;
    EnemyRangedController myController;
    float bulletDamage = 1f;
    Vector2 targetPosition;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myController = myObject.GetComponent<EnemyRangedController>();
        bulletDamage = myController.basicAttackDamage;
        //targetPosition = myController.targetLocation;
    }

    void FixedUpdate()
    {
        var velocity = (targetPosition - new Vector2(transform.position.x, transform.position.y)).normalized * bulletSpeed;
        myRigidbody.velocity = velocity;
        Debug.Log("vector = " + velocity);
        //myRigidbody.velocity = new Vector2(10,10);;
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
