using System.Collections;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public GameObject mushroomPrefab; // Prefab for the mushroom
    public int mushroomsPerRing = 8; // Number of mushrooms per ring
    public int totalRings = 3; // Number of rings to spawn
    public float initialRadius = 2f; // Radius of the first ring
    public float radiusIncrement = 2f; // Increase in radius for each subsequent ring
    public float growthDuration = 1f; // Time for mushrooms to grow
    public float shrinkDuration = 1f; // Time for mushrooms to shrink
    public float delayBetweenRings = 0.5f; // Delay before the next ring spawns

    public void TriggerShockwave()
    {
        StartCoroutine(SpawnMushroomRings());
    }

    private IEnumerator SpawnMushroomRings()
    {
        for (int ring = 0; ring < totalRings; ring++)
        {
            float radius = initialRadius + ring * radiusIncrement;

            // Spawn mushrooms in a ring
            SpawnRingOfMushrooms(radius);

            // Wait before spawning the next ring
            yield return new WaitForSeconds(delayBetweenRings);
        }
    }

    private void SpawnRingOfMushrooms(float radius)
    {
        for (int i = 0; i < mushroomsPerRing; i++)
        {
            // Calculate mushroom position in a circle
            float angle = i * Mathf.PI * 2f / mushroomsPerRing; // Divide full circle into even segments
            Vector3 position = transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

            // Spawn a mushroom
            GameObject mushroom = Instantiate(mushroomPrefab, position, Quaternion.identity);

            // Handle mushroom growth and shrinking
            StartCoroutine(AnimateMushroom(mushroom));
        }
    }

    private IEnumerator AnimateMushroom(GameObject mushroom)
    {
        Transform mushroomTransform = mushroom.transform;

        // Growing phase
        float elapsed = 0f;
        while (elapsed < growthDuration)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(0f, 1f, elapsed / growthDuration);
            mushroomTransform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        // Shrinking phase
        elapsed = 0f;
        while (elapsed < shrinkDuration)
        {
            elapsed += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 0f, elapsed / shrinkDuration);
            mushroomTransform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        // Destroy the mushroom after shrinking
        Destroy(mushroom);
    }
}
