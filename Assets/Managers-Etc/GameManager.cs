using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { MainMenu, Playing, Paused, Storage, GameOver }
    public GameState currentState{get; private set;}
    private bool innLocked{get; set;}
    private bool shoplocked{get; set;}
    public int playerHealth{get; private set;} = 10;
    public int maxPlayerHealth{get; private set;} = 10;
    private IPlayer thePlayer;
    private HealthManager healthManager;
    [SerializeField]
    private GameOverScreen gameOverScreen;
    public bool dead{get; private set;} = false;
    private SceneLoader sceneLoader;
    [SerializeField]
    private Transform startPos;
    [SerializeField]
    ClassSO[] classSos;
    [SerializeField]
    ClassSO currentClassSo;

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
            thePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<IPlayer>();
            LoadPlayScreen();
            resetPlayer();
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
        thePlayer.ResetPlayer();
        thePlayer.SetClassSo(currentClassSo);
        thePlayer.transform.position = startPos.position;
        maxPlayerHealth = currentClassSo.baseHp;
        playerHealth = maxPlayerHealth;
        dead = false;
        currentState = GameState.Playing;
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
                currentClassSo = classSos[0];
                break;
            case "Orc":
                currentClassSo = classSos[1];
                break;
            default:
                break;
        }
    }
}
