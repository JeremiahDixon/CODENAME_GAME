using UnityEngine;

public class MaxHealthUp : Powerup
{
    void Awake()
    {
        powerupSO.CreateInfo(this.gameObject);
    }

    public override void PowerupSelected()
    {
        int increaseBy = Mathf.RoundToInt(GameManager.Instance.MaxPlayerHealth * 0.20f);
        GameManager.Instance.IncreaseMaxHealth(increaseBy);
        GameManager.Instance.Heal(increaseBy);
        Debug.Log("Max health increased by 15 percent. " + GameManager.Instance.MaxPlayerHealth);
        IncreaseLevel();
    }
}
