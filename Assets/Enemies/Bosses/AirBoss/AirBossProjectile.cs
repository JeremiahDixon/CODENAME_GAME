using UnityEngine;

public class AirBossProjectile : Projectile
{
    public Rigidbody2D rb;
    const string PLAYER_TAG = "Player";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag(PLAYER_TAG)){
            rb.linearVelocity = Vector2.zero;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            other.gameObject.GetComponent<IPlayer>().TakeDamage(damage);
            this.gameObject.SetActive(false);
            Debug.Log("Dealing x damage: " + damage);
        }
    }
}
