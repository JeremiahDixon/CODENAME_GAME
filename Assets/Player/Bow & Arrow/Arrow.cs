using System.Collections;
using UnityEngine;

public class Arrow : Projectile
{
    protected Rigidbody2D rb;
    protected float knockbackForce = 2f;
    protected float knockbackDuration = 0.15f;
    const string ENEMY_TAG = "Enemy";
    const string TERRAIN_TAG = "Terrain";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag(ENEMY_TAG)){
            rb.linearVelocity = Vector2.zero;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            transform.parent = other.transform;
            if (other.gameObject.GetComponent<Enemy>().CanBeKnockedBack)
            {
                Vector2 knockbackDirection = other.transform.position - GameManager.Instance.thePlayer.transform.position;
                other.gameObject.GetComponent<IEnemy>().ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
            }
            other.gameObject.GetComponent<IEnemy>().TakeDamage(damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.DamageModifier));
            Debug.Log("Dealing x damage: " + (damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.DamageModifier)));
        }else if(other.gameObject.CompareTag(TERRAIN_TAG)){
            rb.linearVelocity = Vector2.zero;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            transform.parent = other.transform;
        }else if(other.gameObject.GetComponent<Damagable>() != null){
            other.gameObject.GetComponent<Damagable>().TakeDamage(damage);
            rb.linearVelocity = Vector2.zero;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            transform.parent = other.transform;
        }
    }

}
