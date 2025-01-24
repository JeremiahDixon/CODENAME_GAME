using UnityEngine;

public class DroppedItemBounce : MonoBehaviour
{
    public float bounceHeight = 0.4f;          // The maximum height of the bounce
    public float bounceDuration = 0.3f;     // The duration of the bounce (up and down)
    public float horizontalSpeed = 1f;      // The initial speed of horizontal movement
    public Vector2 randomDirectionRange = new Vector2(-0.8f, 0.8f); // Random horizontal movement range
    public float stopThreshold = 0.1f;      // Threshold for stopping movement
    public float deceleration = 1.5f;         // Rate at which horizontal speed decreases
    private Vector2 moveDirection;          // The direction of movement
    private float bounceTimer;              // Tracks time for the bounce effect
    private bool isMoving = true;           // Tracks whether the item is still moving
    private Vector3 originalPosition;       // Tracks the original position of the item

    void Start()
    {
        // Set a random horizontal direction for movement
        float randomX = Random.Range(randomDirectionRange.x, randomDirectionRange.y);
        float randomY = Random.Range(randomDirectionRange.x, randomDirectionRange.y);
        moveDirection = new Vector2(randomX, randomY).normalized;

        // Initialize the bounce timer
        bounceTimer = 0f;
        originalPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            // Update bounce effect
            bounceTimer += Time.deltaTime;
            float progress = (bounceTimer % bounceDuration) / bounceDuration;

            // Calculate the vertical bounce (smooth curve using Mathf.Sin)
            float bounceOffset = Mathf.Sin(progress * Mathf.PI) * bounceHeight;

            // Update horizontal movement with deceleration
            horizontalSpeed = Mathf.Max(0f, horizontalSpeed - deceleration * Time.deltaTime);
            Vector3 horizontalMovement = new Vector3(moveDirection.x, moveDirection.y, 0f) * horizontalSpeed * Time.deltaTime;

            // Combine horizontal movement and bounce offset
            transform.position = originalPosition + horizontalMovement + new Vector3(0f, bounceOffset, 0f);

            // Update the original position to match the horizontal movement
            originalPosition += horizontalMovement;

            // Check if movement has stopped
            if (horizontalSpeed <= stopThreshold)
            {
                isMoving = false; // Stop movement logic
            }
        }
    }
}
