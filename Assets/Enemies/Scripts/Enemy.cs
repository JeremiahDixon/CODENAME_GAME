using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    public int hp;
    public int strength;
    public string enemyName;
    public float speed;
    public AudioClip[] damagedClips;
    public LayerMask whatIsPlayer;
    public const string PLAYER_TAG = "Player";
    public GameObject[] loot;
    public EnemySO enemySo;
    public Transform playerPos;
    public IPlayer player;
    MobSpawner ms;
    public EnemyLevel enemyLevel = new EnemyLevel();

    public enum EnemyLevel{
        basic,
        intermediate,
        advanced,
        legendary
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ms = GameObject.Find("Spawner").GetComponent<MobSpawner>();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void TakeDamage (int damage)
    {
        hp -= damage;
        int randInt = Random.Range(0, damagedClips.Length);
        SoundManager.Instance.PlaySoundEffect(damagedClips[randInt], transform, 1.0f);
        if (hp <= 0)
        {
            GameManager.Instance.IncreaseScore(10);
            float randomInt = Random.Range(0f, 100.0f);
            dropLoot(randomInt);
            ms.RequeueMob(this.gameObject);
        }
    }

    public virtual void dropLoot(float randomInt)
    {

    }

}
