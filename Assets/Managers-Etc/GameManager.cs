using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

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
        thePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<IPlayer>();
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    // called third
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "VS"){
            LoadPlayScreen();
            resetPlayer();
        }
    }

    // called 4th Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void LoadPlayScreen(){
        healthManager = GameObject.Find("HealthManager").GetComponent<HealthManager>();
        gameOverScreen = GameObject.FindGameObjectWithTag("GameOverScreen").GetComponent<GameOverScreen>();
        startPos = GameObject.Find("StartPosition").transform;
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

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

    public void ReloadPlayScene()
    {
        sceneLoader.ReloadCurrentScene();
    }

    void resetPlayer()
    {
        thePlayer.ResetPlayer();
        thePlayer.transform.position = startPos.position;
        maxPlayerHealth = 10;
        playerHealth = maxPlayerHealth;
        dead = false;
        currentState = GameState.Playing;
    }
}
