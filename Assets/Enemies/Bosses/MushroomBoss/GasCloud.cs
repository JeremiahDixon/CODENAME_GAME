using UnityEngine;

public class GasCloud : MonoBehaviour
{
    float poisonDOT = 0;
    float poisonDOTStartTime = 2;
    int damage = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("CLOUD COLLISION");
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("PLAYER IN CLOUD");
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
