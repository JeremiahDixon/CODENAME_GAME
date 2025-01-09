using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    public int hp;
    public int strength;
    public string enemyName;
    public float speed;
    public float currentSpeed;
    public int scoreValue;
    public AudioClip[] damagedClips;
    public LayerMask whatIsPlayer;
    public const string PLAYER_TAG = "Player";
    public GameObject[] loot;
    public EnemySO enemySo;
    public Transform playerPos;
    public IPlayer player;
    MobSpawner ms;
    public EnemyLevel enemyLevel = new EnemyLevel();
    PlaySystemManager playManager;

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
        playManager = GameObject.Find("PlaySystemManager").GetComponent<PlaySystemManager>();

    }

    void Start()
    {
    }

    // Update is called once per frame
    // void FixedUpdate()
    // {
    // }

    public void TakeDamage (int damage)
    {
        hp -= damage;
        int randInt = Random.Range(0, damagedClips.Length);
        SoundManager.Instance.PlaySoundEffect(damagedClips[randInt], transform, 1.0f);
        if (hp <= 0)
        {
            playManager.IncreaseScore(scoreValue);
            float randomInt = Random.Range(0f, 100.0f);
            dropLoot(randomInt);
            ms.RequeueMob(this.gameObject);
        }
    }

    public virtual void dropLoot(float randomInt)
    {

    }

    public void Freeze(float time)
    {
        StartCoroutine(FreezeForX(time));
    }
    IEnumerator FreezeForX(float time)
    {
        GetComponent<SpriteRenderer>().color = Color.blue;
        currentSpeed = 0;
        yield return new WaitForSeconds(time);
        GetComponent<SpriteRenderer>().color = Color.white;
        currentSpeed = speed;
    }

    //this works if i decide to despawn the mob after leaves camera bounds
    public void CheckBounds(){
        if(transform.position.x < Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x
        || transform.position.x > Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x)
        {
            ms.RequeueMob(this.gameObject);
        }

        if(transform.position.y < Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y
        || transform.position.y > Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y)
        {
            ms.RequeueMob(this.gameObject);
        }
    }

}
