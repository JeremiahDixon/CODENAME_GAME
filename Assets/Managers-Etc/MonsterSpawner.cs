using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner Instance;
    public GameObject[] mobs;
    public float timeBtwSpawn;
    [SerializeField]
    private float startTimeBtwSpawn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keeps this manager between scenes
        }
        else
        {
            Destroy(gameObject);  // Prevent duplicates
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.currentState != GameManager.GameState.GameOver){
            if(timeBtwSpawn <= 0){
                Spawn();
                timeBtwSpawn = startTimeBtwSpawn;
            }else{
                timeBtwSpawn -= Time.deltaTime;
            }
        }
    }

    void Spawn(){
        int screenSide = Random.Range(1, 5);
        int randomMob = Random.Range(0, mobs.Length);
        if(screenSide == 1){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(1.1f, 1.4f), 0));
            GameObject mob = Instantiate(mobs[randomMob], v3Pos, Quaternion.identity);
            SetNextSpawnTime(randomMob);
        }else if(screenSide == 2){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(1.1f, 1.4f), Random.Range(0.0f, 1.0f), 0));
            GameObject mob = Instantiate(mobs[randomMob], v3Pos, Quaternion.identity);
            SetNextSpawnTime(randomMob);
        }else if(screenSide == 3){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(0.0f, -0.4f), 0));
            GameObject mob = Instantiate(mobs[randomMob], v3Pos, Quaternion.identity);
            SetNextSpawnTime(randomMob);
        }else if(screenSide == 4){
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, -0.4f), Random.Range(0.0f, 1f), 0));
            GameObject mob = Instantiate(mobs[randomMob], v3Pos, Quaternion.identity);
            SetNextSpawnTime(randomMob);
        }
    }

    void SetNextSpawnTime(int randomMob){
        if(randomMob == 0){
            startTimeBtwSpawn = 1.0f;
        }else if(randomMob == 1){
            startTimeBtwSpawn = 0.15f;
        }else if(randomMob == 2){
            startTimeBtwSpawn = 0.5f;
        }
    }

}
