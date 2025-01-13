using System.Collections;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public float basicTimeBtwSpawn;
    public float startBasicTimeBtwSpawn;
    public float intermediateTimeBtwSpawn;
    public float startIntermediateTimeBtwSpawn;
    public float advancedTimeBtwSpawn;
    public float startAdvancedTimeBtwSpawn;
    public float legendaryTimeBtwSpawn;
    public float startLegendaryTimeBtwSpawn;
    private int basicLimit = 30;
    public GameObject[] basicMobs;
    public GameObject[] intermediateMobs;
    public GameObject[] advancedMobs;
    public ArrayList basicMobsList = new ArrayList();
    public ArrayList intermediateMobsList = new ArrayList();
    public ArrayList advancedMobsList = new ArrayList();
    public ArrayList legendaryMobsList = new ArrayList();
    public LayerMask enemyLayer; // Layer to detect other enemies

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
                Spawn(basicMobsList);
                basicTimeBtwSpawn = startBasicTimeBtwSpawn;
            }else{
                basicTimeBtwSpawn -= Time.deltaTime;
            }
            if(intermediateTimeBtwSpawn <= 0){
                Spawn(intermediateMobsList);
                intermediateTimeBtwSpawn = startIntermediateTimeBtwSpawn;
            }else{
                intermediateTimeBtwSpawn -= Time.deltaTime;
            }
            if(advancedTimeBtwSpawn <= 0){
                Spawn(advancedMobsList);
                advancedTimeBtwSpawn = startAdvancedTimeBtwSpawn;
            }else{
                advancedTimeBtwSpawn -= Time.deltaTime;
            }

        }
    }

    void Spawn(ArrayList mobList){
        int screenSide = Random.Range(1, 5);
        int randomMob = Random.Range(0, mobList.Count);
        if(screenSide == 1){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(1.1f, 1.4f), 0));
             if(mobList.Count > 0 && CanSpawn(v3Pos, 1.0f)){
                GameObject mob = (GameObject)mobList[randomMob];
                mobList.RemoveAt(randomMob);
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }
        }else if(screenSide == 2){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(1.1f, 1.4f), Random.Range(0.0f, 1.0f), 0));
            if(mobList.Count > 0 && CanSpawn(v3Pos, 1.0f)){
                GameObject mob = (GameObject)mobList[randomMob];
                mobList.RemoveAt(randomMob);
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }
        }else if(screenSide == 3){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(0.0f, -0.4f), 0));
            if(mobList.Count > 0 && CanSpawn(v3Pos, 1.0f)){
                GameObject mob = (GameObject)mobList[randomMob];
                mobList.RemoveAt(randomMob);
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }
        }else if(screenSide == 4){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, -0.4f), Random.Range(0.0f, 1f), 0));
            if(mobList.Count > 0 && CanSpawn(v3Pos, 1.0f)){
                GameObject mob = (GameObject)mobList[randomMob];
                mobList.RemoveAt(randomMob);
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }
        }
    }

    bool CanSpawn(Vector3 position, float radius)
    {
        return Physics2D.OverlapCircle(position, radius, enemyLayer) == null;
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
