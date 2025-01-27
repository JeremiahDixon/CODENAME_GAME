using System.Linq;
using UnityEngine;

public class Witch : Enemy
{
    public GameObject laserPrefab; // Laser prefab
    public Transform firePoint; // Fire point for shooting
    public float fireRate; // Time between shots
    public float edgePadding; // Margin from the screen edges
    public float idleDuration = 2f; // Time spent idling
    public float spawnRadius = 5f; // Radius for random movement from spawn point
    public float minPlayerDistance = 2f; // Minimum distance from the player
    public float avoidDistance = 1.5f; // Distance to move away from the player
    public float laserSpeed = 10f; // Speed of the laser projectile

    private float idleTimer = 0f; // Timer for idling
    private float fireCountdown; // Countdown for the next shot
    private Vector2 spawnPosition; // The initial spawn position
    private Vector2 targetPosition; // Current target position
    private GameObject laserInstance; // The single laser object
    private Laser laserScript; // Reference to the laser script
    private bool laserActive = false; // Whether the laser is active

    private enum State { MovingToPosition, Idle, AvoidingPlayer }
    private State currentState = State.MovingToPosition;

    void Start()
    {
        currentState = State.MovingToPosition;
        fireCountdown = fireRate;

        spawnPosition = transform.position;

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

        // Update laser state
        if (!laserInstance.activeInHierarchy && laserInstance != null)
        {
            laserActive = false;
        }
    }

    void MoveToTargetPosition()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isIdle", false);

        // If no target position or target reached, pick a new one
        if (targetPosition == Vector2.zero || Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = GetRandomValidPosition();
        }

        // Move towards the target position
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

        Vector2 directionAway = (transform.position - player.transform.position).normalized;
        Vector2 avoidTarget = (Vector2)transform.position + directionAway * avoidDistance;

        // Clamp avoid target within bounds and spawn radius
        avoidTarget = ClampToCameraBounds(ClampToSpawnRadius(avoidTarget));

        transform.position = Vector2.MoveTowards(transform.position, avoidTarget, speed * Time.deltaTime);

        // Lock Z-axis
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        // Return to Idle when sufficiently far from the player
        if (Vector2.Distance(transform.position, player.transform.position) > minPlayerDistance)
        {
            currentState = State.Idle;
            idleTimer = idleDuration;
        }
    }

    Vector2 GetRandomValidPosition()
    {
        Vector2 randomPosition;
        spawnPosition = transform.position;

        do
        {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;
            randomPosition = spawnPosition + offset;
            randomPosition = ClampToCameraBounds(randomPosition);
        } while (Vector2.Distance(randomPosition, player.transform.position) < minPlayerDistance);

        return randomPosition;
    }

    Vector2 ClampToSpawnRadius(Vector2 position)
    {
        Vector2 direction = position - spawnPosition;
        if (direction.magnitude > spawnRadius)
        {
            position = spawnPosition + direction.normalized * spawnRadius;
        }
        return position;
    }

    Vector2 ClampToCameraBounds(Vector2 position)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);
        viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0f + edgePadding, 1f - edgePadding);
        viewportPosition.y = Mathf.Clamp(viewportPosition.y, 0f + edgePadding, 1f - edgePadding);

        return Camera.main.ViewportToWorldPoint(viewportPosition);
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

            Vector2 direction = (player.transform.position - laserInstance.transform.position).normalized;

            if (laserScript != null)
            {
                laserScript.SetDirection(direction, laserSpeed);
            }

            laserActive = true;
        }
    }

    override public void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (hp <= 0)
        {
            ResetStateOnDeath();
        }
    }

    void ResetStateOnDeath()
    {
        hp = maxHp;
        transform.position = spawnPosition;
        currentState = State.MovingToPosition;
        laserInstance.SetActive(false);
    }
}
