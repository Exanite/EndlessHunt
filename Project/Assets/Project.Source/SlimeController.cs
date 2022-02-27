using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{    
    public float moveSpeed = 10f;
    public float followDistance = 2f;
    public PlayerMovement target;
    public Vector2 movement = new Vector2(0, 0);
    public Transform attackPoint;
    public bool isDead = false;
    float timer = 0;
    private Rigidbody2D myRigidbody;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        UpdateMovementSpeed();
        
        myRigidbody.AddForce(movement * moveSpeed * myRigidbody.mass * myRigidbody.drag);
    }

    void UpdateMovementSpeed()
    {
        var offset = target.transform.position - transform.position;
        var direction = offset.normalized;
        movement = direction;
        
        if (GetDistanceToTarget() < followDistance)
        {
            movement = Vector2.zero;
        }
    }

    public float GetDistanceToTarget()
    {
        if (!target)
        {
            return float.PositiveInfinity;
        }

        return (target.transform.position - transform.position).magnitude;
    }
}
