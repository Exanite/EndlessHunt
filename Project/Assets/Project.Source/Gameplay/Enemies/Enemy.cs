using System;
using System.Collections;
using Project.Source;
using Project.Source.Gameplay;
using Project.Source.Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [Header("Dependencies")]
    public GameObject projectileSpawnEffectPrefab;
    public Projectile projectilePrefab;
    public ParticleSystem deathParticleSystem;
    public Transform attackPoint;
    public SpriteRenderer outOfViewSprite;
    public UnitOffensiveStats OffensiveStats;

    [Header("Configuration")]
    public float health = 10;
    public float moveSpeed = 10f;

    public float aggroRadius = 5f;
    public float deaggroRadius = 10f;
    public float stopDistance = 2f;

    public float projectileSpawnDistance = 1f;
    public float projectileSpawnEffectDistance = 1f;

    public float bulletSpread = 10f;
    public float cooldown = 3f;

    public float onHitDeaggroTime = 5f;
    public float onHitEnrageAmount = 0.1f;

    [Header("Runtime")]
    public Player target;
    public Vector2 movementDirection;
    public bool isDead;

    public bool hasDirectSightOfTarget;

    private float attackTimer;
    private float deaggroTimer;
    
    private Rigidbody2D myRigidbody;
    private PathSolver pathSolver;
    private Path path;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        pathSolver = Pathfinder.Instance.GetSolver(transform.position);
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        deaggroTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;

        UpdateTarget();
        UpdateMovementSpeed();

        myRigidbody.AddForce(movementDirection * moveSpeed * myRigidbody.mass * myRigidbody.drag);
    }

    private void OnDrawGizmos()
    {
        if (target)
        {
            Gizmos.color = hasDirectSightOfTarget ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position + Vector3.back, target.transform.position + Vector3.back);

            if (!hasDirectSightOfTarget && path != null)
            {
                Gizmos.color = Color.cyan;
                path.DrawWithGizmos();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }

        deaggroTimer = onHitDeaggroTime;
        movementDirection *= 1 + onHitEnrageAmount;
        cooldown /= 1 + onHitEnrageAmount;
        target = PlayerManager.Instance.GetClosestPlayer(transform.position);

        health -= damage;
        if (health <= 0)
        {
            isDead = true;
            myRigidbody.mass = myRigidbody.mass * 100;
            myRigidbody.velocity = new Vector2(0, 0);
            deathParticleSystem.Play();
            StartCoroutine(OnDeath());
        }
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }

    public void BulletAttack()
    {
        if (isDead)
        {
            return;
        }

        var offset = target.transform.position - transform.position;
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        angle += Random.Range(-bulletSpread, bulletSpread);
        var rotation = Quaternion.Euler(0, 0, angle);

        var projectile = Instantiate(projectilePrefab, transform.position + offset.normalized * projectileSpawnDistance, rotation);
        projectile.owner = OffensiveStats;

        if (projectileSpawnEffectPrefab)
        {
            Instantiate(projectileSpawnEffectPrefab, transform.position + offset.normalized * projectileSpawnEffectDistance, rotation);
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

    private void UpdateTarget()
    {
        if (target)
        {
            hasDirectSightOfTarget = !Physics2D.Linecast(transform.position, 
                target.transform.position,
                GameSettings.Instance.NonWalkableLayerMask).collider;

            if (!hasDirectSightOfTarget)
            {
                path = pathSolver.FindPath(transform.position, target.transform.position);
            }
            
            if (GetDistanceToTarget() > deaggroRadius && deaggroTimer < 0)
            {
                target = null;
            }
        }
        else
        {
            var player = PlayerManager.Instance.GetClosestPlayer(transform.position);
            var playerDistance = (player.transform.position - transform.position).magnitude;

            if (playerDistance < aggroRadius)
            {
                target = player;
            }
        }
    }

    private void UpdateMovementSpeed()
    {
        if (!target)
        {
            movementDirection = Vector2.zero;

            return;
        }
        
        if (hasDirectSightOfTarget || path == null)
        {
            var offset = target.transform.position - transform.position;
            movementDirection = offset.normalized;
        }
        else if (path.Waypoints.Count > 0)
        {
            var offset = path.Waypoints[0] - transform.position;
            movementDirection = offset.normalized;
        }
        
        if (GetDistanceToTarget() < stopDistance)
        {
            movementDirection = Vector2.zero;
            if (attackTimer < 0)
            {
                //Debug.Log("attack!");
                BulletAttack();
                attackTimer = cooldown;
            }
        }
    }

    private IEnumerator OnDeath()
    {
        yield return new WaitForSeconds(deathParticleSystem.main.duration);

        Despawn();
    }
}