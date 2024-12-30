using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other){
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        transform.parent = other.transform;
        if(other.gameObject.CompareTag("Enemy")){
            other.gameObject.GetComponent<IEnemy>().TakeDamage(4);
        }
        StartCoroutine(DestroyAfterDelay(5));
    }

    private IEnumerator DestroyAfterDelay(int seconds)
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
