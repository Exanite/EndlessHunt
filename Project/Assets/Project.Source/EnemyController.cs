using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    Vector2 movement = new Vector2(0,0);
    BoxCollider2D sightCollider;
    CapsuleCollider2D bodyCollider;
    Rigidbody2D myRigidbody;
    void Start()
    {
        sightCollider = GetComponent<BoxCollider2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log(other);
        Debug.Log("Person here!!");
        
        if(other.TryGetComponent(out PlayerMovement playerMovement)) 
        {
            Vector2 playerPosition = other.transform.position;
            if(myRigidbody.transform.position.x - playerPosition.x > 0) {
                movement = new Vector2(-moveSpeed, 0);
                myRigidbody.transform.localScale.Set(-1,1,1);
            }
            else {
                movement = new Vector2(moveSpeed, 0);
                myRigidbody.transform.localScale.Set(1,1,1);
            }
            Debug.Log("Its a player!");
            myRigidbody.AddForce(movement);
        }
    }

    void FixedUpdate() 
    {
        myRigidbody.AddForce(movement);
    }
    void Update()
    {
        
    }
}
