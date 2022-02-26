using System;
using Project.Source;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Configuration")]
    public float moveSpeed = 10f;
    public float aggroRadius = 5f;
    public float deaggroRadius = 10f;
    public float stopDistance = 2f;
    public float health = 10;
    
    [Header("Runtime")]
    public PlayerMovement target;
    public Vector2 movement = new Vector2(0, 0);
    public Transform attackPoint;
    public float basicAttackDamage = 1f;
    public bool isDead;
    
    private Rigidbody2D myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        UpdateTarget(GetNearbyEntityColliders(aggroRadius));
        UpdateMovementSpeed();
        
        myRigidbody.AddForce(movement * moveSpeed * myRigidbody.mass * myRigidbody.drag);
    }

    private Collider2D[] GetNearbyEntityColliders(float radius)
    {
        return Physics2D.OverlapCircleAll(transform.position, aggroRadius, GameSettings.Instance.entityWorldLayerMask);
    }

    private void UpdateTarget(Collider2D[] colliders)
    {
        if (target)
        {
            if (GetDistanceToTarget() > deaggroRadius)
            {
                target = null;
            }
        }
        else
        {
            foreach (var collider in colliders)
            {
                if (collider.attachedRigidbody && collider.attachedRigidbody.TryGetComponent(out PlayerMovement player))
                {
                    target = player;
                }
            }
        }
    }

    private void UpdateMovementSpeed()
    {
        if (!target)
        {
            movement = Vector2.zero;
            BasicAttack();
            return;
        }

        var offset = target.transform.position - transform.position;
        var direction = offset.normalized;
        movement = direction;
        
        if (GetDistanceToTarget() < stopDistance)
        {
            movement = Vector2.zero;
            BasicAttack();
        }
    }

    public void takeDamage(float damageTaken)
    {
        health -= damageTaken;
        if(health <= 0)
        {
            //Debug.Log("im dead :(");
            isDead = true;
        }
        //else
            //Debug.Log("Damage taken! health: " + health);
    }

    public void BasicAttack()
    {   
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach(Collider2D collider in colliders) 
        {
            if(collider.TryGetComponent(out PlayerMovement player))
            {
                player.takeDamage(basicAttackDamage);
            }
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