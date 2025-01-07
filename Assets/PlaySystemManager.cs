using UnityEngine;

public class PlaySystemManager : MonoBehaviour
{
    public float timePlayed = 0;
    [SerializeField]
    private float playedOneMin = 60.0f;
    [SerializeField]
    private float playedThreeMin = 180.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.currentState == GameManager.GameState.Playing){
            timePlayed += Time.deltaTime;

            if(timePlayed >= playedThreeMin){
                MonsterSpawner.Instance.spawnAfterSlime = 0.1f;
                MonsterSpawner.Instance.spawnAfterDwarf = 0.20f;
                MonsterSpawner.Instance.spawnAfterSkull = 0.45f;

            }else if(timePlayed >= playedOneMin){
                MonsterSpawner.Instance.spawnAfterSlime = 0.135f;
                MonsterSpawner.Instance.spawnAfterDwarf = 0.35f;
                MonsterSpawner.Instance.spawnAfterSkull = 0.65f;

            }

        }
    }
}
