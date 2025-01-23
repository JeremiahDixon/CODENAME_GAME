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
            ms.StartBasicTimeBtwSpawn = 1;
            ms.StartIntermediateTimeBtwSpawn = 0.3f;
            ms.StartAdvancedTimeBtwSpawn = 0.75f;
            ms.StartLegendaryTimeBtwSpawn = 1;
        }else if(timePlayed >= playedThreeMin){
            ms.StartBasicTimeBtwSpawn = 0.2f;
            ms.StartIntermediateTimeBtwSpawn = 0.5f;
            ms.StartAdvancedTimeBtwSpawn = 1;
            ms.StartLegendaryTimeBtwSpawn = 5;
        }else if(timePlayed >= playedTwoMin){
            ms.StartBasicTimeBtwSpawn = 0.3f;
            ms.StartIntermediateTimeBtwSpawn = 2;
            ms.StartAdvancedTimeBtwSpawn = 5;
            ms.StartLegendaryTimeBtwSpawn = 10;
        }else if(timePlayed >= playedOneMin){
            ms.StartBasicTimeBtwSpawn = 0.4f;
            ms.StartIntermediateTimeBtwSpawn = 6;
            ms.StartAdvancedTimeBtwSpawn = 12;
            ms.StartLegendaryTimeBtwSpawn = 20;
        }else if(timePlayed >= playedThirtySeconds){
            ms.StartBasicTimeBtwSpawn = 0.5f;
            ms.StartIntermediateTimeBtwSpawn = 7;
            ms.StartAdvancedTimeBtwSpawn = 15;
            ms.StartLegendaryTimeBtwSpawn = 25;
        }
    }

    void TrackScore()
    {
        if(score >= 100 && powerupCount == 0 )
        {
            powerupCount ++;
            pum.PowerUp();
        }else if(score >= 200 && powerupCount == 1)
        {
            powerupCount ++;
            pum.PowerUp();
        }else if(score >= 300 && powerupCount == 2)
        {
            powerupCount ++;
            pum.PowerUp();
        }
        else if(score >= 400 && powerupCount == 3)
        {
            powerupCount ++;
            pum.PowerUp();
        }
        else if(score >= 500 && powerupCount == 4)
        {
            powerupCount ++;
            pum.PowerUp();
        }
        else if(score >= 750 && powerupCount == 5)
        {
            powerupCount ++;
            pum.PowerUp();
        }else if(score >= 1000 && powerupCount == 6)
        {
            powerupCount ++;
            pum.PowerUp();
        }
        else if(score >= 1250 && powerupCount == 7)
        {
            powerupCount ++;
            pum.PowerUp();
        }
        else if(score >= 1500 && powerupCount == 8)
        {
            powerupCount ++;
            pum.PowerUp();
        }
        else if(score >= 1750 && powerupCount == 9)
        {
            powerupCount ++;
            pum.PowerUp();
        }
        else if(score >= 2000 && powerupCount == 10)
        {
            powerupCount ++;
            pum.PowerUp();
        }
        else if(score >= 2250 && powerupCount == 11)
        {
            powerupCount ++;
            pum.PowerUp();
        }
        else if(score >= 2500 && powerupCount == 12)
        {
            powerupCount ++;
            pum.PowerUp();
            ms.SpawnBoss();
            GameObject.Find("PlayerCamera").GetComponent<TheCamera>().LockCamera();
        }
    }

    public void IncreaseScore(int scoreValue){
        score += scoreValue;
        scoreText.text = score.ToString();
        if(scoreValue == 500)
        {
            GameManager.Instance.GameWin();
        }else{
            TrackScore();
        }
    }
}
