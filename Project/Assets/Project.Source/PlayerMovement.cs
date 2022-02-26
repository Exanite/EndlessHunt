using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, GameInput.IPlayerActions
{
    [SerializeField] float moveSpeed = 10f;
    Vector2 movementInput;
    GameInput inputClass;
    Collider2D myCollider;
    Rigidbody2D myRigidbody;

    void Awake() 
    {
        inputClass = new GameInput();
        inputClass.Player.SetCallbacks(this);
    }
    void Start() 
    {
        myCollider = GetComponent<Collider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        
    }

    void OnEnable()
    {
        inputClass.Enable();
    }

    void OnDisable() 
    {
        inputClass.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        //Debug.Log("moving!");
        movementInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate() 
    {
        myRigidbody.AddForce(movementInput * moveSpeed);
    }
}
