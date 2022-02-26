using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, PlayerInput.IMovementActions, PlayerInput.IAttackActions
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    
    [Header("Dependencies")]
    public Transform rotationTransform;
    public GameObject bullet;
    public Animator animator;

    [Header("Configuration")]
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private float dashSpeed = 10f;
    public Transform attackPoint;
    public float basicAttackDamage = 1f;
    public float AOEAttackDamage = 1f;
    public float AOERadius = 6f;
    public float AOEOffset = 2f;
    private float health = 10f;

    [Header("Runtime")]
    public bool dead;
    
    // Private
    private Vector2 movementInput;
    private PlayerInput playerInput;
    private Collider2D myCollider;
    private Rigidbody2D myRigidbody;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Movement.SetCallbacks(this);
        playerInput.Attack.SetCallbacks(this);
    }

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        //Debug.Log("moving!");
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        //Debug.Log("dashing!");
        myRigidbody.AddForce(movementInput * dashSpeed, ForceMode2D.Impulse);
    }

    public void OnBasicAttack(InputAction.CallbackContext context)
    {
        if (dead)
        {
            return;
        }

        if (!context.performed)
        {
            return;
        }

        //Debug.Log("Shooting!");
        Instantiate(bullet, attackPoint.position, attackPoint.transform.rotation);
        // if(!context.performed) return;
        //     //Debug.Log("Attacking");

        // Collider2D[] colliders = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(1f, 1f), transform.rotation.eulerAngles.z);
        // foreach(Collider2D collider in colliders) 
        // {
        //     if(collider.TryGetComponent(out EnemyController enemyController)) 
        //     {
        //         enemyController.takeDamage(basicAttackDamage);
        //         //Debug.Log("Enemy here!");
        //     }
        // }
    }

    public void OnAOEAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        Debug.Log("AOE Attack!");

        for (var i = AOERadius; i > 0; i -= AOEOffset)
        {
            var colliders = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(i, i), transform.rotation.eulerAngles.z);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out EnemyController enemyController))
                {
                    enemyController.takeDamage(AOEAttackDamage);
                    Debug.LogWarning("AOE DAMAGE " + i);
                }
            }
        }
    }

    public void takeDamage(float damageTaken)
    {
        health -= damageTaken;
        if (health <= 0)
        {
            Debug.Log("im dead :(");
            dead = true;
        }
        else
        {
            Debug.Log("Damage taken! health: " + health);
        }
    }

    private void FixedUpdate()
    {
        myRigidbody.AddForce(movementInput * moveSpeed);
        animator.SetBool(IsWalking, movementInput.sqrMagnitude > 0.1f);
    }

    public float getBasicAttackDamage()
    {
        return basicAttackDamage;
    }
}