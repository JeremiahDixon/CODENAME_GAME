using UnityEngine;

public class MaxHealthUp : Powerup
{
    void Awake()
    {
        powerupSO.CreateInfo(this.gameObject);
    }

    public override void PowerupSelected()
    {
        int increaseBy = Mathf.RoundToInt(GameManager.Instance.maxPlayerHealth * 0.20f);
        GameManager.Instance.maxPlayerHealth += increaseBy;
        GameManager.Instance.playerHealth += increaseBy;
        GameManager.Instance.Heal(0);
        Debug.Log("Max health increased by 15 percent. " + GameManager.Instance.maxPlayerHealth);
        IncreaseLevel();
    }
}
