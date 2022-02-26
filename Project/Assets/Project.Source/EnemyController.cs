using Project.Source;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Configuration")]
    public float moveSpeed = 10f;
    public float aggroRadius = 5f;
    public float deaggroRadius = 10f;
    
    [Header("Runtime")]
    public PlayerMovement target;
    public Vector2 movement = new Vector2(0, 0);
    public Transform attackPoint;
    public float basicAttackDamage = 1f;
    
    private Rigidbody2D myRigidbody;
    private float health = 10;
    bool dead = false;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        UpdateTarget();
        UpdateMovementSpeed();
        
        myRigidbody.AddForce(movement * moveSpeed * myRigidbody.mass * myRigidbody.drag);
    }

    private void UpdateTarget()
    {
        if (target)
        {
            var offset = target.transform.position - transform.position;

            if (offset.magnitude > deaggroRadius)
            {
                target = null;
            }
        }
        else
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, aggroRadius, GameSettings.Instance.entityWorldLayerMask);
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
        if (offset.magnitude < 2)
        {
            movement = Vector2.zero;
            BasicAttack();
        }
        movement = direction;
    }

    public void takeDamage(float damageTaken)
    {
        health -= damageTaken;
        if(health <= 0)
        {
            //Debug.Log("im dead :(");
            dead = true;
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
}