using UnityEngine;

public class EntryPoints : MonoBehaviour
{
    GameManager gameManager;
    public GameObject[] spawnPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
            if(gameObject.name == "ToForest"){
                SceneLoader.Instance.LoadSceneAsync("Scene2");
            }
        }
    }
}
