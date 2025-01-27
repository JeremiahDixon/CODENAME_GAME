using System.Collections;
using UnityEngine;

public class GasCloud : MonoBehaviour
{
    float poisonDOT = 0;
    float poisonDOTStartTime = 2;
    int damage = 5;
    private float moveSpeed = 1f; // Speed of horizontal movement
    private float lifetime = 10f; // Lifetime of the gas cloud

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(float duration, float speed)
    {
        lifetime = duration;
        moveSpeed = speed;
        StartCoroutine(MoveAndDestroy());
    }

    IEnumerator MoveAndDestroy()
    {
        float elapsed = 0f;

        while (elapsed < lifetime)
        {
            // Move horizontally over time
            transform.Translate(-Vector2.right * moveSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Destroy the gas cloud after its lifetime
        Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("PLAYER IN CLOUD");
            if(poisonDOT <= 0)
            {
                other.gameObject.GetComponent<IPlayer>().TakeDamage(damage);
                poisonDOT = poisonDOTStartTime;
            }else
            {
                poisonDOT -= Time.deltaTime;
            }
        }
    }
}
