using UnityEngine;

public class SporeBombGas : MonoBehaviour
{
    float poisonDOT = 3;
    float poisonDOTStartTime = 3;
    int damage = 5;
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
