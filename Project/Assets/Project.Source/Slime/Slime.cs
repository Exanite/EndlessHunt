using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Project.Source;

public class Slime : MonoBehaviour
{    
    [Header("Configuration")]
    public float moveSpeed = 10f;
    public float aggroRadius = 3f;
    public float followDistance = 2f;
    public float lostDistance = 10f;
    public Player playerTarget;
    public float cooldown = 1f;
    public SlimeBullet bulletPrefab;
    public float bulletSpread = 10f;
    public float projectileSpawnDistance = 0.5f;
    public float basicAttackDamage = 1f;
    public float bulletExistence = .5f;
    public float bulletSpeed = 2f;

    [Header("Runtime")]
    Enemy enemyTarget;
    public Vector2 movement = new Vector2(0, 0);
    public bool isDead = false;
    float timer = 0;
    private Rigidbody2D myRigidbody;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        playerTarget = FindObjectOfType<Player>();
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
        return Physics2D.OverlapCircleAll(transform.position, aggroRadius);
    }

    void UpdateMovementSpeed()
    {
        var offset = playerTarget.transform.position - transform.position;
        var direction = offset.normalized;
        movement = direction;
        
        if(GetDistanceToPlayerTarget() < followDistance)
            movement = Vector2.zero;
        if(GetDistanceToPlayerTarget() > lostDistance)
            TeleportToPlayer();
        if (GetDistanceToEnemyTarget() < aggroRadius)
        {
            if(timer < 0) 
            {
                BulletAttack();
                timer = cooldown;
            }
        }
    }

    private void TeleportToPlayer()
    {
        if(!playerTarget) return;
            transform.position = playerTarget.transform.position;
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
                //Debug.Log("there is a collider!" + collider.attachedRigidbody.name);
                if (collider.attachedRigidbody && collider.attachedRigidbody.TryGetComponent(out Enemy enemy))
                {
                    //Debug.Log("enemy set!");
                    if(enemy.health > 0)
                        enemyTarget = enemy;
                }
            }
        }
    }

    public void BulletAttack()
    {   
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
