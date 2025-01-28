using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
public class VSPlayer : MonoBehaviour, IPlayer
{
    const string SWORD_TRIGGER = "isAttackingTrigger";
    const string IS_ATTACKING = "isAttacking";
    const string COMBO_STEP = "comboStep";
    const string WALKING = "isWalking";
    const string IDLE = "isIdle";
    const string DEAD = "isDead";
    const string CAMERA_NAME = "PlayerCamera";
    const string ACTION_MAP = "Player";
    const string LOOK_ACTION = "Look";
    const string MOVE_ACTION = "Move";
    const string SWORD_ATTACK_ACTION = "SwordAttack";
    const string JOYSTICK_LOOK_ACTION = "JoystickLook";
    const string DASH_ACTION = "DASH";
    const string GAMEPAD_SCHEME = "Gamepad";
    const string KM_SCHEME = "Keyboard&Mouse";
    [SerializeField] Vector2 movementSpeed = new Vector2(0.0f, 0.0f);
    Vector2 inputVector = new Vector2(0.0f, 0.0f);
    int comboStep = 0;
    float comboTimer;
    bool isAttacking = false;
    bool bufferedInput = false;
    float animationProgress = 0f;
    bool facingRight = true;
    Rigidbody2D myRigid;
    Animator anim;
    [SerializeField] float knockbackForce = 5f;
    [SerializeField] float knockbackDuration = 0.2f;
    [SerializeField] float maxComboDelay = 0.8f;
    [SerializeField] float timeBtwAttack;
    [SerializeField] float startTimeBtwAttack;
    [SerializeField] Transform attackPos;
    [SerializeField] Transform attackPosLeft;

    [SerializeField] LayerMask whatIsEnemies;
    [SerializeField] LayerMask whatIsTerrain;
    [SerializeField] float attackRange;
    [SerializeField] int baseAttackStrength;
    int currentAttackStrength; public int CurrentAttackStrength{get => currentAttackStrength; set => currentAttackStrength = value;}
    float damageModifier = 0; public float DamageModifier{get => damageModifier; set => damageModifier = value;}
    [SerializeField]int baseHp;
    public bool shouldBeDamaging {get; private set;} = false;
    List<IEnemy> damagedEnemies = new List<IEnemy>();
    List<Damagable> damagedTerrain = new List<Damagable>();
    public static VSPlayer Instance;
    [SerializeField] Vector2 activeMovementSpeed;
    [SerializeField] Vector2 dashSpeed;
    [SerializeField] float dashLength, dashCooldown;
    float dashCounter;
    float dashCoolCounter;
    [SerializeField] string className;
    CinemachineCamera playerCamera;
    CinemachineImpulseSource impulseSource;
    [SerializeField] InputActionAsset playerControls;
    InputAction move;
    InputAction look;
    InputAction dash;
    InputAction swordAttack;
    InputAction joysticklook;
    PlayerInput playerInput;
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite sprite;
    [SerializeField] ClassSO classSo;
    [SerializeField] AudioClip[] steppingClips;
    [SerializeField] float stepLength;
    [SerializeField] float stepTimer;
    public bool isDoubleProjectile = false; public bool IsDoubleProjectile{get => isDoubleProjectile; set => isDoubleProjectile = value;}
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerControls = playerInput.actions;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        classSo.CreateClassInfo(this.gameObject);
        currentAttackStrength = baseAttackStrength;
        activeMovementSpeed = movementSpeed;
        playerCamera = GameObject.Find(CAMERA_NAME).GetComponent<CinemachineCamera>();
        playerCamera.Follow = this.gameObject.transform;
        playerCamera.LookAt = this.gameObject.transform;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void OnEnable()
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

    void OnDisable()
    {
        if(playerControls != null){
            playerControls.Disable();
            move.Disable();
            look.Disable();
            dash.Disable();
            swordAttack.Disable();
            joysticklook.Disable();
        }
    }


