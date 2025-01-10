using System.Collections;
using UnityEngine;

public class Arrow : Projectile
{
    public Rigidbody2D rb;
    private float timeToDestroy = 20.0f;
    const string ENEMY_TAG = "Enemy";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToDestroy <= 0){
            StartCoroutine(DestroyAfterDelay(1));
        }else{
            timeToDestroy -= Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        transform.parent = other.transform;
        if(other.gameObject.CompareTag(ENEMY_TAG)){
            other.gameObject.GetComponent<IEnemy>().TakeDamage(damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.damageModifier));
            Debug.Log("Dealing x damage: " + (damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.damageModifier)));
            Destroy(gameObject);
        }else{
            transform.parent = other.transform;
            StartCoroutine(DestroyAfterDelay(5));
        }
    }

    public IEnumerator DestroyAfterDelay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
