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
    SpriteRenderer mySprite;

    PlayerMovement target;
    void Start()
    {
        sightCollider = GetComponent<BoxCollider2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log(other);
        Debug.Log("Person here!!");
        
        if(other.TryGetComponent(out PlayerMovement playerMovement)) 
        {
            target = playerMovement;
            
            Debug.Log("Its a player!");
        }
    }

    void FixedUpdate() 
    {
        myRigidbody.AddForce(movement * moveSpeed);
        
    }
    void Update()
    {
        if(!target)
            return;
        Vector3 offset = target.transform.position - transform.position;
        Vector3 direction = offset.normalized;
        movement = direction;
        
        if(offset.magnitude > 8) {
            target = null;
            movement = Vector2.zero;
        }
    }
}
