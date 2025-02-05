using UnityEngine;

public class Witch : Enemy
{
    public GameObject laserPrefab;
    public Transform firePoint;
    public float fireRate;
    public float edgePadding;
    public float idleDuration = 2f;
    public float minPlayerDistance = 2f;
    public float avoidDistance = 1.5f;
    public float laserSpeed = 7f;
    public LayerMask terrainLayer;
    public float terrainCheckRadius = 1f;

    private float idleTimer = 0f;
    private float fireCountdown;
    private Vector2 targetPosition;
    private GameObject laserInstance;
    private Laser laserScript;
    private bool laserActive = false;

    private enum State { MovingToPosition, Idle, AvoidingPlayer }
    private State currentState = State.MovingToPosition;

    void Start()
    {
        currentState = State.MovingToPosition;
        fireCountdown = fireRate;

        if (laserPrefab != null)
        {
            laserInstance = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
            laserInstance.SetActive(false);
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

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (!laserInstance.activeInHierarchy && laserInstance != null)
        {
            laserActive = false;
        }
    }

    void MoveToTargetPosition()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isIdle", false);

        if (targetPosition == Vector2.zero || Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = GetValidPositionInView();
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

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

        if (Vector2.Distance(transform.position, player.transform.position) < avoidDistance)
        {
            currentState = State.AvoidingPlayer;
        }

        if (idleTimer <= 0f || !IsInCameraView(transform.position))
        {
            currentState = State.MovingToPosition;
        }
    }

    void AvoidPlayer()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isIdle", false);

        targetPosition = GetValidPositionInView();
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, player.transform.position) > minPlayerDistance)
        {
            currentState = State.Idle;
            idleTimer = idleDuration;
        }
    }

    Vector2 GetValidPositionInView()
    {
        Vector2 randomPosition;
        do
        {
            randomPosition = (Vector2)transform.position + Random.insideUnitCircle * 5f;
            randomPosition = ClampToCameraBounds(randomPosition);
        }
        while (Vector2.Distance(randomPosition, player.transform.position) < minPlayerDistance || IsNearTerrain(randomPosition));

        return randomPosition;
    }

    bool IsInCameraView(Vector2 position)
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(position);
        return viewportPos.x > 0f && viewportPos.x < 1f && viewportPos.y > 0f && viewportPos.y < 1f;
    }

    bool IsNearTerrain(Vector2 position)
    {
        return Physics2D.OverlapCircle(position, terrainCheckRadius, terrainLayer) != null;
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
        if (!IsInCameraView(transform.position)) return;
        
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
        currentState = State.MovingToPosition;
        laserInstance.SetActive(false);
    }
}
