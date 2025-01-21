using UnityEngine;

public class Hammer : MonoBehaviour
{
    public int damage = 10; // Damage dealt by the hammer

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object hit is an enemy
        if (collision.CompareTag("Enemy"))
        {
            // Example: Apply damage to the enemy (assuming the enemy has a Health script)
            collision.GetComponent<IEnemy>().TakeDamage(damage);
        }
    }

}
