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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySo.CreateStats(gameObject);
        anim = GetComponent<Animator>();
    }
    void FixedUpdate()
    {

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
            anim.SetBool("isAttacking", false);
            anim.SetBool("isRunning", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);
            anim.SetBool("isRunning", false);
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

    public string getEnemyName()
    {
        return enemyName;
    }

    public void setEnemyName(string enemyName)
    {
        this.enemyName = enemyName;
    }
}
