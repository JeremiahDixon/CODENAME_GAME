using System.Collections;
using UnityEngine;

public class MushroomPatch : Damagable
{
    public float health; // The starting health of the mushroom
    public float shakeDuration; // Duration of the shake effect
    public float shakeAmount; // Amount of shake movement
    private Vector3 originalPosition; // To store the mushroom's original position

    // This will store the current health
    private float currentHealth;

    public float minScale; // Minimum scale size during bounce
    public float secMaxScale; // Maximum scale size
    public float maxScale; // Maximum scale size
    private Vector3 initialScale; // The initial scale of the mushroom

    public float growthDuration; // Time to grow to maxScale
    public float oscillationDuration; // Duration of the bouncy oscillation phase before settling

    private void Start()
    {
        currentHealth = health;
        originalPosition = transform.position;

        initialScale = transform.localScale; // Store the initial scale
        // Start the growth and bounce coroutine
        StartCoroutine(GrowAndBounce());
    }

    // Call this method whenever the mushroom takes damage
    override public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Trigger the shake effect
        Shake();

        // If the mushroom's health is below the threshold, destroy it
        if (currentHealth <= 0)
        {
            DestroyMushroom();
        }
    }

    private void Shake()
    {
        // Start the shake effect by moving the mushroom around
        StopAllCoroutines(); // Stop any existing shakes
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            // Shake the mushroom by slightly altering its position
            float shakeX = Random.Range(-shakeAmount, shakeAmount);
            float shakeY = Random.Range(-shakeAmount, shakeAmount);
            transform.position = originalPosition + new Vector3(shakeX, shakeY, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Return the mushroom to its original position after shaking
        transform.position = originalPosition;
    }

    private void DestroyMushroom()
    {
        // You can add a destroy effect or sound here before destroying the mushroom
        Destroy(gameObject); // Destroy the mushroom object
    }

    private IEnumerator GrowAndBounce()
    {
        // Phase 1: Quickly grow from the initial scale to maxScale
        float elapsedTime = 0f;
        while (elapsedTime < growthDuration)
        {
            float scale = Mathf.Lerp(initialScale.x, maxScale, elapsedTime / growthDuration);
            transform.localScale = new Vector3(scale, scale, 1); // Keep Z scale as 1 for 2D
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is exactly maxScale
        transform.localScale = new Vector3(maxScale, maxScale, 1);

        // Phase 2: Apply a bouncy oscillation between maxScale and minScale
        elapsedTime = 0f;
        while (elapsedTime < oscillationDuration)
        {
            // Oscillation effect (smooth bounce)
            float t = Mathf.Sin(elapsedTime * Mathf.PI * 2 / oscillationDuration); // Creates a sine wave
            float scale = Mathf.Lerp(minScale, secMaxScale, (t + 1) / 2); // Convert sine wave to scale range
            transform.localScale = new Vector3(scale, scale, 1); // Keep Z scale as 1 for 2D
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Phase 3: Smooth transition to scale 1 (final resting size)
        while (transform.localScale.x > 1f)
        {
            float scale = Mathf.Lerp(transform.localScale.x, 1f, 2 / oscillationDuration); // Smooth transition to 1
            transform.localScale = new Vector3(scale, scale, 1); // Keep Z scale as 1 for 2D
            yield return null;
        }

        // Ensure the final scale is exactly 1
        transform.localScale = new Vector3(1f, 1f, 1);
    }
}