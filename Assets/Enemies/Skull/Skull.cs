using UnityEngine;

public class Skull : MonoBehaviour, IEnemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private int hp;
    [SerializeField]
    private int strength;
    [SerializeField]
    private string enemyName;
    private Item[] loot;
    public EnemySO enemySo;

    void Start()
    {
        enemySo.CreateStats(gameObject);
    }
    void FixedUpdate()
    {

    }

    public void TakeDamage (int damage){
        hp -= damage;
        Debug.Log("Damage Taken!");
        if (hp <= 0){
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){

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
