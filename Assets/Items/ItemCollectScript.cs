using UnityEngine;

public class ItemCollectScript : MonoBehaviour
{
    private bool isAttracted = false;       // Whether the item is moving toward the player
    private Transform player;               // Reference to the player's transform
    public float attractionRadius = 3f;     // Radius within which the item moves toward the player
    public float attractionSpeed = 5f;      // Speed at which the item moves toward the player

    void Start()
    {
        player = GameManager.Instance.thePlayer.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttracted)
        {
            CheckPlayerProximity();
        }
        else
        {
            MoveTowardPlayer();
        }
    }

    void CheckPlayerProximity()
    {
        if (player != null)
        {
            // Check if the player is within the attraction radius
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= attractionRadius)
            {
                isAttracted = true;
            }
        }
    }
    void MoveTowardPlayer()
    {
        if (player != null)
        {
            // Move toward the player's position
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            transform.position += directionToPlayer * attractionSpeed * Time.deltaTime;

        }
    }
}
