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
    
    private Rigidbody2D myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        UpdateTarget();
        UpdateMovementSpeed();
        
        myRigidbody.AddForce(movement * moveSpeed);
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
            var colliders = Physics2D.OverlapCircleAll(transform.position, aggroRadius);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out PlayerMovement player))
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
    }
}