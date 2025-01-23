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
        maxHealth = GameManager.Instance.MaxPlayerHealth;
        healthAmount = GameManager.Instance.PlayerHealth;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage){
        maxHealth = GameManager.Instance.MaxPlayerHealth;
        healthAmount = GameManager.Instance.PlayerHealth;
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / maxHealth;
    }

    public void Heal(int amount){
        maxHealth = GameManager.Instance.MaxPlayerHealth;
        healthAmount = GameManager.Instance.PlayerHealth;
        healthAmount += amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);

        healthBar.fillAmount = healthAmount / maxHealth;
    }
}
