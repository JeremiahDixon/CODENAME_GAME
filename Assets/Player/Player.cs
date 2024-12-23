using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 MovementSpeed = new Vector2(100.0f, 100.0f); // 2D Movement speed to have independant axis speed
    private Vector2 inputVector = new Vector2(0.0f, 0.0f);
    private Rigidbody2D myRigid;
    private float moveInput;
    private float moveInputY;
    private bool facingRight = true;
    private Animator anim;
    public float timeBtwAttack;
    public float startTimeBtwAttack;
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public int whatIsPickupItem;
    public float attackRange;
    [SerializeField]
    private int strength;
    private int currentStrength;
    public bool shouldBeDamaging {get; private set;} = false;
    private List<IEnemy> damagedEnemies = new List<IEnemy>();
    public static Player Instance;
    public Vector2 activeMovementSpeed;
    public Vector2 dashSpeed;
    public float dashLength = .5f, dashCooldown =1f;
    private float dashCounter;
    private float dashCoolCounter;
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
    }


    // Update is called once per frame
    void Update()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        
        if(Input.GetKeyDown(KeyCode.M)){
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
                anim.SetTrigger("isAttackingTrigger");
            }
        }else{
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // Rigidbody2D affects physics so any ops on it should happen in FixedUpdate
        myRigid.MovePosition(myRigid.position + (inputVector * activeMovementSpeed * Time.fixedDeltaTime));
        if(facingRight == false && inputVector.x < 0){
            Flip();
        }else if(facingRight == true && inputVector.x > 0){
            Flip();
        }

        if(moveInput == 0 && moveInputY == 0){
            anim.SetBool("isWalking", false);
        }else{
            anim.SetBool("isWalking", true);
        }
    }

    void Flip(){
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public IEnumerator DamageWhileAttackingIsActive(){
        myRigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        shouldBeDamaging = true;
        timeBtwAttack = startTimeBtwAttack;

        while(shouldBeDamaging){
            RaycastHit2D[] enemiesToDamage = Physics2D.CircleCastAll(attackPos.position, attackRange, transform.right, 0f, whatIsEnemies);
            for (int i = 0; i < enemiesToDamage.Length; i++){
                IEnemy enemy = enemiesToDamage[i].collider.gameObject.GetComponent<IEnemy>();
                if(enemy != null && !damagedEnemies.Contains(enemy) && enemiesToDamage[i].collider is PolygonCollider2D){
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

    void AttackStop(){
        shouldBeDamaging = false;
        myRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    public void Heal(int amount){
        GameManager.Instance.Heal(amount);
    }

    public void equipWeapon(int amount){
        currentStrength = strength + amount;
    }
}
