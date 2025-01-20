using System.Collections;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    public int hp;
    public int maxHp;
    public int strength;
    public string enemyName;
    public float speed;
    public float currentSpeed;
    public int scoreValue;
    public bool isFreezable;
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
    public float detectionRadius; // Radius for overlap detection
    public float separationStrength; // Force to push enemies apart
    public Animator anim;
    const string RUNNING = "isRunning";
    public bool facingRight = false;
    public LayerMask enemyLayer; // Layer to detect other enemies
    public enum EnemyLevel{
        basic,
        intermediate,
        advanced,
        legendary
    };

    private Vector2 knockbackDirection;
    private float knockbackDuration;
    private float knockbackSpeed;

    private bool isKnockedBack;

    void Awake()
    {
        ms = GameObject.Find("Spawner").GetComponent<MobSpawner>();
        playManager = GameObject.Find("PlaySystemManager").GetComponent<PlaySystemManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySo.CreateStats(gameObject);
        hp = maxHp;
        currentSpeed = speed;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<IPlayer>();
        playerPos = player.transform;
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
    }

    public void PreventOverlap()
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
            SoundManager.Instance.PlaySoundEffect(damagedClips[randInt], transform, 1.0f);
        }
        if (hp <= 0)
        {
            playManager.IncreaseScore(scoreValue);
            float randomInt = Random.Range(0f, 100.0f);
            dropLoot(randomInt);
            GetComponent<SpriteRenderer>().color = Color.white;
            currentSpeed = speed;
            hp = maxHp;
            foreach(Projectile arrow in GetComponentsInChildren<Projectile>().ToList())
            {
                arrow.gameObject.transform.parent = null;
                arrow.gameObject.SetActive(false);
            }
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

    public void Flip()
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
