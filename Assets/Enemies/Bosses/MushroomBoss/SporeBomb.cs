using UnityEngine;

public class SporeBomb : MonoBehaviour
{
    public float speed = 5f; // Horizontal movement speed
    public float bounceHeight = 2f; // Initial bounce height
    public float bounceDamping = 0.8f; // Damping factor for bounce height
    public float flashDuration = 2f; // Flashing time before explosion
    public float explosionRadius = 3f; // Radius of the explosion
    public int damage = 20; // Damage dealt to the player
    public LayerMask playerLayer; // Layer to detect the player

    private Vector2 targetPosition;
    private SpriteRenderer spriteRenderer;

    public void Initialize(Vector2 target)
    {
        targetPosition = target;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(MoveAndBounce());
    }

    private System.Collections.IEnumerator MoveAndBounce()
    {
        Vector2 startPosition = transform.position;
        float elapsedTime = 0f;
        float bounceHeight = this.bounceHeight; // Initial bounce height
        float verticalSpeed = 0f; // Vertical speed for the bounce
        float gravity = -9.8f; // Gravity-like force for the bounce
        Vector2 currentPosition = startPosition;

        float maxTravelTime = 10f; // Failsafe: maximum time allowed for movement
        float startDistance = Vector2.Distance(startPosition, targetPosition);

        while (Vector2.Distance(currentPosition, targetPosition) > 0.1f && elapsedTime < maxTravelTime)
        {
            // Move horizontally toward the target
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            currentPosition += direction * speed * Time.deltaTime;

            // Apply vertical bounce physics
            verticalSpeed += gravity * Time.deltaTime; // Gravity reduces vertical speed
            currentPosition.y += verticalSpeed * Time.deltaTime;

            // Check if it hits the "ground" (y <= original ground level)
            if (currentPosition.y <= startPosition.y)
            {
                currentPosition.y = startPosition.y; // Snap to ground level
                verticalSpeed = Mathf.Sqrt(2 * Mathf.Abs(gravity) * bounceHeight); // Reset vertical speed for next bounce
                bounceHeight *= bounceDamping; // Reduce bounce height for the next bounce
            }

            // Update position
            transform.position = currentPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Final position adjustment in case it didn't perfectly align with the target
        transform.position = targetPosition;

        // Safety check: Ensure the bomb moves into the flashing phase
        if (elapsedTime >= maxTravelTime)
        {
            Debug.LogWarning("MoveAndBounce timed out before reaching the target position.");
        }

        StartCoroutine(FlashAndExplode());
    }

    private System.Collections.IEnumerator FlashAndExplode()
    {
        float elapsedTime = 0f;
        bool isRed = false;

        while (elapsedTime < flashDuration)
        {
            isRed = !isRed;
            spriteRenderer.color = isRed ? Color.red : Color.white;

            // Dynamically adjust flashing speed as it gets closer to explosion
            float t = elapsedTime / flashDuration;
            float currentFlashSpeed = Mathf.Lerp(0.5f, 0.1f, t);

            elapsedTime += currentFlashSpeed;
            yield return new WaitForSeconds(currentFlashSpeed);
        }

        Explode();
    }

    private void Explode()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, explosionRadius, playerLayer);
        if (hit != null && hit.CompareTag("Player"))
        {
            hit.GetComponent<IPlayer>()?.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}