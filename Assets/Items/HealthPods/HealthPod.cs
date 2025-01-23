using UnityEngine;

public class HealthPod : MonoBehaviour
{
    [SerializeField] int healAmount;
    const string PLAYER_TAG = "Player";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == PLAYER_TAG){
            other.gameObject.GetComponent<IPlayer>().Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
