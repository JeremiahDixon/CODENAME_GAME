using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class MushroomBoss : Enemy
{
    [SerializeField] private float timeBtwAttack;
    [SerializeField] Transform attackPos;
    //[SerializeField] float attackRange;
    public enum BossState {StageOne, StageTwo, StageThree, Transition}
    public BossState currentBossState {get; private set;}

    public enum StageOneAttack {MushroomAttack, SporeBombs}
    public StageOneAttack currentStageOneState {get; private set;}

    [SerializeField] GameObject gasCloudPrefab; // Assign your gas cloud prefab in the Inspector
    [SerializeField] int numberOfClouds = 5;    // Number of gas clouds to spawn

    Vector2 screenBoundsMin; // Minimum bounds of the screen in world coordinates
    Vector2 screenBoundsMax; // Maximum bounds of the screen in world coordinates
    float spawnGas = 5f;
    float startSpawnGas = 10f;

    float spawnMushrooms = 10f;
    float startSpawnMushrooms = 6f;

    float spawnBombs = 0f;
    float startSpawnBombs = 20f;

    public GameObject mushroomPrefab; // Assign your mushroom prefab in the Inspector
    public float spawnInterval = 0.5f; // Time between spawning mushrooms
    public float maxDistance = 20f; // Maximum distance for the line
    public float spawnOffset = 0.75f; // Distance between each mushroom
    public Transform spawnPoint; // Position from which mushrooms spawn
    public Transform ogSpawnPoint;
    private float currentDistance = 0f;


    public GameObject sporeBombPrefab; // Prefab of the spore bomb
    public float attackDuration;// = 30f; // How long the attack lasts
    public float bombSpawnInterval;// = 1f; // Time between spore bombs
    public float spawnRadius;// = 2f; // Maximum range around the player for targeting

    int mushroomAttackCount = 0;

    void Start()
    {
        currentBossState = BossState.StageOne;
        currentStageOneState = StageOneAttack.MushroomAttack;
        CalculateScreenBounds();
    }

    void Update()
    {
        if(spawnGas <= 0)
        {
            SpawnGasClouds();
            spawnGas = startSpawnGas;
        }
        else
        {
            spawnGas -= Time.deltaTime;
        }

        if(playerPos.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }else if(playerPos.position.x < transform.position.x && facingRight)
        {
            Flip();
        }
        if(currentBossState == BossState.StageOne)
        {
            if(playerPos.position.y - anim.transform.position.y > 0.2f || playerPos.position.y - anim.transform.position.y < -0.2f)
            {
                anim.transform.position = Vector2.MoveTowards(anim.transform.position, new Vector2(Camera.main.ViewportToWorldPoint(new Vector3(0.9f, playerPos.position.y, 0)).x, playerPos.position.y), currentSpeed * Time.deltaTime);
            }

            if(currentStageOneState == StageOneAttack.MushroomAttack)
            {
                if(spawnMushrooms <= 0)
                {
                    StartCoroutine(SpawnMushrooms());
                    mushroomAttackCount++;
                    spawnMushrooms = startSpawnMushrooms;
                    if(mushroomAttackCount >= 4)
                    {
                        currentStageOneState = StageOneAttack.SporeBombs;
                        mushroomAttackCount = 0;
                    }
                }
                else
                {
                    spawnMushrooms -= Time.deltaTime;
                }
            }
            
            if(currentStageOneState == StageOneAttack.SporeBombs)
            {
                if(spawnBombs <= 0)
                {
                    StartCoroutine(SporeBombAttack());
                    spawnBombs = startSpawnBombs;
                }
                else
                {
                    spawnBombs -= Time.deltaTime;
                }
            }

        }
    }
    
    void CalculateScreenBounds()
    {
        // Get the main camera
        Camera mainCamera = Camera.main;

        // Bottom-left corner of the screen in world coordinates
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));

        // Top-right corner of the screen in world coordinates
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        // Set screen bounds
        screenBoundsMin = new Vector2(bottomLeft.x, bottomLeft.y);
        screenBoundsMax = new Vector2(topRight.x, topRight.y);
    }

    void SpawnGasClouds()
    {
        for (int i = 0; i < numberOfClouds; i++)
        {
            // Randomly choose a position within the screen bounds
            Vector2 randomPosition = new Vector2(
                Random.Range(screenBoundsMin.x, screenBoundsMax.x),
                Random.Range(screenBoundsMin.y, screenBoundsMax.y)
            );

            // Instantiate the gas cloud at the random position
            GameObject gasCloud = Instantiate(gasCloudPrefab, randomPosition, Quaternion.identity);

            // Initialize the gas cloud's movement
            GasCloud gasCloudScript = gasCloud.GetComponent<GasCloud>();
            if (gasCloudScript != null)
            {
                gasCloudScript.Initialize(8f, Random.Range(1f, 2f));
            }
        }
    }

    IEnumerator SpawnMushrooms()
    {
        while (currentDistance < maxDistance)
        {
            // Spawn a mushroom
            GameObject mushroom = Instantiate(mushroomPrefab, spawnPoint.position, Quaternion.identity);

            // Move the spawn point forward by the offset
            spawnPoint.position += -spawnPoint.right * spawnOffset;

            currentDistance += spawnOffset;

            // Wait before spawning the next mushroom
            yield return new WaitForSeconds(spawnInterval);
        }

        // Reset for the next attack cycle
        currentDistance = 0f;
        spawnPoint.position = ogSpawnPoint.position; // Reset spawn point to boss position
    }

    private IEnumerator SporeBombAttack()
    {
        float elapsedTime = 0f;

        while (elapsedTime < attackDuration)
        {
            // Pick a random point near the player
            Vector2 targetPosition = playerPos.position + (Vector3)Random.insideUnitCircle * spawnRadius;

            // Spawn the spore bomb
            GameObject sporeBomb = Instantiate(sporeBombPrefab, ogSpawnPoint.position, Quaternion.identity);
            sporeBomb.GetComponent<SporeBomb>().Initialize(targetPosition);

            // Wait for the next spawn
            elapsedTime += bombSpawnInterval;
            yield return new WaitForSeconds(bombSpawnInterval);
        }

        currentStageOneState = StageOneAttack.MushroomAttack;
    }

    override public void TakeDamage (int damage)
    {
        hp -= damage;
        if(damagedClips.Length > 0)
        {
            int randInt = Random.Range(0, damagedClips.Length);
            SoundManager.Instance.PlaySoundEffect(damagedClips[randInt], transform, _soundFxVolume);
        }
        if(hp <= maxHp * 0.65f)
        {
            //currentStageOneState = StageOneAttack.SporeBombs;
            //currentBossState = BossState.StageTwo;
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
        }
    }
}
