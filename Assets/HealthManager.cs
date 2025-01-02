using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;
    public Image healthBar;
    public float healthAmount;
    public int maxHealth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keeps this manager between scenes
            Debug.Log("HealthManager Initialized");
        }
        else
        {
            Destroy(gameObject);  // Prevent duplicates
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = GameManager.Instance.maxPlayerHealth;
        healthAmount = GameManager.Instance.playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage){
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / maxHealth;
    }

    public void Heal(int amount){
        healthAmount += amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);

        healthBar.fillAmount = healthAmount / maxHealth;
    }
}
