using UnityEngine;

public class MaxHealthUp : Powerup
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int increaseBy = Mathf.RoundToInt(GameManager.Instance.maxPlayerHealth * 0.20f);
        GameManager.Instance.maxPlayerHealth += increaseBy;
        GameManager.Instance.playerHealth += increaseBy;
        Debug.Log("Max health increased by 15 percent. " + GameManager.Instance.maxPlayerHealth);
        IncreaseLevel();
    }
}
