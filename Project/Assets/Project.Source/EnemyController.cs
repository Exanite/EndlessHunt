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
    Vector2 playerPosition;
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
            playerPosition = other.transform.position;
            float xDifference = myRigidbody.transform.position.x - playerPosition.x;
            float yDifference = myRigidbody.transform.position.y - playerPosition.y;
            if(Mathf.Abs(xDifference) > Mathf.Abs(yDifference)) 
            {
                if(xDifference > 0) {
                    movement = new Vector2(-moveSpeed, 0);
                    FlipObjectX();
                    //mySprite.flipX = true;
                }
                else {
                    movement = new Vector2(moveSpeed, 0);
                    FlipObjectX();
                    //mySprite.flipX = false;
                }
            }
            else 
            {
                if(yDifference > 0) {
                    movement = new Vector2(0, -moveSpeed);
                    FlipObjectY();
                    //mySprite.flipX = true;
                }
                else {
                    movement = new Vector2(0, moveSpeed);
                    FlipObjectY();
                    //mySprite.flipX = false;
                }
            }
            Debug.Log("Its a player!");
            myRigidbody.AddForce(movement);
        }
    }

    void FlipObjectX()
    {
        transform.localScale = new Vector2 (-myRigidbody.transform.localScale.x, 1f);
    }

    void FlipObjectY()
    {
        transform.localScale = new Vector2 (1f, -myRigidbody.transform.localScale.y);
    }

    void FixedUpdate() 
    {
        myRigidbody.AddForce(movement);
        
    }
    void Update()
    {
        
    }
}
