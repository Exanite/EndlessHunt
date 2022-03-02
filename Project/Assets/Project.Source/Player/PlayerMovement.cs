using Project.Source;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using System.Collections;

public class PlayerMovement : MonoBehaviour, PlayerInput.IMovementActions, PlayerInput.IAttackActions
{
    public static readonly int IsWalking = Animator.StringToHash("IsWalking");
    public static readonly int OnAttack = Animator.StringToHash("OnAttack");
    
    [Header("Dependencies")]
    public Transform rotationTransform;
    public GameObject bullet;
    public Animator playerAnimator;
    public Animator leftArmAnimator;
    public Animator rightArmAnimator;
    public Transform attackPoint;
    public ParticleSystem deathParticleSystem;
    public ParticleSystem deathParticleSystem2;
    public Camera playerCamera;

    [Header("Sounds")]
    public AudioClip dashSound;
    public AudioClip runSound;
    public AudioSource runNoise;

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

        if(runNoise.isActiveAndEnabled && !runNoise.isPlaying)
            runNoise.Play();
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

    public void takeDamage(float damageTaken)
    {
        health -= damageTaken;
        if (health <= 0)
        {
            if(reloading) return;
            deathParticleSystem.Play();
            deathParticleSystem2.Play();
            StartCoroutine(Death());
            myRigidbody.velocity = new Vector2(0,0);
            dead = true;
        }
    }

    private IEnumerator Death()
    {
        reloading = true;
        //Debug.Log("about to reload");
        yield return new WaitForSeconds(1);

        yield return FadeTransition.Instance.FadeToBlack(0.5f);
        //Debug.Log("reloading");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void FixedUpdate()
    {
        if (dead) return;
        if(health < maxHealth)
            health += Time.deltaTime*timePerHealthPoint;
        myRigidbody.AddForce(movementInput * moveSpeed);
        playerAnimator.SetBool(IsWalking, movementInput.sqrMagnitude > 0.1f);
        if(runNoise.isPlaying)
            if(!Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("s") && !Input.GetKey("d"))
            {
                runNoise.Pause();
            }
    }

    public float getBasicAttackDamage()
    {
        return basicAttackDamage;
    }
}