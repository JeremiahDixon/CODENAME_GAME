using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject slime;
    public float timeBtwSpawn;
    public float startTimeBtwSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBtwSpawn <= 0){
            spawn();
            timeBtwSpawn = startTimeBtwSpawn;
        }else{
            timeBtwSpawn -= Time.deltaTime;
        }
    }

    void spawn(){
        int randomInt = Random.Range(1, 5);
        if(randomInt == 1){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(1.1f, 1.5f), 0));
            GameObject newSlime = Instantiate(slime, v3Pos, Quaternion.identity);
        }else if(randomInt == 2){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(1.1f, 1.5f), Random.Range(0.0f, 1.0f), 0));
            GameObject newSlime = Instantiate(slime, v3Pos, Quaternion.identity);
        }else if(randomInt == 3){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(0.0f, -0.5f), 0));
            GameObject newSlime = Instantiate(slime, v3Pos, Quaternion.identity);
        }else if(randomInt == 4){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, -0.5f), Random.Range(0.0f, 1f), 0));
            GameObject newSlime = Instantiate(slime, v3Pos, Quaternion.identity);
        }
    }

}
