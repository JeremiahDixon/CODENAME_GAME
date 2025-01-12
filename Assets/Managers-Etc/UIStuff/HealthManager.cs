using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private Image healthBar;
    public float healthAmount;
    public int maxHealth;

    private void Awake()
    {
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = GameManager.Instance.maxPlayerHealth;
        healthAmount = GameManager.Instance.playerHealth;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage){
        maxHealth = GameManager.Instance.maxPlayerHealth;
        healthAmount = GameManager.Instance.playerHealth;
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / maxHealth;
    }

    public void Heal(int amount){
        maxHealth = GameManager.Instance.maxPlayerHealth;
        healthAmount = GameManager.Instance.playerHealth;
        healthAmount += amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);

        healthBar.fillAmount = healthAmount / maxHealth;
    }
}
