using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Project.Source;

public class SlimeController : MonoBehaviour
{    
    [Header("Configuration")]
    public float moveSpeed = 10f;
    public float aggroRadius = 3f;
    public float followDistance = 2f;
    public PlayerMovement playerTarget;
    public float cooldown = 1f;
    public SlimeBulletController bulletPrefab;
    public float bulletSpread = 10f;
    public float projectileSpawnDistance = 0.5f;
    public float basicAttackDamage = 1f;

    [Header("Runtime")]
    EnemyController enemyTarget;
    public Vector2 movement = new Vector2(0, 0);
    public bool isDead = false;
    float timer = 0;
    private Rigidbody2D myRigidbody;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        playerTarget = FindObjectOfType<PlayerMovement>();
    }

    void FixedUpdate()
    {
        if(isDead) return;
        timer -= Time.deltaTime;
        UpdateEnemyTarget(GetNearbyEntityColliders(aggroRadius));
        UpdateMovementSpeed();
        
        myRigidbody.AddForce(movement * moveSpeed * myRigidbody.mass * myRigidbody.drag);
    }

    private Collider2D[] GetNearbyEntityColliders(float radius)
    {
        return Physics2D.OverlapCircleAll(transform.position, aggroRadius, GameSettings.Instance.entityWorldLayerMask);
    }

    void UpdateMovementSpeed()
    {
        var offset = playerTarget.transform.position - transform.position;
        var direction = offset.normalized;
        movement = direction;
        
        if(GetDistanceToPlayerTarget() < followDistance)
            movement = Vector2.zero;
        if (GetDistanceToEnemyTarget() < aggroRadius)
        {
            if(timer < 0) 
            {
                BulletAttack();
                timer = cooldown;
            }
        }
    }

    public float GetDistanceToPlayerTarget()
    {
        if (!playerTarget)
        {
            return float.PositiveInfinity;
        }

        return (playerTarget.transform.position - transform.position).magnitude;
    }

    public float GetDistanceToEnemyTarget()
    {
        if (!enemyTarget)
        {
            return float.PositiveInfinity;
        }

        return (enemyTarget.transform.position - transform.position).magnitude;
    }

    private void UpdateEnemyTarget(Collider2D[] colliders)
    {
        if (enemyTarget)
        {
            if (GetDistanceToEnemyTarget() > aggroRadius)
            {
                enemyTarget = null;
            }
        }
        else
        {
            foreach (var collider in colliders)
            {
                Debug.Log("there is a collider!" + collider.attachedRigidbody.name);
                if (collider.attachedRigidbody && collider.attachedRigidbody.TryGetComponent(out EnemyController enemy))
                {
                    Debug.Log("enemy set!");
                    enemyTarget = enemy;
                }
            }
        }
        Debug.Log("does this output?");
    }

    public void BulletAttack()
    {   
        Debug.Log("attacking!");
        if(isDead) return;
        //Debug.Log("Shooting!");
        var angleDifference = UnityEngine.Random.Range(-bulletSpread,bulletSpread);
        var offset = enemyTarget.transform.position - transform.position;
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0,0, angle + angleDifference);
        var bullet = Instantiate(bulletPrefab, transform.position + offset.normalized * projectileSpawnDistance, rotation);
    }

    public float getBasicAttackDamage()
    {
        return basicAttackDamage;
    }
}
