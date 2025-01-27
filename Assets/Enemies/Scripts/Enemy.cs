using System.Collections;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    protected int hp; public int Hp{get => hp; set => hp = value;}
    protected int maxHp; public int MaxHp{ get => maxHp; set => maxHp = value;}
    protected int strength; public int Strength{ get => strength; set => strength = value;}
    protected string enemyName; public string EnemyName{ get => enemyName; set => enemyName = value;}
    protected float speed; public float Speed{ get => speed; set => speed = value;}
    protected float currentSpeed; public float CurrentSpeed{ get => currentSpeed; set => currentSpeed = value;}
    protected int scoreValue; public int ScoreValue{ get => scoreValue; set => scoreValue = value;}
    protected bool isFreezable; public bool IsFreezable{ get => isFreezable; set => isFreezable = value;}
    [SerializeField] protected AudioClip[] damagedClips;
    [SerializeField] protected LayerMask whatIsPlayer;
    protected const string PLAYER_TAG = "Player";
    const string SPAWNER_NAME = "Spawner";
    const string PLAYSYSTEM_MANAGER_NAME = "PlaySystemManager";
    protected float _soundFxVolume = 0.05f;
    [SerializeField] protected GameObject[] loot; public GameObject[] Loot{ get => loot; set => loot = value;}
    [SerializeField] protected EnemySO enemySo;
    protected Transform playerPos;
    protected IPlayer player;
    MobSpawner ms;
    public EnemyLevel enemyLevel = new EnemyLevel();
    protected PlaySystemManager playManager;
    [SerializeField] protected float detectionRadius; // Radius for overlap detection
    [SerializeField] protected float separationStrength; // Force to push enemies apart
    protected Animator anim;
    protected const string RUNNING = "isRunning";
    protected bool facingRight = false;
    [SerializeField] protected LayerMask enemyLayer; // Layer to detect other enemies
    [SerializeField] protected bool canBeKnockedBack = true; public bool CanBeKnockedBack{ get => canBeKnockedBack; set => canBeKnockedBack = value;}
    public enum EnemyLevel{
        basic,
        intermediate,
        advanced,
        legendary
    };

    Vector2 knockbackDirection;
    float knockbackDuration;
    float knockbackSpeed;
    bool isKnockedBack;

    void Awake()
    {
    }

    void OnEnable()
    {
        enemySo.CreateStats(gameObject);
        hp = maxHp;
        currentSpeed = speed;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<IPlayer>();
        playerPos = player.transform;
        ms = GameObject.Find(SPAWNER_NAME).GetComponent<MobSpawner>();
        playManager = GameObject.Find(PLAYSYSTEM_MANAGER_NAME).GetComponent<PlaySystemManager>();
    }

    void FixedUpdate()
    {
        PreventOverlap();
        if (isKnockedBack)
        {
            // Move the enemy in the knockback direction
            transform.position += (Vector3)(knockbackDirection * knockbackSpeed * Time.deltaTime);

            // Decrease the knockback duration
            knockbackDuration -= Time.deltaTime;

            // Stop knockback when the duration is over
            if (knockbackDuration <= 0)
            {
                isKnockedBack = false;
            }
        }
        CheckBounds();
    }

    void PreventOverlap()
    {
        // Detect nearby colliders within the detection radius
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

        foreach (Collider2D enemy in nearbyEnemies)
        {
            // Ignore self-collision
            if (enemy.gameObject == gameObject) continue;

            // Calculate direction away from the other enemy
            Vector2 directionAway = (transform.position - enemy.transform.position).normalized;

            // Apply a small displacement to separate the objects
            transform.position += (Vector3)(directionAway * separationStrength * Time.fixedDeltaTime);
        }
    }

    public virtual void TakeDamage (int damage)
    {
        hp -= damage;
        if(damagedClips.Length > 0)
        {
            int randInt = Random.Range(0, damagedClips.Length);
            SoundManager.Instance.PlaySoundEffect(damagedClips[randInt], transform, _soundFxVolume);
        }
        if (hp <= 0)
        {
            playManager.IncreaseScore(scoreValue);
            if(loot.Length > 0)
            {
                float randomInt = Random.Range(0f, 100.0f);
                dropLoot(randomInt);
            }
            GetComponent<SpriteRenderer>().color = Color.white;
            currentSpeed = speed;
            hp = maxHp;
            foreach(Projectile projectile in GetComponentsInChildren<Projectile>().ToList())
            {
                projectile.gameObject.transform.parent = null;
                projectile.gameObject.SetActive(false);
            }
            ms.RequeueMob(this.gameObject);
        }
    }

    protected virtual void dropLoot(float randomFloat)
    {
        //Vector3 objectPosition = transform.position;
        //Vector3 randPoint = new Vector3(objectPosition.x + Random.Range(-1.0f, 1.0f), objectPosition.y + Random.Range(-1.0f, 1.0f));
        if(randomFloat < 15)
        {
            Instantiate(loot[0], transform.position, Quaternion.identity);
        }else if(randomFloat >= 50 && randomFloat <= 100 && loot.Length > 1)
        {
            Instantiate(loot[1], transform.position, Quaternion.identity);
        }
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
    void CheckBounds(){
        if(transform.position.x < Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, 0, 0)).x
        || transform.position.x > Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 0, 0)).x)
        {
            ms.RespawnMob(this.gameObject);
        }

        if(transform.position.y < Camera.main.ViewportToWorldPoint(new Vector3(0, -0.1f, 0)).y
        || transform.position.y > Camera.main.ViewportToWorldPoint(new Vector3(0, 1.1f, 0)).y)
        {
            ms.RespawnMob(this.gameObject);
        }
    }

    protected void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void ApplyKnockback(Vector2 direction, float speed, float duration)
    {
        knockbackDirection = direction.normalized;
        knockbackSpeed = speed;
        knockbackDuration = duration;
        isKnockedBack = true;
    }
}
