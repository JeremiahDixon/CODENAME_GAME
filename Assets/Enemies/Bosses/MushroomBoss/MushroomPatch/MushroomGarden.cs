using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomGarden : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public Transform spawnPoint; // The point where enemies will spawn
    public float spawnInterval = 0.5f; // Time interval between each spawn (half second)
    public float spawnDurationFactor = 1f; // Factor to scale spawn duration based on mushrooms left
    public float spawnCycleInterval = 30f; // Time between each spawn cycle (in seconds)

    private Queue<GameObject> pool = new Queue<GameObject>(); // The object pool for enemies
    private List<GameObject> activeEnemies = new List<GameObject>(); // List of active spawned enemies
    int initialCount = 15;

    void Start()
    {
        // Initialize the object pool for enemies with an initial pool size of 10
        CreatePool();

        // Start the child activation process, and then start spawning enemies once it's finished
        StartCoroutine(ActivateChildrenAndStartSpawning());
    }

    private IEnumerator ActivateChildrenAndStartSpawning()
    {
        // Activate each child (mushroom) one by one with a delay of 0.5 seconds
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.12f); // Wait 0.5 seconds before activating the next one
        }

        // Once all mushrooms are activated, start spawning enemies
        StartCoroutine(PeriodicSpawning());
    }

    private IEnumerator PeriodicSpawning()
    {
        // This will loop indefinitely, spawning enemies every spawnCycleInterval
        while (true)
        {
            // If there are mushrooms left, start spawning
            if (transform.childCount > 0)
            {
                yield return StartCoroutine(SpawnEnemiesForDuration());
            }
            else
            {
                // No mushrooms left, destroy the patch and clean up remaining enemies
                CleanUpEnemies();
                DestroyPatch();
                yield break;
            }

            // Wait for the next spawn cycle
            yield return new WaitForSeconds(spawnCycleInterval);
        }
    }

    private IEnumerator SpawnEnemiesForDuration()
    {
        // Get the number of mushrooms left in the patch (number of child objects)
        int remainingMushrooms = transform.childCount;

        // Calculate spawn duration based on how many mushrooms are left
        float spawnDuration = Mathf.Max(remainingMushrooms * spawnDurationFactor, 1f); // Ensure at least 1 second of spawning

        // Start spawning enemies for the calculated duration
        float elapsedTime = 0f;
        while (elapsedTime < spawnDuration)
        {
            // Spawn an enemy from the object pool
            GameObject enemy = GetObject();
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);

            // Add the enemy to the active list
            activeEnemies.Add(enemy);

            // Increment elapsed time
            elapsedTime += spawnInterval;

            // Wait for the next spawn interval
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void DestroyPatch()
    {
        // Optionally, add an effect or sound here before destroying the patch
        Destroy(gameObject); // Destroy the patch GameObject
    }

    private void CleanUpEnemies()
    {
        // Return all remaining active enemies to the pool and deactivate them
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(false); // Deactivate the enemy
                ReturnObject(enemy); // Return it to the pool
            }
        }

        // Clear the list of active enemies
        activeEnemies.Clear();
    }

    void CreatePool()
    {
        for (int i = 0; i < initialCount; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            obj.SetActive(false); // Make sure they are initially inactive
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        // If the pool is empty, create a new object
        if (pool.Count == 0)
        {
            GameObject obj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            obj.SetActive(false);
            return obj;
        }
        else
        {
            // Get an object from the pool and activate it
            GameObject obj = pool.Dequeue();
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false); // Deactivate the object
        pool.Enqueue(obj); // Return it to the pool
    }
}

// public class ObjectPool
// {
//     private GameObject prefab;
//     private Queue<GameObject> pool;

//     public ObjectPool(GameObject prefab, int initialCount)
//     {
//         this.prefab = prefab;
//         pool = new Queue<GameObject>();

//         // Create initial objects and add them to the pool
//         for (int i = 0; i < initialCount; i++)
//         {
//             GameObject obj = Object.Instantiate(prefab);
//             obj.SetActive(false); // Make sure they are initially inactive
//             pool.Enqueue(obj);
//         }
//     }

//     public GameObject GetObject()
//     {
//         // If the pool is empty, create a new object
//         if (pool.Count == 0)
//         {
//             GameObject obj = Object.Instantiate(prefab);
//             obj.SetActive(false);
//             return obj;
//         }
//         else
//         {
//             // Get an object from the pool and activate it
//             GameObject obj = pool.Dequeue();
//             return obj;
//         }
//     }

//     public void ReturnObject(GameObject obj)
//     {
//         obj.SetActive(false); // Deactivate the object
//         pool.Enqueue(obj); // Return it to the pool
//     }
//}