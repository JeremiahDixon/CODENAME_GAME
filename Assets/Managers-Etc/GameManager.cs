using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { MainMenu, Playing, Paused, GameWin, GameOver, PoweringUp }
    public GameState currentState{get; private set;}
    int playerHealth; public int PlayerHealth{get => playerHealth; private set => playerHealth = value;}
    int maxPlayerHealth; public int MaxPlayerHealth{get => maxPlayerHealth; private set => maxPlayerHealth = value;}
    bool dead = false; public bool Dead{get => dead; private set => dead = value;}
    public IPlayer thePlayer;
    HealthManager healthManager;
    private SceneLoader sceneLoader;
    [SerializeField] GameOverScreen gameOverScreen;
    [SerializeField] GameWinScreen gameWinScreen;
    [SerializeField] Transform startPos;
    [SerializeField] GameObject[] playerClasses;
    [SerializeField] GameObject currentClass;
    [SerializeField] ClassSO[] classSos;
    public ClassSO currentClassSo;
    const string GAMEPLAY_SCENE_DARKFOREST = "VS";
    const string GAMEPLAY_SCENE_HEAT = "VS 2";
    const string SCENE_LOADER_NAME = "SceneLoader";
    const string GAMEOVER_SCREEN_NAME = "GameOverScreen";
    const string GAMEWIN_SCREEN_NAME = "GameWinScreen";
    const string START_POSITION_NAME = "StartPosition";
    const string HEALTH_MANAGER_NAME = "HealthManager";
    const string ARCHER_CLASS_NAME = "Soldier";
    const string ORC_CLASS_NAME = "Orc";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keeps this manager between scenes
            Debug.Log("GameManager Initialized");
        }
        else
        {
            Destroy(gameObject);  // Prevent duplicates
        }
    }

    // called second
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        sceneLoader = GameObject.Find(SCENE_LOADER_NAME).GetComponent<SceneLoader>();
    }

    // called third
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == GAMEPLAY_SCENE_DARKFOREST){
            LoadPlayScreen();
            resetPlayer();
            currentState = GameState.Playing;
        }else if(scene.name == GAMEPLAY_SCENE_HEAT){
            LoadPlayScreen();
            resetPlayer();
            currentState = GameState.Playing;
        }
    }

    // called 4th
    void Start()
    {
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void LoadPlayScreen(){
        healthManager = GameObject.Find(HEALTH_MANAGER_NAME).GetComponent<HealthManager>();
        gameOverScreen = GameObject.FindGameObjectWithTag(GAMEOVER_SCREEN_NAME).GetComponent<GameOverScreen>();
        gameWinScreen = GameObject.FindGameObjectWithTag(GAMEWIN_SCREEN_NAME).GetComponent<GameWinScreen>();
        startPos = GameObject.Find(START_POSITION_NAME).transform;
    }

    public void Heal(int amount){
        int newHealthValue = this.playerHealth + amount;
        healthManager.Heal(amount);
        if(newHealthValue >= maxPlayerHealth){
            playerHealth = maxPlayerHealth;
        }else{
            playerHealth = newHealthValue;
        }
    }

    public void TakeDamage(int amount){
        if(!dead){
            int newHealthValue = this.playerHealth - amount;
            healthManager.TakeDamage(amount);
            if(newHealthValue <= 0){
                dead = true;
                currentState = GameState.GameOver;
                playerHealth = 0;
                thePlayer.Die();
                StartCoroutine(GameOverAfter());
            }else{
                playerHealth = newHealthValue;
            }
        }
    }

    private IEnumerator GameOverAfter()
    {
        yield return new WaitForSeconds(1);
        gameOverScreen.GameOver();
        Time.timeScale = 0;
    }

    public void GameWin()
    {
        currentState = GameState.GameWin;
        gameWinScreen.GameWin();
        Time.timeScale = 0;
    }

    void resetPlayer()
    {
        GameObject newPlayer = Instantiate(currentClass, startPos.position, startPos.rotation);
        thePlayer = newPlayer.GetComponent<IPlayer>();
        maxPlayerHealth = currentClassSo.baseHp;
        playerHealth = maxPlayerHealth;
        dead = false;
    }

    public void ReloadCurrentScene()
    {
        sceneLoader.ReloadCurrentScene();
    }
    public void LoadAScene(String sceneName){
        sceneLoader.LoadScene(sceneName);
    }

    public void setPlayerClassSo(string className){
        switch(className){
            case ARCHER_CLASS_NAME:
                currentClass = playerClasses[0];
                currentClassSo = classSos [0];
                break;
            case ORC_CLASS_NAME:
                currentClass = playerClasses[1];
                currentClassSo = classSos [1];
                break;
            default:
                break;
        }
    }

    public void PoweringUp()
    {
        currentState = GameState.PoweringUp;
    }

    public void DonePoweringUp()
    {
        currentState = GameState.Playing;
    }

    public void IncreaseMaxHealth(int increaseAmount)
    {
        maxPlayerHealth += increaseAmount;
    }

}
