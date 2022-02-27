using System;
using Project.Source;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    [Header("Configuration")]
    public float moveSpeed = 10f;
    public float aggroRadius = 5f;
    public float deaggroRadius = 10f;
    public float stopDistance = 2f;
    public float health = 10;
    public float attackDamage = 1f;
    public float bulletSpread = 10f;
    //public float meleeTimer = 0.2f;
    public bool isMelee = false;
    public GameObject meleeAnim;
    [FormerlySerializedAs("bullet")]
    public EnemyBulletController bulletPrefab;
    public float cooldown = 3f;
    public float bulletSpeed = 1f;
    public float bulletTime = 3f;
    public float projectileSpawnDistance = 1f;
    public float meleeSpawnDistance = 1f;
    public ParticleSystem deathParticleSystem;
    
    [Header("Runtime")]

    public PlayerMovement target;
    public Vector2 movement = new Vector2(0, 0);
    public Transform attackPoint;
    public bool isDead = false;
    float timer = 0;
    private Rigidbody2D myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(isDead) return;
        timer -= Time.deltaTime;
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
            return;
        }

        var offset = target.transform.position - transform.position;
        var direction = offset.normalized;
        movement = direction;
        
        if (GetDistanceToTarget() < stopDistance)
        {
            movement = Vector2.zero;
            if(timer < 0) 
            {
                //Debug.Log("attack!");
                BulletAttack();
                timer = cooldown;
            }
        }
    }

    public void takeDamage(float damageTaken)
    {
        if(isDead) return;
        health -= damageTaken;
        if(health <= 0)
        {
            isDead = true;
            myRigidbody.mass = myRigidbody.mass * 100;
            myRigidbody.velocity = new Vector2(0,0);
            deathParticleSystem.Play();
            Invoke("death", deathParticleSystem.main.duration);
        }
    }

    void death()
    {
        Destroy(gameObject);
    }

    public void BulletAttack()
    {   
        if(isDead) return;

        //Debug.Log("Shooting!");
        var angleDifference = UnityEngine.Random.Range(-bulletSpread,bulletSpread);
        var offset = target.transform.position - transform.position;
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0,0, angle + angleDifference);
        var bullet = Instantiate(bulletPrefab, transform.position + offset.normalized * projectileSpawnDistance, rotation);
        bullet.myController = this;
        if(isMelee)
            Instantiate(meleeAnim, transform.position + offset.normalized * meleeSpawnDistance, rotation);   
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