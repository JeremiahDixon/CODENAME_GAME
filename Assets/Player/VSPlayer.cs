using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class VSPlayer : MonoBehaviour, IPlayer
{
    public Vector2 MovementSpeed = new Vector2(100.0f, 100.0f); // 2D Movement speed to have independant axis speed
    private Vector2 inputVector = new Vector2(0.0f, 0.0f);
    const string ATTACKING_TRIGGER = "isAttackingTrigger";
    const string WALKING = "isWalking";
    const string IDLE = "isIdle";
    const string CAMERA_NAME = "PlayerCamera";
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


    // Update is called once per frame
    void Update()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        
        if(Input.GetKeyDown(KeyCode.E)){
            if(dashCoolCounter <=0 && dashCounter <= 0){
                activeMovementSpeed = dashSpeed;
                dashCounter = dashLength;
            }
        }
        if(dashCounter > 0){
            dashCounter -= Time.deltaTime;

            if(dashCounter <= 0){
                activeMovementSpeed = MovementSpeed;
                dashCoolCounter = dashCooldown;
            }
        }

        if(dashCoolCounter > 0){
            dashCoolCounter -= Time.deltaTime;
        }
        if(timeBtwAttack <= 0){
            if(Input.GetKeyDown(KeyCode.Space)){
                anim.SetTrigger(ATTACKING_TRIGGER);
            }
        }else{
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // Rigidbody2D affects physics so any ops on it should happen in FixedUpdate
        myRigid.MovePosition(myRigid.position + (inputVector * activeMovementSpeed * Time.fixedDeltaTime));
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(facingRight == false && mousePosition.x > transform.position.x){
            Flip();
        }else if(facingRight == true && mousePosition.x < transform.position.x){
            Flip();
        }

        if(inputVector.x == 0 && inputVector.y == 0){
            anim.SetBool(WALKING, false);
            anim.SetBool(IDLE, true);
        }else{
            anim.SetBool(IDLE, false);
            anim.SetBool(WALKING, true);
        }
    }

    void Freeze(){
        myRigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }
    void Flip(){
        facingRight = !facingRight;
        if(facingRight){
            spriteRenderer.flipX = false;
        }else{
            spriteRenderer.flipX = true;
        }
        //flip the sword attack pos
    }

    public IEnumerator DamageWhileAttackingIsActive(){
        myRigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        shouldBeDamaging = true;
        timeBtwAttack = startTimeBtwAttack;
        bool shook = false;

        while(shouldBeDamaging){
            RaycastHit2D[] enemiesToDamage;
            if(facingRight){
                enemiesToDamage = Physics2D.CircleCastAll(attackPos.position, attackRange, transform.right, 0f, whatIsEnemies);
            }else{
                enemiesToDamage = Physics2D.CircleCastAll(attackPosLeft.position, attackRange, transform.right, 0f, whatIsEnemies);
            }
            for (int i = 0; i < enemiesToDamage.Length; i++){
                IEnemy enemy = enemiesToDamage[i].collider.gameObject.GetComponent<IEnemy>();
                if(enemy != null && !damagedEnemies.Contains(enemy) && enemiesToDamage[i].collider is PolygonCollider2D){
                    if(!shook){
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
        returnDamagedEnemiesHittable();
    }

    private void returnDamagedEnemiesHittable(){
        damagedEnemies.Clear();
    }

    public void AttackStop(){
        shouldBeDamaging = false;
        myRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        Gizmos.DrawWireSphere(attackPosLeft.position, attackRange);
    }

    public void Heal(int amount){
        GameManager.Instance.Heal(amount);
    }

    public void equipWeapon(int amount){
        currentStrength = strength + amount;
    }

    public void TakeDamage(int amount){
        Debug.Log("Player Damaged!");
        GameManager.Instance.TakeDamage(amount);
    }

    public void Die(){
        Debug.Log("DEAD!");
    }

}
