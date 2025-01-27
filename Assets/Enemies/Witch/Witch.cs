using System.Linq;
using UnityEngine;

public class Witch : Enemy
{
    public GameObject laserPrefab; // Laser prefab
    public Transform firePoint; // Fire point for shooting
    public float fireRate; // Time between shots
    public float edgePadding; // Margin from the screen edges
    public float idleDuration = 2f; // Time spent idling
    private float idleTimer = 0f; // Timer for idling
    private float fireCountdown; // Countdown for the next shot
    public float laserSpeed = 10f; // Speed of the laser projectile

    private GameObject laserInstance; // The single laser object
    private Laser laserScript; // Reference to the laser script
    private bool laserActive = false; // Whether the laser is active

    private enum State { MovingToPosition, Idle, AvoidingPlayer }
    private State currentState = State.MovingToPosition;

    public float minPlayerDistance = 2f; // Minimum distance from the player
    public float avoidDistance = 1.5f; // Distance to move away from the player

    void Start()
    {
        currentState = State.MovingToPosition;
        fireCountdown = fireRate;

        // Instantiate the laser once and keep it disabled
        if (laserPrefab != null)
        {
            laserInstance = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
            laserInstance.SetActive(false); // Disable it initially
            laserScript = laserInstance.GetComponent<Laser>();
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case State.MovingToPosition:
                MoveToTargetPosition();
                break;
            case State.Idle:
                Idle();
                break;
            case State.AvoidingPlayer:
                AvoidPlayer();
                break;
        }

        // Ensure the witch's Z position stays locked
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        // Update the laser position if it is active
        if (!laserInstance.activeInHierarchy && laserInstance != null)
        {
            laserActive = false;
        }
    }

    void MoveToTargetPosition()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isIdle", false);

        Vector2 targetPosition = GetRandomValidPosition();
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Lock Z-axis
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentState = State.Idle;
            idleTimer = idleDuration;
        }
    }

    void Idle()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isIdle", true);

        idleTimer -= Time.deltaTime;
        HandleShooting();

        // Check distance to player
        if (Vector2.Distance(transform.position, player.transform.position) < avoidDistance)
        {
            currentState = State.AvoidingPlayer;
        }

        if (idleTimer <= 0f)
        {
            currentState = State.MovingToPosition;
        }
    }

    void AvoidPlayer()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isIdle", false);

        // Calculate direction directly away from the player
        Vector2 directionAway = (transform.position - player.transform.position).normalized;
        Vector2 targetPosition = (Vector2)transform.position + directionAway * avoidDistance;

        // Clamp the target position to stay within the camera bounds
        targetPosition = ClampToCameraBounds(targetPosition);

        // Move towards the clamped target position
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Lock Z-axis
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        // Return to Idle if sufficiently far from the player
        if (Vector2.Distance(transform.position, player.transform.position) > minPlayerDistance)
        {
            currentState = State.Idle;
            idleTimer = idleDuration;
        }
    }

    Vector2 GetRandomValidPosition()
    {
        Vector2 randomPosition;
        do
        {
            float x = Random.Range(0f + edgePadding, 1f - edgePadding);
            float y = Random.Range(0f + edgePadding, 1f - edgePadding);
            randomPosition = Camera.main.ViewportToWorldPoint(new Vector2(x, y));
        } while (Vector2.Distance(randomPosition, player.transform.position) < minPlayerDistance);

        // Lock Z-axis
        return new Vector3(randomPosition.x, randomPosition.y, 0);
    }

    Vector2 ClampToCameraBounds(Vector2 position)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);
        viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0f + edgePadding, 1f - edgePadding);
        viewportPosition.y = Mathf.Clamp(viewportPosition.y, 0f + edgePadding, 1f - edgePadding);

        Vector3 clampedPosition = Camera.main.ViewportToWorldPoint(viewportPosition);

        // Lock Z-axis
        return new Vector3(clampedPosition.x, clampedPosition.y, 0);
    }

    void HandleShooting()
    {
        fireCountdown -= Time.deltaTime;

        if (fireCountdown <= 0f && currentState == State.Idle && laserInstance != null && !laserActive)
        {
            anim.SetTrigger("isShooting");
            fireCountdown = fireRate;
        }
    }

    void Shoot()
    {
        if (laserInstance != null && player != null)
        {
            laserInstance.SetActive(true);
            laserInstance.transform.position = firePoint.position;

            // Calculate direction to the player
            Vector2 direction = (player.transform.position - laserInstance.transform.position).normalized;

            // Set direction and speed using a custom laser script
            if (laserScript != null)
            {
                laserScript.SetDirection(direction, laserSpeed);
            }

            laserActive = true;
        }
    }

    override public void TakeDamage(int damage)
    {
        hp -= damage;
        if (damagedClips.Length > 0)
        {
            int randInt = Random.Range(0, damagedClips.Length);
            SoundManager.Instance.PlaySoundEffect(damagedClips[randInt], transform, _soundFxVolume);
        }
        if (hp <= 0)
        {
            playManager.IncreaseScore(scoreValue);
            if (loot.Length > 0)
            {
                float randomInt = Random.Range(0f, 100.0f);
                dropLoot(randomInt);
            }
            GetComponent<SpriteRenderer>().color = Color.white;
            currentSpeed = speed;
            hp = maxHp;
            foreach (Projectile projectile in GetComponentsInChildren<Projectile>().ToList())
            {
                projectile.gameObject.transform.parent = null;
                projectile.gameObject.SetActive(false);
            }
            laserInstance.SetActive(false);
            ms.RequeueMob(this.gameObject);
        }
    }
}
