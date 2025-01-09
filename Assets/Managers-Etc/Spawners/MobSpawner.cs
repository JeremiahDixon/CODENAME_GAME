using System.Collections;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public float basicTimeBtwSpawn;
    [SerializeField]
    private float startBasicTimeBtwSpawn;
    public float intermediateTimeBtwSpawn;
    [SerializeField]
    private float startIntermediateTimeBtwSpawn;
    public float advancedTimeBtwSpawn;
    [SerializeField]
    private float startAdvancedTimeBtwSpawn;
    public float legendaryTimeBtwSpawn;
    [SerializeField]
    private float startLegendaryTimeBtwSpawn;
    private int basicLimit = 30;
    public GameObject[] basicMobs;
    public GameObject[] intermediateMobs;
    public GameObject[] advancedMobs;
    public ArrayList basicMobsList = new ArrayList();
    public ArrayList intermediateMobsList = new ArrayList();
    public ArrayList advancedMobsList = new ArrayList();
    public ArrayList legendaryMobsList = new ArrayList();

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
        int randomMob = Random.Range(0, basicMobsList.Count);
        if(screenSide == 1){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(1.1f, 1.4f), 0));
             if(basicMobsList.Count > 0){
                GameObject mob = (GameObject)basicMobsList[randomMob];
                basicMobsList.RemoveAt(randomMob);
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }
        }else if(screenSide == 2){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(1.1f, 1.4f), Random.Range(0.0f, 1.0f), 0));
            if(basicMobsList.Count > 0){
                GameObject mob = (GameObject)basicMobsList[randomMob];
                basicMobsList.RemoveAt(randomMob);
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }
        }else if(screenSide == 3){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(0.0f, -0.4f), 0));
            if(basicMobsList.Count > 0){
                GameObject mob = (GameObject)basicMobsList[randomMob];
                basicMobsList.RemoveAt(randomMob);
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }
        }else if(screenSide == 4){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, -0.4f), Random.Range(0.0f, 1f), 0));
            if(basicMobsList.Count > 0){
                GameObject mob = (GameObject)basicMobsList[randomMob];
                basicMobsList.RemoveAt(randomMob);
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }
        }
    }

    void CreateObjectPools()
    {
        for(int x = 0; x < basicMobs.Length; x++){
            for (int i = 0; i < basicLimit; i++)
            {
                GameObject newBasicMob = Instantiate(basicMobs[x], this.transform.position, this.transform.rotation);
                newBasicMob.SetActive(false);
                basicMobsList.Add(newBasicMob);
            }
        }

        for(int x = 0; x < intermediateMobs.Length; x++){
            for (int i = 0; i < basicLimit; i++)
            {
                GameObject newIntermediateMob = Instantiate(intermediateMobs[x], this.transform.position, this.transform.rotation);
                newIntermediateMob.SetActive(false);
                intermediateMobsList.Add(newIntermediateMob);
            }
        }

        for(int x = 0; x < advancedMobs.Length; x++){
            for (int i = 0; i < basicLimit; i++)
            {
                GameObject newAdvancedMob = Instantiate(advancedMobs[x], this.transform.position, this.transform.rotation);
                newAdvancedMob.SetActive(false);
                advancedMobsList.Add(newAdvancedMob);
            }
        }

    }

    public void RequeueMob(GameObject mob)
    {
        switch(mob.GetComponent<Enemy>().enemyLevel)
        {
            case Enemy.EnemyLevel.basic:
                mob.SetActive(false);
                basicMobsList.Add(mob);
                break;
            case Enemy.EnemyLevel.intermediate:
                mob.SetActive(false);
                intermediateMobsList.Add(mob);
                break;
            case Enemy.EnemyLevel.advanced:
                mob.SetActive(false);
                advancedMobsList.Add(mob);
                break;
            case Enemy.EnemyLevel.legendary:
                mob.SetActive(false);
                legendaryMobsList.Add(mob);
                break;
        }
        
    }

}
