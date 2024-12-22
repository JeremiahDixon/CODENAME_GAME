using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentStrength = strength;
    }


    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        moveInputY = Input.GetAxisRaw("Vertical");
        myRigid.linearVelocity = new Vector2(moveInput * speed, myRigid.linearVelocity.y);
        myRigid.linearVelocity = new Vector2(myRigid.linearVelocity.x, moveInputY * speed);

        if(facingRight == false && moveInput < 0){
            Flip();
        }else if(facingRight == true && moveInput > 0){
            Flip();
        }

        if(moveInput == 0 && moveInputY == 0){
            anim.SetBool("isWalking", false);
        }else{
            anim.SetBool("isWalking", true);
        }

        if(timeBtwAttack <= 0){
            if(Input.GetKeyDown(KeyCode.Space)){
                anim.SetTrigger("isAttackingTrigger");
            }
        }else{
            timeBtwAttack -= Time.deltaTime;
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
                if(enemy != null && !damagedEnemies.Contains(enemy)){
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
