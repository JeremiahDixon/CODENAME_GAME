using UnityEngine;

public class Dwarf : Enemy
{
    private Animator anim;
    [SerializeField]
    private float timeBtwAttack;
    private bool facingRight = false;
    const string ATTACKING_TRIGGER = "isAttackingTrigger";
    const string RUNNING = "isRunning";
    [SerializeField]
    Transform attackPos;
    [SerializeField]
    float attackRange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySo.CreateStats(gameObject);
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<IPlayer>();
        playerPos = player.transform;
        anim.SetBool(RUNNING, true);
    }
    // void FixedUpdate()
    // {
    // }

    void Update()
    {
        if(Vector2.Distance(anim.transform.position, playerPos.position) > 0.75f)
        {
            anim.transform.position = Vector2.MoveTowards(anim.transform.position, playerPos.position, speed * Time.deltaTime);
        }
        if(Vector2.Distance(transform.position, playerPos.position) <= 0.75f)
        {
            anim.SetBool(RUNNING, false);
            if(timeBtwAttack <= 0)
            {
                timeBtwAttack = 3f;
                anim.SetTrigger(ATTACKING_TRIGGER);
            }
        }else
        {
            anim.SetBool(RUNNING, true);
        }
        if(timeBtwAttack >= 0)
        {
            timeBtwAttack -= Time.deltaTime;
        }
        if(playerPos.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }else if(playerPos.position.x < transform.position.x && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    //call this durring animation event to check if hit
    void tryToHitPlayer()
    {
        //create a attackpos and replace transform
        RaycastHit2D playerToDamage = Physics2D.CircleCast(attackPos.position, attackRange, transform.right, 0f, whatIsPlayer);
        if( playerToDamage )
        {
            IPlayer thePlayer = playerToDamage.collider.gameObject.GetComponent<IPlayer>();
            if(thePlayer != null && playerToDamage.collider is BoxCollider2D){
                thePlayer.TakeDamage(strength);
                //might need to add a bool playerBeenDamaged
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
