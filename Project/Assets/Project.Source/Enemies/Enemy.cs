using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Dependencies")]
    public GameObject meleeAnim;
    public EnemyBullet bulletPrefab;
    public ParticleSystem deathParticleSystem;
    public Transform attackPoint;
    public SpriteRenderer outOfViewSprite;

    [Header("Configuration")]
    public float moveSpeed = 10f;
    public float aggroRadius = 5f;
    public float deaggroRadius = 10f;
    public float stopDistance = 2f;
    public float health = 10;
    public float attackDamage = 1f;
    public float bulletSpread = 10f;
    public bool isMelee;
    public float cooldown = 3f;
    public float bulletSpeed = 1f;
    public float bulletTime = 3f;
    public float projectileSpawnDistance = 1f;
    public float meleeSpawnDistance = 1f;
    public float onHitDeaggroTime = 5f;
    public float onHitEnrageAmount = 1.1f;

    [Header("Runtime")]
    public Player target;
    public Vector2 movement = new Vector2(0, 0);
    public bool isDead;

    private float timer;
    private float deaggroTimer;
    private Rigidbody2D myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        deaggroTimer -= Time.deltaTime;
        timer -= Time.deltaTime;
        UpdateTarget();
        UpdateMovementSpeed();

        myRigidbody.AddForce(movement * moveSpeed * myRigidbody.mass * myRigidbody.drag);
    }

    private void UpdateTarget()
    {
        if (target)
        {
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
            movement = Vector2.zero;

            return;
        }

        var offset = target.transform.position - transform.position;
        var direction = offset.normalized;
        movement = direction;

        if (GetDistanceToTarget() < stopDistance)
        {
            movement = Vector2.zero;
            if (timer < 0)
            {
                //Debug.Log("attack!");
                BulletAttack();
                timer = cooldown;
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
        movement *= onHitEnrageAmount;
        cooldown /= onHitEnrageAmount;
        target = PlayerManager.Instance.GetClosestPlayer(transform.position);

        health -= damage;
        if (health <= 0)
        {
            isDead = true;
            myRigidbody.mass = myRigidbody.mass * 100;
            myRigidbody.velocity = new Vector2(0, 0);
            deathParticleSystem.Play();
            Invoke("OnDeath", deathParticleSystem.main.duration);
        }
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }

    public void BulletAttack()
    {
        if (isDead)
        {
            return;
        }

        //Debug.Log("Shooting!");
        var angleDifference = Random.Range(-bulletSpread, bulletSpread);
        var offset = target.transform.position - transform.position;
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0, 0, angle + angleDifference);
        var bullet = Instantiate(bulletPrefab, transform.position + offset.normalized * projectileSpawnDistance, rotation);
        bullet.owner = this;
        if (isMelee)
        {
            Instantiate(meleeAnim, transform.position + offset.normalized * meleeSpawnDistance, rotation);
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