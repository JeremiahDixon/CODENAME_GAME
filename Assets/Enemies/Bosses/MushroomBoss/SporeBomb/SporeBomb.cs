using System.Collections;
using UnityEngine;

public class SporeBomb : MonoBehaviour
{
    public float speed;// = 5f; // Horizontal movement speed
    public float bounceHeight;// = 2f; // Initial bounce height
    public float bounceDamping;// = 0.8f; // Damping factor for bounce height
    public float flashDuration;// = 2f; // Flashing time before explosion
    public float explosionRadius;// = 3f; // Radius of the explosion
    public int damage;// = 20; // Damage dealt to the player
    public LayerMask playerLayer; // Layer to detect the player

    private Vector2 targetPosition;
    private SpriteRenderer spriteRenderer;
    Animator anim;

    public void Initialize(Vector2 target)
    {
        targetPosition = target;
        StartCoroutine(MoveAndBounce());
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private IEnumerator MoveAndBounce()
    {
        Vector2 startPosition = transform.position;
        float elapsedTime = 0f;

        float totalDistance = Vector2.Distance(startPosition, targetPosition);
        float travelTime = totalDistance / speed; // Total time to reach the target based on speed
        float bounceHeight = this.bounceHeight; // Initial bounce height
        // Scale the number of bounces based on the total distance
        int minBounces = 1; // Minimum number of bounces
        int maxBounces = 6; // Maximum number of bounces
        int bounceCount = Mathf.Clamp(Mathf.FloorToInt(totalDistance / 2f), minBounces, maxBounces); // Number of major bounces
        Debug.Log("bounceCount: " + bounceCount);
        if(bounceCount <= 1)
        {
            bounceHeight = 2;
        }else if(bounceCount <= 3)
        {
            bounceHeight = 2.5f;
        }else if(bounceCount <= 5)
        {
            bounceHeight = 3;
        }else if(bounceCount <= 6)
        {
            bounceHeight = 3.5f;
        }
        // Calculate bounce duration and horizontal travel per bounce
        float timePerBounce = travelTime / bounceCount;

        while (elapsedTime < travelTime)
        {
            // Determine normalized time (0 to 1) along the entire path
            float t = elapsedTime / travelTime;

            // Linear interpolation for the ground path
            Vector2 groundPathPosition = Vector2.Lerp(startPosition, targetPosition, t);

            // Calculate the bounce offset (exponential decay)
            float bounceProgress = (elapsedTime % timePerBounce) / timePerBounce; // Progress within the current bounce
            float bounceOffset = Mathf.Sin(bounceProgress * Mathf.PI) * bounceHeight; // Creates the bounce arc

            // Apply exponential damping to the bounce height
            bounceHeight = Mathf.Lerp(this.bounceHeight, 0f, t * bounceDamping);

            // Combine ground path with bounce offset
            Vector2 currentPosition = groundPathPosition;
            currentPosition.y += bounceOffset;

            // Update position
            transform.position = currentPosition;

            // Increment time
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position is exactly the target position
        transform.position = targetPosition;

        // Start flashing and explosion phase
        StartCoroutine(FlashAndExplode());
    }

    private IEnumerator FlashAndExplode()
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
            if(elapsedTime >= flashDuration * 0.6f)
            {
                anim.SetBool("isExplode", true);
            }
            yield return new WaitForSeconds(currentFlashSpeed);
        }

        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
        Collider2D hit = Physics2D.OverlapCircle(transform.position, explosionRadius, playerLayer);
        if (hit != null && hit.CompareTag("Player"))
        {
            hit.GetComponent<IPlayer>()?.TakeDamage(damage);
        }
        bool idk = true;
        while (idk)
        {
            idk = false;
            yield return new WaitForSeconds(3);
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IPlayer>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    
}