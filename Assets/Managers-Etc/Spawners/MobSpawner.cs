 
using System.Collections;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] float basicTimeBtwSpawn; public float BasicTimeBtwSpawn{get => basicTimeBtwSpawn; set => basicTimeBtwSpawn = value;}
    [SerializeField] float startBasicTimeBtwSpawn; public float StartBasicTimeBtwSpawn{get => startBasicTimeBtwSpawn; set => startBasicTimeBtwSpawn = value;}
    [SerializeField] float intermediateTimeBtwSpawn; public float IntermediateTimeBtwSpawn{get => intermediateTimeBtwSpawn; set => intermediateTimeBtwSpawn = value;}
    [SerializeField] float startIntermediateTimeBtwSpawn; public float StartIntermediateTimeBtwSpawn{get => startIntermediateTimeBtwSpawn; set => startIntermediateTimeBtwSpawn = value;}
    [SerializeField] float advancedTimeBtwSpawn; public float AdvancedTimeBtwSpawn{get => advancedTimeBtwSpawn; set => advancedTimeBtwSpawn = value;}
    [SerializeField] float startAdvancedTimeBtwSpawn; public float StartAdvancedTimeBtwSpawn{get => startAdvancedTimeBtwSpawn; set => startAdvancedTimeBtwSpawn = value;}
    [SerializeField] float legendaryTimeBtwSpawn; public float LegendaryTimeBtwSpawn{get => legendaryTimeBtwSpawn; set => legendaryTimeBtwSpawn = value;}
    [SerializeField] float startLegendaryTimeBtwSpawn; public float StartLegendaryTimeBtwSpawn{get => startLegendaryTimeBtwSpawn; set => startLegendaryTimeBtwSpawn = value;}
    [SerializeField] float shooterTimeBtwSpawn; public float ShooterTimeBtwSpawn{get => shooterTimeBtwSpawn; set => shooterTimeBtwSpawn = value;}
    [SerializeField] float startShooterTimeBtwSpawn; public float StartShooterTimeBtwSpawn{get => startShooterTimeBtwSpawn; set => startShooterTimeBtwSpawn = value;}
    int basicLimit = 30;
    int legendaryLimit = 1;
    int shooterLimit = 20;
    [SerializeField] GameObject[] basicMobs; public GameObject[] BasicMobs{get => basicMobs; set => basicMobs = value;}
    [SerializeField] GameObject[] intermediateMobs; public GameObject[] IntermediateMobs{get => intermediateMobs; set => intermediateMobs = value;}
    [SerializeField] GameObject[] advancedMobs; public GameObject[] AdvancedMobs{get => advancedMobs; set => advancedMobs = value;}
    [SerializeField] GameObject[] legendaryMobs; public GameObject[] LegendaryMobs{get => legendaryMobs; set => legendaryMobs = value;}
    [SerializeField] GameObject[] shooterMobs; public GameObject[] ShooterMobs{get => shooterMobs; set => shooterMobs = value;}
    [SerializeField] ArrayList basicMobsList = new ArrayList();
    [SerializeField] ArrayList intermediateMobsList = new ArrayList();
    [SerializeField] ArrayList advancedMobsList = new ArrayList();
    [SerializeField] ArrayList legendaryMobsList = new ArrayList();
    [SerializeField] ArrayList shooterMobsList = new ArrayList();
    [SerializeField] ArrayList spawnedMobs = new ArrayList();
    [SerializeField] LayerMask enemyLayer;
    public enum PlayState { Bossfight, MobWaves }
    [SerializeField] PlayState currentState;
    [SerializeField] MobSpawnerSO spawnerSo;
    [SerializeField] float canSpawnRadius;

    private void Awake()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnerSo.CreateInfo(this.gameObject);
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
            if(shooterTimeBtwSpawn <= 0){
                GameObject smob = GetRandomMob(shooterMobsList);
                if( smob != null )
                {
                    Spawn(smob, GetRandomScreenside());
                }
                shooterTimeBtwSpawn = startShooterTimeBtwSpawn;
            }else{
                shooterTimeBtwSpawn -= Time.deltaTime;
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

    // ArrayList GetMobList(Enemy.EnemyLevel enemyLevel)
    // {
    //     switch (enemyLevel)
    //     {
    //         case Enemy.EnemyLevel.basic:
    //             return basicMobsList;
    //         case Enemy.EnemyLevel.intermediate:
    //             return intermediateMobsList;
    //         case Enemy.EnemyLevel.advanced:
    //             return advancedMobsList;
    //         case Enemy.EnemyLevel.legendary:
    //             return legendaryMobsList;
    //         case Enemy.EnemyLevel.shooter:
    //             return shooterMobsList;
    //         default:
    //             return basicMobsList;
    //     }
    // }

    void Spawn(GameObject mob, int screenSide){
        //ArrayList mobList = GetMobList(mob.GetComponent<Enemy>().enemyLevel);
        if(screenSide == 1){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(1.01f, 1.05f), 0));
            if(CanSpawn(v3Pos, canSpawnRadius)){
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }else
            {
                RequeueMob(mob);
            }
        }else if(screenSide == 2){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(1.01f, 1.05f), Random.Range(0.0f, 1.0f), 0));
            if(CanSpawn(v3Pos, canSpawnRadius)){
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }else
            {
                RequeueMob(mob);
            }
        }else if(screenSide == 3){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(0.0f, -0.05f), 0));
            if(CanSpawn(v3Pos, canSpawnRadius)){
                mob.transform.position = v3Pos;
                mob.SetActive(true);
            }else
            {
                RequeueMob(mob);
            }
        }else if(screenSide == 4){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, -0.05f), Random.Range(0f, 1f), 0));
            if(CanSpawn(v3Pos, canSpawnRadius)){
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

        for(int x = 0; x < shooterMobs.Length; x++){
            for (int i = 0; i < shooterLimit; i++)
            {
                GameObject newShooterMob = Instantiate(shooterMobs[x], this.transform.position, this.transform.rotation);
                newShooterMob.SetActive(false);
                shooterMobsList.Add(newShooterMob);
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
            case Enemy.EnemyLevel.shooter:
                mob.SetActive(false);
                shooterMobsList.Add(mob);
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
        Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.75f, 1f), Random.Range(1.01f, 1.05f), 0));
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
