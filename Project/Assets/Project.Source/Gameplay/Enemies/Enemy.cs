using System;
using System.Collections;
using Project.Source;
using Project.Source.Gameplay;
using Project.Source.Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public enum TargetStatus
{
    NoLineOfSight = 0,
    CanDirectlyShootAt = 1,
    CanDirectlyWalkTo = 2,
}

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

    public float pathfindingCooldown = 1f;
    public float pathfindingWaypointPopDistance = 1f;

    [Header("Runtime")]
    public Player target;
    public Vector2 movementDirection;
    public bool isDead;

    public TargetStatus TargetStatus;

    private float attackTimer;
    private float deaggroTimer;
    private float pathUpdateTimer;

    private Rigidbody2D myRigidbody;
    private PathSolver pathSolver;
    private Path path;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        pathSolver = Pathfinder.Instance.GetSolver(transform.position);
        path = pathSolver.Path;
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        deaggroTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
        pathUpdateTimer -= Time.deltaTime;

        UpdateTarget();
        UpdateMovementSpeed();

        myRigidbody.AddForce(movementDirection * moveSpeed * myRigidbody.mass * myRigidbody.drag);
    }

    private void OnDrawGizmos()
    {
        if (target)
        {
            Gizmos.color = TargetStatus switch
            {
                TargetStatus.NoLineOfSight => Color.red,
                TargetStatus.CanDirectlyShootAt => Color.yellow,
                TargetStatus.CanDirectlyWalkTo => Color.green,
                _ => throw new NotSupportedException($"{TargetStatus} is not supported"),
            };

            Gizmos.DrawLine(transform.position + Vector3.back, target.transform.position + Vector3.back);

            if (TargetStatus < TargetStatus.CanDirectlyWalkTo && path.IsValid)
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
            if (!Physics2D.Linecast(transform.position,
                    target.transform.position,
                    GameSettings.Instance.NonWalkableLayerMask)
                .collider)
            {
                TargetStatus = TargetStatus.CanDirectlyWalkTo;
            }
            else if (!Physics2D.Linecast(transform.position,
                    target.transform.position,
                    GameSettings.Instance.ProjectileBlockingLayerMask)
                .collider)
            {
                TargetStatus = TargetStatus.CanDirectlyShootAt;
            }
            else
            {
                TargetStatus = TargetStatus.NoLineOfSight;
            }

            if (TargetStatus < TargetStatus.CanDirectlyWalkTo && pathUpdateTimer < 0)
            {
                pathSolver.FindPath(transform.position, target.transform.position);
                pathUpdateTimer = pathfindingCooldown;
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

        if (TargetStatus == TargetStatus.CanDirectlyWalkTo || !path.IsValid)
        {
            var offset = target.transform.position - transform.position;
            movementDirection = offset.normalized;
            path.IsValid = false;
        }
        else if (path.HasNext() && path.Length < deaggroRadius)
        {
            var next = path.GetNext();

            var offset = next - transform.position;
            movementDirection = offset.normalized;

            Debug.Log(offset);
            
            if (offset.magnitude < pathfindingWaypointPopDistance)
            {
                path.Pop();
            }
        }

        if (GetDistanceToTarget() < stopDistance && TargetStatus >= TargetStatus.CanDirectlyShootAt)
        {
            movementDirection = Vector2.zero;
            if (attackTimer < 0)
            {
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