
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
    private int legendaryLimit = 1;
    public GameObject[] basicMobs;
    public GameObject[] intermediateMobs;
    public GameObject[] advancedMobs;
    public GameObject[] legendaryMobs;
    public ArrayList basicMobsList = new ArrayList();
    public ArrayList intermediateMobsList = new ArrayList();
    public ArrayList advancedMobsList = new ArrayList();
    public ArrayList legendaryMobsList = new ArrayList();
    public ArrayList spawnedMobs = new ArrayList();
    public LayerMask enemyLayer; // Layer to detect other enemies
    public enum PlayState { Bossfight, MobWaves }
    public PlayState currentState;

    private void Awake()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = PlayState.MobWaves;
        CreateObjectPools();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.currentState == GameManager.GameState.Playing && currentState == PlayState.MobWaves){
            if(basicTimeBtwSpawn <= 0){
                GameObject bmob = GetRandomMob(basicMobsList);
                if( bmob != null )
                {
                    Spawn(bmob, GetRandomScreenside());
                }
                basicTimeBtwSpawn = startBasicTimeBtwSpawn;
            }else{
                basicTimeBtwSpawn -= Time.deltaTime;
            }
            if(intermediateTimeBtwSpawn <= 0){
                GameObject imob = GetRandomMob(intermediateMobsList);
                if( imob != null )
                {
                    Spawn(imob, GetRandomScreenside());
                }
                intermediateTimeBtwSpawn = startIntermediateTimeBtwSpawn;
            }else{
                intermediateTimeBtwSpawn -= Time.deltaTime;
            }
            if(advancedTimeBtwSpawn <= 0){
                GameObject amob = GetRandomMob(advancedMobsList);
                if( amob != null )
                {
                    Spawn(amob, GetRandomScreenside());
                }
                advancedTimeBtwSpawn = startAdvancedTimeBtwSpawn;
            }else{
                advancedTimeBtwSpawn -= Time.deltaTime;
            }

        }
    }

    GameObject GetRandomMob(ArrayList mobList){
        if(mobList.Count > 0)
        {
            int randomMob = Random.Range(0, mobList.Count);
            GameObject mob = (GameObject)mobList[randomMob];
            mobList.RemoveAt(randomMob);
            spawnedMobs.Add(mob);
            return mob;
        }
        return null;
    }

    int GetRandomScreenside(){
        int screenSide = Random.Range(1, 5);
        return screenSide;
    }

    ArrayList GetMobList(Enemy.EnemyLevel enemyLevel)
    {
        switch (enemyLevel)
        {
            case Enemy.EnemyLevel.basic:
                return basicMobsList;
            case Enemy.EnemyLevel.intermediate:
                return intermediateMobsList;
            case Enemy.EnemyLevel.advanced:
                return advancedMobsList;
            case Enemy.EnemyLevel.legendary:
                return legendaryMobsList;
            default:
                return basicMobsList;
        }
    }

    void Spawn(GameObject mob, int screenSide){
        ArrayList mobList = GetMobList(mob.GetComponent<Enemy>().enemyLevel);
        if(screenSide == 1){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(1.01f, 1.05f), 0));
            if(CanSpawn(v3Pos, 0.2f)){
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }else
            {
                RequeueMob(mob);
            }
        }else if(screenSide == 2){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(1.01f, 1.05f), Random.Range(0.0f, 1.0f), 0));
            if(CanSpawn(v3Pos, 0.2f)){
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }else
            {
                RequeueMob(mob);
            }
        }else if(screenSide == 3){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(0.0f, -0.05f), 0));
            if(CanSpawn(v3Pos, 0.2f)){
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }else
            {
                RequeueMob(mob);
            }
        }else if(screenSide == 4){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, -0.05f), Random.Range(0f, 1f), 0));
            if(CanSpawn(v3Pos, 0.2f)){
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }else
            {
                RequeueMob(mob);
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

        for(int x = 0; x < legendaryMobs.Length; x++){
            for (int i = 0; i < legendaryLimit; i++)
            {
                GameObject newLegendaryMob = Instantiate(legendaryMobs[x], this.transform.position, this.transform.rotation);
                newLegendaryMob.SetActive(false);
                legendaryMobsList.Add(newLegendaryMob);
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
                spawnedMobs.Remove(mob);
                break;
            case Enemy.EnemyLevel.intermediate:
                mob.SetActive(false);
                intermediateMobsList.Add(mob);
                spawnedMobs.Remove(mob);
                break;
            case Enemy.EnemyLevel.advanced:
                mob.SetActive(false);
                advancedMobsList.Add(mob);
                spawnedMobs.Remove(mob);
                break;
            case Enemy.EnemyLevel.legendary:
                mob.SetActive(false);
                legendaryMobsList.Add(mob);
                spawnedMobs.Remove(mob);
                break;
        }
        
    }

    public void RespawnMob(GameObject mob)
    {
        Spawn(mob, GetRandomScreenside());
    }

    public void SpawnBoss()
    {
        currentState = PlayState.Bossfight;
        DespawnAll();
        Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(1.01f, 1.05f), 0));
        if(legendaryMobsList.Count > 0){
            GameObject mob = (GameObject)legendaryMobsList[0];
            legendaryMobsList.RemoveAt(0);
            mob.transform.position = v3Pos;
            mob.SetActive(true);
        }
    }

    public void DespawnAll()
    {
        foreach (GameObject mob in spawnedMobs)
        {
            mob.SetActive(false);
        }
        
    }
}
