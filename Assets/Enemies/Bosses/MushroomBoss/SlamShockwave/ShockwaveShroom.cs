using UnityEngine;

public class ShockwaveShroom : MonoBehaviour
{
    [SerializeField] float knockbackForce = 5f;
    [SerializeField] float knockbackDuration = 0.2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player hit by mushroom!");
            collision.GetComponent<IPlayer>().TakeDamage(2);
            // Vector2 knockbackDirection = collision.GetComponent<IPlayer>().transform.position - transform.position;
            // collision.GetComponent<IPlayer>().ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
        }
    }
}
