using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField] int goldAmount;
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
            other.gameObject.GetComponent<IPlayer>().GainGold(goldAmount);
            Destroy(gameObject);
        }
    }

}
