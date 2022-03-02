using System.Collections;
using Project.Source;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, PlayerInput.IMovementActions, PlayerInput.IAttackActions
{
    public static readonly int IsWalking = Animator.StringToHash("IsWalking");
    public static readonly int OnAttack = Animator.StringToHash("OnAttack");

    [Header("Dependencies")]
    public SpriteRenderer playerSprite;
    public Transform rotationTransform;
    public PlayerBullet bulletPrefab;
    public Animator playerAnimator;
    public Animator leftArmAnimator;
    public Animator rightArmAnimator;
    public Transform attackPoint;
    public ParticleSystem deathParticleSystem;
    public ParticleSystem deathParticleSystem2;
    public Camera playerCamera;
    public Collider2D worldCollider;
    public Collider2D damageCollider;

    [Header("Sounds")]
    public AudioClip dashSound;
    public AudioClip runSound;
    public AudioSource runNoise;

    [Header("Configuration")]
    public float moveSpeed = 10f;
    public float dashSpeed = 10f;
    public float projectileSpeed = 1f;
    public float basicAttackDamage = 1f;
    public float AOEAttackDamage = 1f;
    public float AOERadius = 6f;
    public float AOEOffset = 2f;
    public float health = 10f;
    public float maxHealth = 10f;
    public float healRate = 0.1f;
    public float iframeDuration = 0.1f;
    public float dashCooldown = 0.1f;
    public float castRate = 5f;

    [Header("Runtime")]
    public bool isDead;
    // Private
    private Vector2 movementInput;
    private PlayerInput playerInput;
    private Rigidbody2D myRigidbody;

    private float castTimer;

    private bool isInvulnerable;
    private bool canDash = true;
    private bool isCasting;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Movement.SetCallbacks(this);
        playerInput.Attack.SetCallbacks(this);
    }

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        health = maxHealth;
        PlayerManager.Instance.players.Add(this);
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

        if (runNoise.isActiveAndEnabled && !runNoise.isPlaying)
        {
            runNoise.Play();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (isDead)
        {
            return;
        }

        if (!context.performed)
        {
            return;
        }

        if (canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public void OnBasicAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCasting = true;
        }
        else if (context.canceled)
        {
            isCasting = false;
        }
    }

    public void OnSpreadAttack(InputAction.CallbackContext context)
    {
        //     if(dead) return;
        //     if (!context.performed)
        //     {
        //         return;
        //     }
        //     Vector3 rot = attackPoint.transform.rotation.eulerAngles;
        //     rot = new Vector3(0,0,rot.z);
        //     var angle1 = Quaternion.Euler(rot);
        //     rot = new Vector3(0,0,rot.z-20);
        //     var angle2 = Quaternion.Euler(rot);
        //     rot = new Vector3(0,0,rot.z+40);
        //     var angle3 = Quaternion.Euler(rot);
        //     Instantiate(bullet, attackPoint.position, angle1);
        //     Instantiate(bullet, attackPoint.position, angle2);
        //     Instantiate(bullet, attackPoint.position, angle3);
        //     leftArmAnimator.SetTrigger(OnAttack);
        //     rightArmAnimator.SetTrigger(OnAttack);
    }

    public void OnAOEAttack(InputAction.CallbackContext context)
    {
        //     if(dead) return;
        //     if (!context.performed)
        //     {
        //         return;
        //     }
        //     Vector3 rot = transform.rotation.eulerAngles;
        //     leftArmAnimator.SetTrigger(OnAttack);
        //     rightArmAnimator.SetTrigger(OnAttack);
        //     for(int i = 0; i < 19; i++)
        //     {
        //         rot = new Vector3(0,0,rot.z - 20);
        //         var angle1 = Quaternion.Euler(rot);
        //         Instantiate(bullet, attackPoint.position, angle1); 
        //     }
    }

    public void TakeDamage(float damageTaken)
    {
        if (isInvulnerable || isDead)
        {
            return;
        }

        health -= damageTaken;

        if (health <= 0)
        {
            deathParticleSystem.Play();
            deathParticleSystem2.Play();
            StartCoroutine(Death());
            myRigidbody.velocity = new Vector2(0, 0);
            isDead = true;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isInvulnerable = true;

        myRigidbody.AddForce(movementInput * dashSpeed, ForceMode2D.Impulse);
        SoundManager.Instance.PlaySound(dashSound, transform.position, 0.75f);

        yield return new WaitForSeconds(iframeDuration);

        isInvulnerable = false;

        yield return new WaitForSeconds(iframeDuration - dashCooldown);

        canDash = true;
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(1);

        yield return FadeTransition.Instance.FadeToBlack(0.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        if (health < maxHealth)
        {
            health += Time.deltaTime * healRate;
        }

        castTimer -= Time.deltaTime;
        if (isCasting && castTimer < 0)
        {
            castTimer = 1 / castRate;
            Attack();
        }

        playerSprite.color = isInvulnerable ? new Color(1, 1, 1, 0.5f) : Color.white;
        damageCollider.enabled = !isInvulnerable;

        myRigidbody.AddForce(movementInput * moveSpeed);
        playerAnimator.SetBool(IsWalking, movementInput.sqrMagnitude > 0.1f);
        if (runNoise.isPlaying)
        {
            if (!Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("s") && !Input.GetKey("d"))
            {
                runNoise.Pause();
            }
        }
    }

    public void Attack()
    {
        if (isDead)
        {
            return;
        }

        var bullet = Instantiate(bulletPrefab, attackPoint.position, attackPoint.transform.rotation);
        bullet.player = this;

        leftArmAnimator.SetTrigger(OnAttack);
        rightArmAnimator.SetTrigger(OnAttack);
    }
}