using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Slime : MonoBehaviour, IEnemy
{
    [SerializeField]
    private int hp;
    [SerializeField]
    private int strength;
    [SerializeField]
    private string enemyName;
    [SerializeField]
    private GameObject[] loot;
    [SerializeField]
    private EnemySO enemySo;
    private Animator anim;
    private Transform playerPos;
    private IPlayer player;
    [SerializeField]
    private float timeBtwAttack;
    private bool facingRight = false;
    [SerializeField]
    private float speed;
    [SerializeField]
    private LayerMask whatIsPlayer;
    const string PLAYER_TAG = "Player";
    const string ATTACKING_TRIGGER = "isAttackingTrigger";
    const string RUNNING = "isRunning";
    [SerializeField]
    private AudioClip damagedClip;

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

    public void TakeDamage (int damage)
    {
        hp -= damage;
        SoundManager.Instance.PlaySoundEffect(damagedClip, transform, 1.0f);
        if (hp <= 0)
        {
            int randomInt = Random.Range(1, 101);
            dropLoot(randomInt);
            Destroy(gameObject);
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    // }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    // }

    void dropLoot(int randomInt)
    {
        Vector3 objectPosition = transform.position;
        Vector3 randPoint = new Vector3(objectPosition.x + Random.Range(-1.0f, 1.0f), objectPosition.y + Random.Range(-1.0f, 1.0f));
        if(randomInt < 15)
        {
            Instantiate(loot[0], randPoint, Quaternion.identity);
        }
    }

    //call this durring animation event to check if hit
    void tryToHitPlayer()
    {
        //create a attackpos and replace transform
        RaycastHit2D playerToDamage = Physics2D.CircleCast(transform.position, 0.75f, transform.right, 0f, whatIsPlayer);
        if( playerToDamage )
        {
            IPlayer thePlayer = playerToDamage.collider.gameObject.GetComponent<IPlayer>();
            if(thePlayer != null && playerToDamage.collider is BoxCollider2D){
                thePlayer.TakeDamage(strength);
                //might need to add a bool playerBeenDamaged
            }
        }
    }
    public int getHp()
    {
        return this.hp;
    }

    public void setHp(int hp)
    {
        this.hp = hp;
    }

    public int getStrength()
    {
        return this.strength;
    }

    public void setStrength(int strength)
    {
        this.strength = strength;
    }

    public float getSpeed()
    {
        return this.speed;
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    public string getEnemyName()
    {
        return enemyName;
    }

    public void setEnemyName(string enemyName)
    {
        this.enemyName = enemyName;
    }
}
