using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
public class VSPlayer : MonoBehaviour, IPlayer
{
    public Vector2 MovementSpeed = new Vector2(0.0f, 0.0f); // 2D Movement speed to have independant axis speed
    private Vector2 inputVector = new Vector2(0.0f, 0.0f);
    const string SWORD_TRIGGER = "isAttackingTrigger";
    const string WALKING = "isWalking";
    const string IDLE = "isIdle";
    const string CAMERA_NAME = "PlayerCamera";
    const string ACTION_MAP = "Player";
    const string LOOK_ACTION = "Look";
    const string MOVE_ACTION = "Move";
    const string SWORD_ATTACK_ACTION = "SwordAttack";
    const string JOYSTICK_LOOK_ACTION = "JoystickLook";
    const string DASH_ACTION = "DASH";
    const string GAMEPAD_SCHEME = "Gamepad";
    const string KM_SCHEME = "Keyboard&Mouse";
    private Rigidbody2D myRigid;
    private bool facingRight = true;
    private Animator anim;
    public float timeBtwAttack;
    public float startTimeBtwAttack;
    public Transform attackPos;
    public Transform attackPosLeft;
    public LayerMask whatIsEnemies;
    public int whatIsPickupItem;
    public float attackRange;
    [SerializeField]
    private int strength;
    private int currentStrength;
    public bool shouldBeDamaging {get; private set;} = false;
    private List<IEnemy> damagedEnemies = new List<IEnemy>();
    public static VSPlayer Instance;
    public Vector2 activeMovementSpeed;
    public Vector2 dashSpeed;
    public float dashLength = .5f, dashCooldown =1f;
    private float dashCounter;
    private float dashCoolCounter;
    private CinemachineCamera playerCamera;
    private CinemachineImpulseSource impulseSource;
    public InputActionAsset playerControls;
    private InputAction move;
    private InputAction look;
    private InputAction dash;
    private InputAction swordAttack;
    private InputAction joysticklook;
    private PlayerInput playerInput;

    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keeps this manager between scenes
        }
        else
        {
            Destroy(gameObject);  // Prevent duplicates
        }
        playerInput = GetComponent<PlayerInput>();
        playerControls = playerInput.actions;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentStrength = strength;
        activeMovementSpeed = MovementSpeed;
        playerCamera = GameObject.Find(CAMERA_NAME).GetComponent<CinemachineCamera>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        move = playerControls.FindActionMap(ACTION_MAP).FindAction(MOVE_ACTION);
        look = playerControls.FindActionMap(ACTION_MAP).FindAction(LOOK_ACTION);
        dash = playerControls.FindActionMap(ACTION_MAP).FindAction(DASH_ACTION);
        joysticklook = playerControls.FindActionMap(ACTION_MAP).FindAction(JOYSTICK_LOOK_ACTION);
        swordAttack = playerControls.FindActionMap(ACTION_MAP).FindAction(SWORD_ATTACK_ACTION);
        move.Enable();
        look.Enable();
        dash.Enable();
        joysticklook.Enable();
        swordAttack.Enable();
    }

    private void OnDisable()
    {
        //playerControls.Disable();
        move.Disable();
        look.Disable();
        dash.Disable();
        swordAttack.Disable();
        joysticklook.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        inputVector = move.ReadValue<Vector2>(); //new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        
        if(dash.WasPressedThisFrame())
        {
            if(dashCoolCounter <=0 && dashCounter <= 0)
            {
                activeMovementSpeed = dashSpeed;
                dashCounter = dashLength;
            }
        }
        if(dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            if(dashCounter <= 0)
            {
                activeMovementSpeed = MovementSpeed;
                dashCoolCounter = dashCooldown;
            }
        }

        if(dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
        if(timeBtwAttack <= 0)
        {
            if(swordAttack.WasPressedThisFrame())
            {
                anim.SetTrigger(SWORD_TRIGGER);
            }
        }else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // Rigidbody2D affects physics so any ops on it should happen in FixedUpdate
        myRigid.MovePosition(myRigid.position + (inputVector * activeMovementSpeed * Time.fixedDeltaTime));

        if(playerInput.currentControlScheme == GAMEPAD_SCHEME){
            Vector2 joystickPos = joysticklook.ReadValue<Vector2>();
            if(joystickPos != new Vector2(0, 0)){
                if(!facingRight && joystickPos.x > 0)
                {
                    Flip();
                }else if(facingRight && joystickPos.x <= 0)
                {
                    Flip();
                }
            }
        }else if(playerInput.currentControlScheme == KM_SCHEME){
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(look.ReadValue<Vector2>());
            if(!facingRight && mousePosition.x > transform.position.x)
            {
                Flip();
            }else if(facingRight && mousePosition.x < transform.position.x)
            {
                Flip();
            }
        }

        if(inputVector.x == 0 && inputVector.y == 0)
        {
            anim.SetBool(WALKING, false);
            anim.SetBool(IDLE, true);
        }else
        {
            anim.SetBool(IDLE, false);
            anim.SetBool(WALKING, true);
        }
    }

    void Freeze()
    {
        myRigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }
    void Flip()
    {
        facingRight = !facingRight;
        if(facingRight)
        {
            spriteRenderer.flipX = false;
        }else
        {
            spriteRenderer.flipX = true;
        }
        //flip the sword attack pos
    }

    public IEnumerator DamageWhileAttackingIsActive()
    {
        myRigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        shouldBeDamaging = true;
        timeBtwAttack = startTimeBtwAttack;
        bool shook = false;

        while(shouldBeDamaging)
        {
            RaycastHit2D[] enemiesToDamage;
            if(facingRight)
            {
                enemiesToDamage = Physics2D.CircleCastAll(attackPos.position, attackRange, transform.right, 0f, whatIsEnemies);
            }else
            {
                enemiesToDamage = Physics2D.CircleCastAll(attackPosLeft.position, attackRange, transform.right, 0f, whatIsEnemies);
            }
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                IEnemy enemy = enemiesToDamage[i].collider.gameObject.GetComponent<IEnemy>();
                if(enemy != null && !damagedEnemies.Contains(enemy) && enemiesToDamage[i].collider is PolygonCollider2D)
                {
                    if(!shook)
                    {
                        impulseSource.GenerateImpulse(new Vector3(0, -0.1f, 0));
                        shook = true;
                    }
                    enemy.TakeDamage(strength);
                    damagedEnemies.Add(enemy);
                }
            }
            yield return null;
        }
        Debug.Log("Attack!");
        ReturnDamagedEnemiesHittable();
    }

    private void ReturnDamagedEnemiesHittable()
    {
        damagedEnemies.Clear();
    }

    public void AttackStop()
    {
        shouldBeDamaging = false;
        myRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        Gizmos.DrawWireSphere(attackPosLeft.position, attackRange);
    }

    public void Heal(int amount)
    {
        GameManager.Instance.Heal(amount);
    }

    public void EquipWeapon(int amount)
    {
        currentStrength = strength + amount;
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Player Damaged!");
        GameManager.Instance.TakeDamage(amount);
    }

    public void Die()
    {
        Debug.Log("DEAD!");
    }

}
