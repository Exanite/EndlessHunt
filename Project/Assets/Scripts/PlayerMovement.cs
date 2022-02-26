using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerSpeed = 4f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] GameObject bullet;
    bool isAlive = true;
    float myGravity;
    int coins = 0;
    Animator animator;
    Vector2 moveInput;
    Rigidbody2D rigidBody;
    BoxCollider2D myCollider;
    CapsuleCollider2D myFeet;
    SpriteRenderer mySprite;
  
    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myFeet = GetComponent<CapsuleCollider2D>();
        mySprite = GetComponent<SpriteRenderer>();
        myGravity = rigidBody.gravityScale;
    }
    void Update()
    {
        if(!isAlive) { return; }
        Run();
        OnClimb();
        FlipSprite();
        // bounce();
    }
    
    void OnMove(InputValue value)
    {
        if(!isAlive) { return; }
        moveInput = value.Get<Vector2>();
        //Debug.Log(moveInput);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * playerSpeed, rigidBody.velocity.y);
        rigidBody.velocity = playerVelocity;
        if(Mathf.Abs(moveInput.x) > 0)
            animator.SetBool("isRunning", true);
        else
            animator.SetBool("isRunning", false);
    }

    void OnJump(InputValue value)
    {
        if(!isAlive) { return; }
        if((!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))))
        {
            return;
        }
        else if(value.isPressed)
        {
            rigidBody.velocity += new Vector2 (0f, jumpSpeed);
        }
    }

    void OnClimb()
    {   
        if((!myCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))))
        {
            animator.SetBool("isClimbing", false);
            rigidBody.gravityScale = myGravity;
            return;
        }
        rigidBody.gravityScale = 0;
        rigidBody.velocity = new Vector2 (moveInput.x * playerSpeed, moveInput.y * climbSpeed);
        if(Mathf.Abs(moveInput.y) > 0)
            animator.SetBool("isClimbing", true);
        else
            animator.SetBool("isClimbing", false);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
            transform.localScale = new Vector2 (Mathf.Sign(rigidBody.velocity.x), 1f);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if((myCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards"))))
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        
    }
    void Die()
    {
        isAlive = false;
        rigidBody.velocity = new Vector2 (0f, 20f);
        mySprite.color = new Color(1, 0, 0);
        animator.SetTrigger("isDead");
        myCollider.isTrigger = true;
        myFeet.isTrigger = true;
        //Destroy(myCollider);
    }
    void OnFire(InputValue input)
    {
        if(!isAlive) {return;}
        Instantiate(bullet, bulletSpawn.position, transform.rotation);
    }
}
