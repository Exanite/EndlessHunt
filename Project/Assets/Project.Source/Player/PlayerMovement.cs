using Project.Source;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using System.Collections;

public class PlayerMovement : MonoBehaviour, PlayerInput.IMovementActions, PlayerInput.IAttackActions
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int OnAttack = Animator.StringToHash("OnAttack");
    
    [Header("Dependencies")]
    public Transform rotationTransform;
    public GameObject bullet;
    public Animator playerAnimator;
    public Animator leftArmAnimator;
    public Animator rightArmAnimator;
    public Transform attackPoint;
    public ParticleSystem deathParticleSystem;

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
    public float timePerHealthPoint = 0.1f;
    
    [Header("Runtime")]
    public bool dead;
    // Private
    private Vector2 movementInput;
    private PlayerInput playerInput;
    private Collider2D myCollider;
    private Rigidbody2D myRigidbody;

    private bool reloading = false;

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
        health = maxHealth;
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
        if (dead) return;
        if (!context.performed)
        {
            return;
        }
        myRigidbody.AddForce(movementInput * dashSpeed, ForceMode2D.Impulse);
        SoundManager.Instance.PlaySound(dashSound, transform.position, 0.75f);
    }

    public void OnBasicAttack(InputAction.CallbackContext context)
    {
        if (dead) return;

        if (!context.performed)
        {
            return;
        }

        Instantiate(bullet, attackPoint.position, attackPoint.transform.rotation);
        leftArmAnimator.SetTrigger(OnAttack);
        rightArmAnimator.SetTrigger(OnAttack);
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
            if(reloading) return;
            deathParticleSystem.Play();
            StartCoroutine(Death());
            myRigidbody.velocity = new Vector2(0,0);
            Debug.Log("im dead :(");
            dead = true;
        }
        else
        {
            Debug.Log("Damage taken! health: " + health);
        }
    }

    private IEnumerator Death()
    {
        reloading = true;
        //Debug.Log("about to reload");
        yield return new WaitForSeconds(1);
        //Debug.Log("reloading");
        SceneManager.LoadScene(0);
    }

    private void FixedUpdate()
    {
        if (dead) return;
        if(health < maxHealth)
            health += Time.deltaTime*timePerHealthPoint;
        myRigidbody.AddForce(movementInput * moveSpeed);
        playerAnimator.SetBool(IsWalking, movementInput.sqrMagnitude > 0.1f);
    }

    public float getBasicAttackDamage()
    {
        return basicAttackDamage;
    }
}