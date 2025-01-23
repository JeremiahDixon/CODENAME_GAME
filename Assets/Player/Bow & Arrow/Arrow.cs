using System.Collections;
using UnityEngine;

public class Arrow : Projectile
{
    public Rigidbody2D rb;
    public float knockbackForce = 2f;
    public float knockbackDuration = 0.15f;
    // private float timeToDestroy = 20.0f;
    const string ENEMY_TAG = "Enemy";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
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
            other.gameObject.GetComponent<IEnemy>().TakeDamage(damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.damageModifier));
            Debug.Log("Dealing x damage: " + (damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.damageModifier)));
        }else if(other.gameObject.CompareTag("Terrain")){
            rb.linearVelocity = Vector2.zero;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            transform.parent = other.transform;
        }
    }

}
