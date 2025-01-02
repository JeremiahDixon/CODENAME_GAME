using UnityEditor;
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
    private Item[] loot;
    public EnemySO enemySo;
    private Animator anim;
    private Transform playerPos;
    private IPlayer player;
    public float timeBtwAttack;
    private bool isTargeting = false;
    [SerializeField]
    private float speed;
    [SerializeField]
    private LayerMask whatIsPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySo.CreateStats(gameObject);
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<IPlayer>();
        playerPos = player.transform;
    }
    void FixedUpdate()
    {
    }

    void Update(){
        if(isTargeting){
            anim.SetBool("isRunning", true);
            if(Vector2.Distance(anim.transform.position, playerPos.position) > 0.75f){
                anim.transform.position = Vector2.MoveTowards(anim.transform.position, playerPos.position, speed * Time.deltaTime);
            }
        }
        if(timeBtwAttack <= 0){
            if(Vector2.Distance(transform.position, playerPos.position) <= 0.75f){
                anim.SetBool("isRunning", false);
                timeBtwAttack = 3f;
                anim.SetTrigger("isAttackingTrigger");
            }
        }else{
            timeBtwAttack -= Time.deltaTime;
        }
    }

    public void TakeDamage (int damage){
        hp -= damage;
        Debug.Log("Damage Taken!");
        if (hp <= 0){
            Debug.Log("dropping loot!");
            int randomInt = Random.Range(1, 101);
            dropLoot(randomInt);
            Debug.Log("destorying!");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
            Debug.Log("Triggered");
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", true);
            isTargeting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
            isTargeting = false;
        } 
    }

    void dropLoot(int randomInt){
        Vector3 objectPosition = transform.position;
        Vector3 randPoint = new Vector3(objectPosition.x + Random.Range(-1.0f, 1.0f), objectPosition.y + Random.Range(-1.0f, 1.0f));
        if(randomInt < 10){
            Instantiate(loot[0], randPoint, Quaternion.identity);
        }else if(randomInt >= 10 && randomInt <= 100){
            Instantiate(loot[1], randPoint, Quaternion.identity);
        }
    }

    //call this durring animation event to check if hit
    void tryToHitPlayer(){
        //create a attackpos and replace transform
        RaycastHit2D playerToDamage = Physics2D.CircleCast(transform.position, 1, transform.right, 0f, whatIsPlayer);
        Player thePlayer = playerToDamage.collider.gameObject.GetComponent<Player>();
        if(thePlayer != null && playerToDamage.collider is BoxCollider2D){
            //damage, screenshake etc
            //might need to add a bool playerBeenDamaged
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
