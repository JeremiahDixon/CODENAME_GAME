using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { MainMenu, Playing, Paused, Storage, GameOver }
    public GameState currentState{get; private set;}
    private bool innLocked{get; set;}
    private bool shoplocked{get; set;}
    public int playerHealth{get; private set;} = 10;
    public int maxPlayerHealth{get; private set;} = 10;
    public IPlayer thePlayer;
    private HealthManager healthManager;
    [SerializeField]
    private GameOverScreen gameOverScreen;
    public bool dead{get; private set;} = false;
    private SceneLoader sceneLoader;
    [SerializeField]
    private Transform startPos;
    [SerializeField]
    GameObject[] playerClasses;
    [SerializeField]
    GameObject currentClass;
    [SerializeField]
    ClassSO[] classSos;
    public ClassSO currentClassSo;

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
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    // called third
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "VS"){
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
        healthManager = GameObject.Find("HealthManager").GetComponent<HealthManager>();
        gameOverScreen = GameObject.FindGameObjectWithTag("GameOverScreen").GetComponent<GameOverScreen>();
        startPos = GameObject.Find("StartPosition").transform;
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
            case "Soldier":
                currentClass = playerClasses[0];
                currentClassSo = classSos [0];
                break;
            case "Orc":
                currentClass = playerClasses[1];
                currentClassSo = classSos [1];
                break;
            default:
                break;
        }
    }

}
