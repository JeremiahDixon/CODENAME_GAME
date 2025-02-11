using System;
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
    TextMeshProUGUI scoreText;
    MobSpawner ms;
    PowerupMenu pum;
    public int score { get; private set; } = 0;
    public int Level { get; private set; } = 1;
    private int[] scoreThresholds = { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500 }; 
    public event Action<int> OnLevelUp;

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
            ms.StartIntermediateTimeBtwSpawn = 0.5f;
            ms.StartAdvancedTimeBtwSpawn = 1f;
            ms.StartLegendaryTimeBtwSpawn = 1;
            //ms.StartShooterTimeBtwSpawn = 7f;
        }else if(timePlayed >= playedThreeMin){
            ms.StartBasicTimeBtwSpawn = 0.4f;
            ms.StartIntermediateTimeBtwSpawn = 0.5f;
            ms.StartAdvancedTimeBtwSpawn = 1;
            ms.StartLegendaryTimeBtwSpawn = 5;
            //ms.StartShooterTimeBtwSpawn = 7f;
        }else if(timePlayed >= playedTwoMin){
            ms.StartBasicTimeBtwSpawn = 0.5f;
            ms.StartIntermediateTimeBtwSpawn = 2;
            ms.StartAdvancedTimeBtwSpawn = 5;
            ms.StartLegendaryTimeBtwSpawn = 10;
            //ms.StartShooterTimeBtwSpawn = 7f;
        }else if(timePlayed >= playedOneMin){
            ms.StartBasicTimeBtwSpawn = 0.6f;
            ms.StartIntermediateTimeBtwSpawn = 6;
            ms.StartAdvancedTimeBtwSpawn = 12;
            ms.StartLegendaryTimeBtwSpawn = 20;
            //ms.StartShooterTimeBtwSpawn = 7f;
        }else if(timePlayed >= playedThirtySeconds){
            ms.StartBasicTimeBtwSpawn = 0.7f;
            ms.StartIntermediateTimeBtwSpawn = 7;
            ms.StartAdvancedTimeBtwSpawn = 15;
            ms.StartLegendaryTimeBtwSpawn = 25;
            //ms.StartShooterTimeBtwSpawn = 7f;
        }
    }

    void TrackScore()
    {
        while (Level - 1 < scoreThresholds.Length && score >= scoreThresholds[Level - 1])
        {
            Level++;
            OnLevelUp?.Invoke(Level);
        }
        // if(score >= 5 && powerupCount == 0 )
        // {
        //     powerupCount ++;
        //     pum.PowerUp();
        // }else if(score >= 10 && powerupCount == 1)
        // {
        //     powerupCount ++;
        //     pum.PowerUp();
        // }else if(score >= 10 && powerupCount == 2)
        // {
        //     powerupCount ++;
        //     pum.PowerUp();
        // }
        // else if(score >= 10 && powerupCount == 3)
        // {
        //     powerupCount ++;
        //     pum.PowerUp();
        // }
        // else if(score >= 10 && powerupCount == 4)
        // {
        //     powerupCount ++;
        //     pum.PowerUp();
        // }
        // else if(score >= 10 && powerupCount == 5)
        // {
        //     powerupCount ++;
        //     pum.PowerUp();
        // }else if(score >= 10 && powerupCount == 6)
        // {
        //     powerupCount ++;
        //     pum.PowerUp();
        // }
        // else if(score >= 10 && powerupCount == 7)
        // {
        //     powerupCount ++;
        //     pum.PowerUp();
        // }
        // else if(score >= 10 && powerupCount == 8)
        // {
        //     powerupCount ++;
        //     pum.PowerUp();
        // }
        // else if(score >= 10 && powerupCount == 9)
        // {
        //     powerupCount ++;
        //     pum.PowerUp();
        // }
        // else if(score >= 10 && powerupCount == 10)
        // {
        //     powerupCount ++;
        //     pum.PowerUp();
        // }
        // else if(score >= 10 && powerupCount == 11)
        // {
        //     powerupCount ++;
        //     pum.PowerUp();
        // }
        // else if(score >= 15 && powerupCount == 12)
        // {
        //     powerupCount ++;
        //     pum.PowerUp();
        //     ms.SpawnBoss();
        //     GameObject.Find("PlayerCamera").GetComponent<TheCamera>().LockCamera();
        // }
    }

    public void IncreaseScore(int scoreValue){
        score += scoreValue;
        scoreText.text = score.ToString();
        TrackScore();
        // if(scoreValue == 500)
        // {
        //     GameManager.Instance.GameWin();
        // }else{
        //     TrackScore();
        // }
    }
}
