using Unity.VisualScripting;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public float knockbackForce = 2f;
    public float knockbackDuration = 0.15f;
    public int damage = 40; // Damage dealt by the hammer

    int roationSpeed = 1080;

    void Update()
    {
        transform.Rotate( Vector3.back.normalized * roationSpeed * Time.deltaTime );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object hit is an enemy
        if (collision.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Enemy>().CanBeKnockedBack)
            {
                Vector2 knockbackDirection = collision.transform.position - GameManager.Instance.thePlayer.transform.position;
                collision.gameObject.GetComponent<IEnemy>().ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
            }
            collision.GetComponent<IEnemy>().TakeDamage(damage);
        }
    }

    public void IncreaseDamage(int damage)
    {
        this.damage = damage;
    }

}
