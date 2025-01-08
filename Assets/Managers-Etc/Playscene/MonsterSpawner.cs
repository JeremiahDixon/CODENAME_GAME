using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    string mob1;
    [SerializeField]
    string mob2;
    [SerializeField]
    string mob3;
    [SerializeField]
    string mob4;
    [SerializeField]
    int mob1Int = 0;
    [SerializeField]
    int mob2Int = 0;
    [SerializeField]
    int mob3Int = 0;
    [SerializeField]
    int mob4Int = 0;
    //public static MonsterSpawner Instance;
    [SerializeField]
    GameObject[] mobs;
    public float basicTimeBtwSpawn;
    [SerializeField]
    private float startBasicTimeBtwSpawn;
    [SerializeField]
    public float intermediateTimeBtwSpawn;
    [SerializeField]
    private float startIntermediateTimeBtwSpawn;
    [SerializeField]
    public float advancedTimeBtwSpawn;
    [SerializeField]
    private float startAdvancedTimeBtwSpawn;
    [SerializeField]
    public float legendaryTimeBtwSpawn;
    [SerializeField]
    private float startLegendaryTimeBtwSpawn;
    private int basicLimit = 20;
    private Queue<GameObject> basicMob1 = new Queue<GameObject>();
    private Queue<GameObject> basicMob2 = new Queue<GameObject>();
    private Queue<GameObject> basicMob3 = new Queue<GameObject>();
    private Queue<GameObject> basicMob4 = new Queue<GameObject>();
    public GameObject[] intermediateMobs;
    public GameObject[] advancedMobs;

    private void Awake()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateObjectPools();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.currentState == GameManager.GameState.Playing){
            if(basicTimeBtwSpawn <= 0){
                Spawn();
                basicTimeBtwSpawn = startBasicTimeBtwSpawn;
            }else{
                basicTimeBtwSpawn -= Time.deltaTime;
            }

            if(intermediateTimeBtwSpawn <= 0){
                int randomMob = Random.Range(0, intermediateMobs.Length);
                Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(1.1f, 1.4f), 0));
                GameObject mob = Instantiate(intermediateMobs[randomMob], v3Pos, Quaternion.identity);
                intermediateTimeBtwSpawn = startIntermediateTimeBtwSpawn;
            }else{
                intermediateTimeBtwSpawn -= Time.deltaTime;
            }
            if(advancedTimeBtwSpawn <= 0){
                int randomMob = Random.Range(0, advancedMobs.Length);
                Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(1.1f, 1.4f), 0));
                GameObject mob = Instantiate(advancedMobs[randomMob], v3Pos, Quaternion.identity);
                advancedTimeBtwSpawn = startAdvancedTimeBtwSpawn;
            }else{
                advancedTimeBtwSpawn -= Time.deltaTime;
            }

        }
    }

    void Spawn(){
        int screenSide = Random.Range(1, 5);
        int randomMob = Random.Range(1, 5);
        if(screenSide == 1){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(1.1f, 1.4f), 0));
             if(GetAQueue(randomMob).Count > 0){
                GameObject mob = GetAQueue(randomMob).Dequeue();
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }
        }else if(screenSide == 2){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(1.1f, 1.4f), 0));
            if(GetAQueue(randomMob).Count > 0){
                GameObject mob = GetAQueue(randomMob).Dequeue();
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }
        }else if(screenSide == 3){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(1.1f, 1.4f), 0));
            if(GetAQueue(randomMob).Count > 0){
                GameObject mob = GetAQueue(randomMob).Dequeue();
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }
        }else if(screenSide == 4){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(1.1f, 1.4f), 0));
            if(GetAQueue(randomMob).Count > 0){
                GameObject mob = GetAQueue(randomMob).Dequeue();
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }
        }
    }

    Queue<GameObject> GetAQueue(int randInt){
        if(randInt == 1){
            return basicMob1;
        }else if(randInt == 2){
            return basicMob2;
        }else if(randInt == 3){
            return basicMob3;
        }else if(randInt == 4){
            return basicMob4;
        }
        return null;
    }

    void CreateObjectPools()
    {
        for (int i = 0; i < basicLimit; i++)
        {
            GameObject newBasicMob1 = Instantiate(mobs[mob1Int], this.transform.position, this.transform.rotation);
            newBasicMob1.SetActive(false);
            basicMob1.Enqueue(newBasicMob1);
        }

        for (int i = 0; i < basicLimit; i++)
        {
            GameObject newBasicMob2 = Instantiate(mobs[mob2Int], this.transform.position, this.transform.rotation);
            newBasicMob2.SetActive(false);
            basicMob2.Enqueue(newBasicMob2);
        }

        for (int i = 0; i < basicLimit; i++)
        {
            GameObject newBasicMob3 = Instantiate(mobs[mob3Int], this.transform.position, this.transform.rotation);
            newBasicMob3.SetActive(false);
            basicMob3.Enqueue(newBasicMob3);
        }

        for (int i = 0; i < basicLimit; i++)
        {
            GameObject newBasicMob4 = Instantiate(mobs[mob4Int], this.transform.position, this.transform.rotation);
            newBasicMob4.SetActive(false);
            basicMob4.Enqueue(newBasicMob4);
        }

    }

    public void RequeueMob(GameObject mob)
    {
        if(mob.GetComponent<Enemy>().enemyName == mob1)
        {
            mob.SetActive(false);
            basicMob1.Enqueue(mob);
        }else if(mob.GetComponent<Enemy>().enemyName == mob2)
        {
            mob.SetActive(false);
            basicMob2.Enqueue(mob);
        }else if(mob.GetComponent<Enemy>().enemyName == mob3)
        {
            mob.SetActive(false);
            basicMob3.Enqueue(mob);
        }else if(mob.GetComponent<Enemy>().enemyName == mob4)
        {
            mob.SetActive(false);
            basicMob4.Enqueue(mob);
        }else{
            Destroy(mob);
        }
    }

}