    void Update()
    {
        if(GameManager.Instance.currentState == GameManager.GameState.Playing)
        {
            inputVector = move.ReadValue<Vector2>();
            if(dash.WasPressedThisFrame())
            {
                if(dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    activeMovementSpeed += dashSpeed;
                    dashCounter = dashLength;
                }
            }
            if(dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;

                if(dashCounter <= 0)
                {
                    activeMovementSpeed -= dashSpeed;
                    dashCoolCounter = dashCooldown;
                }
            }

            if(dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }

            if(timeBtwAttack <= 0){
                HandleCombatInput();
                ResetComboTimer();
            }else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
        
    }

    void FixedUpdate()
    {
        if(GameManager.Instance.currentState == GameManager.GameState.Playing)
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
                stepTimer = .15f;
            }else
            {
                anim.SetBool(IDLE, false);
                anim.SetBool(WALKING, true);
                stepTimer += Time.deltaTime;

                if (stepTimer > stepLength)
                {
                    int rand = UnityEngine.Random.Range(0, steppingClips.Length);
                    SoundManager.Instance.PlaySoundEffect(steppingClips[rand], transform, 0.05f);
                    stepTimer = 0f;
                }
            }
        }
        
    }

    void HandleCombatInput()
    {
        if (swordAttack.WasPressedThisFrame())
        {
            if (!isAttacking)
            {
                // Start the combo
                isAttacking = true;
                comboStep = 1;
                comboTimer = maxComboDelay;
                anim.SetInteger(COMBO_STEP, comboStep);
                anim.SetBool(IS_ATTACKING, true);
                anim.SetTrigger(SWORD_TRIGGER);
            }
            else if (comboStep < 3 && comboTimer > 0)
            {
                // Buffer the input if current animation is not ready to chain
                if (animationProgress < 0.7f) // Allow buffering during the first 70% of the animation
                {
                    bufferedInput = true;
                }
                else
                {
                    // Chain the next attack immediately if animation is ready
                    TriggerNextAttack();
                }
            }
        }
    }

    void TriggerNextAttack()
    {
        comboStep++;
        comboTimer = maxComboDelay; // Reset combo timer
        anim.SetInteger(COMBO_STEP, comboStep);
        anim.SetTrigger(SWORD_TRIGGER);
        bufferedInput = false; // Clear buffered input after triggering the next attack
    }

    void ResetComboTimer()
    {
        if (isAttacking)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0)
            {
                // End the combo if time runs out
                isAttacking = false;
                comboStep = 0;
                anim.SetBool(IS_ATTACKING, false);
                bufferedInput = false; // Clear any buffered input
                timeBtwAttack = startTimeBtwAttack;
            }
        }
    }

    public void OnAttackAnimationEnd()
    {
        if (comboStep >= 3 || comboTimer <= 0)
        {
            isAttacking = false;
            comboStep = 0;
            anim.SetBool(IS_ATTACKING, false);
            timeBtwAttack = startTimeBtwAttack;
        }
        else if (bufferedInput)
        {
            // Trigger the next attack if there was buffered input
            TriggerNextAttack();
        }else{
            timeBtwAttack = startTimeBtwAttack;
        }
    }

    public void UpdateAnimationProgress(float progress)
    {
        animationProgress = progress; // This value should range from 0.0 to 1.0
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
    }

    public IEnumerator DamageWhileAttackingIsActive()
    {
        //Freeze();
        shouldBeDamaging = true;
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
                if(enemy != null && !damagedEnemies.Contains(enemy)/* && enemiesToDamage[i].collider is PolygonCollider2D*/)
                {
                    if(!shook)
                    {
                        impulseSource.GenerateImpulse(new Vector3(0, -0.1f, 0));
                        shook = true;
                    }
                    // Calculate knockback direction
                    if(enemy.CanBeKnockedBack)
                    {
                        Vector2 knockbackDirection = enemiesToDamage[i].transform.position - transform.position;
                        enemy.ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
                    }
                    enemy.TakeDamage(currentAttackStrength + Mathf.RoundToInt(currentAttackStrength * damageModifier));
                    damagedEnemies.Add(enemy);
                }
            }

            RaycastHit2D[] terrainToDamage;
            if(facingRight)
            {
                terrainToDamage = Physics2D.CircleCastAll(attackPos.position, attackRange, transform.right, 0f, whatIsTerrain);
            }else
            {
                terrainToDamage = Physics2D.CircleCastAll(attackPosLeft.position, attackRange, transform.right, 0f, whatIsTerrain);
            }
            for (int i = 0; i < terrainToDamage.Length; i++)
            {
                Damagable terrain = terrainToDamage[i].collider.gameObject.GetComponent<Damagable>();
                if(terrain != null && !damagedTerrain.Contains(terrain)/* && enemiesToDamage[i].collider is PolygonCollider2D*/)
                {
                    terrain.TakeDamage(currentAttackStrength + Mathf.RoundToInt(currentAttackStrength * damageModifier));
                    damagedTerrain.Add(terrain);
                }
            }
            yield return null;
        }
        ReturnDamagedEnemiesHittable();
        ReturnDamagedTerrainHittable();
    }

    private void ReturnDamagedEnemiesHittable()
    {
        damagedEnemies.Clear();
    }

    private void ReturnDamagedTerrainHittable()
    {
        damagedTerrain.Clear();
    }

    public void AttackStop()
    {
        shouldBeDamaging = false;
        //myRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
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

    public void GainGold(int amount)
    {
        GameManager.Instance.GainGold(amount);
    }

    public void BoostStrength(int amount)
    {
        currentAttackStrength += amount;
    }

    public void DecreaseStrength(int amount)
    {
        currentAttackStrength -= amount;
    }

    public void TakeDamage(int amount)
    {
        GameManager.Instance.TakeDamage(amount);
    }

    public void Die()
    {
        Debug.Log("DEAD!");
        anim.SetBool(IDLE, false);
        anim.SetBool(WALKING, false);
        anim.SetTrigger(DEAD);
    }

    public void ResetPlayer()
    {
        if(anim != null)
        {
            anim.SetBool(IDLE, true);
        }
    }

    public void UpgradeDashSpeed(float percent)
    {
        dashSpeed = new Vector2(dashSpeed.x + dashSpeed.x * percent, dashSpeed.y + dashSpeed.y * percent);
    }

    public void UpgradeDashLength(float percent)
    {
        dashLength += dashLength * percent;
    }

    public void UpgradeDashCooldown(float percent)
    {
        dashCooldown -= dashCooldown * percent;
    }

    public void SetMovementSpeed(Vector2 movementSpeed)
    {
        this.movementSpeed = movementSpeed;
    }

    public void SetActiveMovementSpeed(Vector2 movementSpeed)
    {
        this.activeMovementSpeed = movementSpeed;
    }

    public Vector2 GetActiveMovementSpeed()
    {
        return activeMovementSpeed;
    }

    public void SetDashSpeed(Vector2 dashSpeed)
    {
        this.dashSpeed = dashSpeed;
    }

    public void SetClassName(string className)
    {
        this.className = className;
    }

    public void SetStartTimeBtwAttack(float startTimeBtwAttack)
    {
        this.startTimeBtwAttack = startTimeBtwAttack;
    }

    public void SetDashLength(float dashLength)
    {
        this.dashLength = dashLength;
    }

    public void SetDashCooldown(float dashCooldown)
    {
        this.dashCooldown = dashCooldown;
    }

    public void SetBaseAttackStrength(int baseAttackStrength)
    {
        this.baseAttackStrength = baseAttackStrength;
    }

    public void SetBaseHp(int baseHp)
    {
        this.baseHp = baseHp;
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        this.sprite = sprite;
    }

}
