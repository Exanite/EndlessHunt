using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    CapsuleCollider2D sightCollider;
    BoxCollider2D bodyCollider;
    Rigidbody2D myRigidBody;
    void Start()
    {
        sightCollider = GetComponent<CapsuleCollider2D>();
        bodyCollider = GetComponent<BoxCollider2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        Vector2 movement = new Vector2(moveSpeed, 0);
        if(other.tag == "Player")
            myRigidBody.AddForce(movement);
    }
    void Update()
    {
        
    }
}
