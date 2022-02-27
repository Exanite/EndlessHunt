using Project.Source;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, PlayerInput.IMovementActions, PlayerInput.IAttackActions
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    
    [Header("Dependencies")]
    public Transform rotationTransform;
    public GameObject bullet;
    public Animator animator;
    public Transform attackPoint;

    [Header("Sounds")]
    public AudioClip dashSound;
    public AudioClip runSound;

    [Header("Configuration")]
    public float moveSpeed = 10f;
    public float dashSpeed = 10f;
    public float basicAttackDamage = 1f;
    public float AOEAttackDamage = 1f;
    public float AOERadius = 6f;
    public float AOEOffset = 2f;
    
    public float health = 10f;
    public float maxHealth = 10f;
    
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
        //SoundManager.Instance.PlaySound(runSound, transform.position, 0.75f);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        myRigidbody.AddForce(movementInput * dashSpeed, ForceMode2D.Impulse);
        SoundManager.Instance.PlaySound(dashSound, transform.position, 0.75f);
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

        Instantiate(bullet, attackPoint.position, attackPoint.transform.rotation);
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