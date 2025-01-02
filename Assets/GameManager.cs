using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { MainMenu, Playing, Paused, Storage, GameOver }
    public GameState currentState;
    private bool innLocked{get; set;}
    private bool shoplocked{get; set;}
    public int playerHealth{get; private set;} = 10;
    public int maxPlayerHealth{get; private set;} = 10;
    private IPlayer thePlayer;
    private HealthManager healthManager;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<IPlayer>();
        // playerHealth = 10;
        // maxPlayerHealth = 10;
        healthManager = GameObject.Find("HealthManager").GetComponent<HealthManager>();
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
        int newHealthValue = this.playerHealth - amount;
        healthManager.TakeDamage(amount);
        if(newHealthValue <= 0){
            playerHealth = 0;
            thePlayer.Die();
        }else{
            playerHealth = newHealthValue;
        }
    }
}
