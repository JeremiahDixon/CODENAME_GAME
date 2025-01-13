using System.Collections;
using UnityEngine;

public class Arrow : Projectile
{
    public Rigidbody2D rb;
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
        rb.linearVelocity = Vector2.zero;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        if(other.gameObject.CompareTag(ENEMY_TAG)){
            transform.parent = other.transform;
            other.gameObject.GetComponent<IEnemy>().TakeDamage(damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.damageModifier));
            Debug.Log("Dealing x damage: " + (damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.damageModifier)));
        }else{
            transform.parent = other.transform;
        }
    }

}
