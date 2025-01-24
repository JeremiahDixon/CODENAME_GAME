using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBoss : Enemy
{
    [SerializeField] private float timeBtwAttack;
    [SerializeField] Transform attackPos;
    [SerializeField] float attackRange;
    public enum BossState { StageOne, StageTwo, StageThree, Transition}
    public BossState currentBossState {get; private set;}

    [SerializeField] GameObject gasCloudPrefab; // Assign your gas cloud prefab in the Inspector
    [SerializeField] int numberOfClouds = 5;    // Number of gas clouds to spawn

    private Vector2 screenBoundsMin; // Minimum bounds of the screen in world coordinates
    private Vector2 screenBoundsMax; // Maximum bounds of the screen in world coordinates

    void Start()
    {
        CalculateScreenBounds();
        SpawnGasClouds();
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
                gasCloudScript.Initialize(10f, Random.Range(1f, 3f)); // Duration: 10s, speed: random between 1-3
            }
        }
    }
}
