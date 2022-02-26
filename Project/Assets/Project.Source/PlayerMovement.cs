using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, PlayerInput.IMovementActions, PlayerInput.IAttackActions
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float dashSpeed = 10f;
    Vector2 movementInput;
    PlayerInput playerInput;

    Collider2D myCollider;
    Rigidbody2D myRigidbody;

    void Awake() 
    {
        playerInput = new PlayerInput();
        playerInput.Movement.SetCallbacks(this);
        playerInput.Attack.SetCallbacks(this);
    }
    void Start() 
    {
        myCollider = GetComponent<Collider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable() 
    {
        playerInput.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        //Debug.Log("moving!");
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        Debug.Log("dashing!");
        myRigidbody.AddForce(movementInput * dashSpeed, ForceMode2D.Impulse);
    }

    public void OnBasicAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Attacking");
    }

    public void OnAOEAttack(InputAction.CallbackContext context)
    {
        Debug.Log("AOE Attack!");
    }

    void FixedUpdate() 
    {
        myRigidbody.AddForce(movementInput * moveSpeed);
    }
}
