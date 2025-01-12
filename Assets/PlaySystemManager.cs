using TMPro;
using UnityEngine;

public class PlaySystemManager : MonoBehaviour
{
    public float timePlayed = 0;
    [SerializeField]
    private float playedThirtySeconds = 30.0f;
    [SerializeField]
    private float playedOneMin = 60.0f;
    [SerializeField]
    private float playedTwoMin = 120.0f;
    [SerializeField]
    private float playedThreeMin = 180.0f;
    [SerializeField]
    private float playedFiveMin = 300.0f;
    int score = 0;
    TextMeshProUGUI scoreText;
    MobSpawner ms;
    PowerupMenu pum;
    int powerupCount = 0;

    void Awake()
    {
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        score = 0;
        scoreText.text = score.ToString();
        ms = GameObject.Find("Spawner").GetComponent<MobSpawner>();
        pum = GameObject.Find("PowerupCanvas").GetComponent<PowerupMenu>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.currentState == GameManager.GameState.Playing){
            TrackTime();
        }
    }

    void TrackTime()
    {
        timePlayed += Time.deltaTime;
        if(timePlayed >= playedFiveMin){
            ms.startBasicTimeBtwSpawn = 1;
            ms.startIntermediateTimeBtwSpawn = 0.3f;
            ms.startAdvancedTimeBtwSpawn = 0.75f;
            ms.startLegendaryTimeBtwSpawn = 1;
        }else if(timePlayed >= playedThreeMin){
            ms.startBasicTimeBtwSpawn = 1;
            ms.startIntermediateTimeBtwSpawn = 0.5f;
            ms.startAdvancedTimeBtwSpawn = 1;
            ms.startLegendaryTimeBtwSpawn = 5;
        }else if(timePlayed >= playedTwoMin){
            ms.startBasicTimeBtwSpawn = 0.3f;
            ms.startIntermediateTimeBtwSpawn = 2;
            ms.startAdvancedTimeBtwSpawn = 5;
            ms.startLegendaryTimeBtwSpawn = 10;
        }else if(timePlayed >= playedOneMin){
            ms.startBasicTimeBtwSpawn = 0.4f;
            ms.startIntermediateTimeBtwSpawn = 6;
            ms.startAdvancedTimeBtwSpawn = 12;
            ms.startLegendaryTimeBtwSpawn = 20;
        }else if(timePlayed >= playedThirtySeconds){
            ms.startBasicTimeBtwSpawn = 0.5f;
            ms.startIntermediateTimeBtwSpawn = 7;
            ms.startAdvancedTimeBtwSpawn = 15;
            ms.startLegendaryTimeBtwSpawn = 25;
        }
    }

    void TrackScore()
    {
        if(score >= 100 && powerupCount == 0 )
        {
            powerupCount ++;
            pum.PowerUp();
        }else if(score >= 150 && powerupCount == 1)
        {
            powerupCount ++;
            pum.PowerUp();
        }else if(score >= 200 && powerupCount == 2)
        {
            powerupCount ++;
            pum.PowerUp();
        }
        else if(score >= 250 && powerupCount == 3)
        {
            powerupCount ++;
            pum.PowerUp();
        }
        else if(score >= 300 && powerupCount == 4)
        {
            powerupCount ++;
            pum.PowerUp();
        }
        else if(score >= 350 && powerupCount == 5)
        {
            powerupCount ++;
            pum.PowerUp();
        }else if(score >= 400 && powerupCount == 6)
        {
            powerupCount ++;
            pum.PowerUp();
        }
        else if(score >= 450 && powerupCount == 7)
        {
            powerupCount ++;
            pum.PowerUp();
        }
    }

    public void IncreaseScore(int scoreValue){
        score += scoreValue;
        scoreText.text = score.ToString();
        TrackScore();
    }
}
