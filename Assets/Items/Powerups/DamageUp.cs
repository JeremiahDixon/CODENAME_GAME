using UnityEngine;

public class DamageUp : Powerup
{
    IPlayer player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.Instance.thePlayer;
        player.damageModifier += 0.15f;
        Debug.Log("Damage modifier increased by 15 percent. " + player.damageModifier);
        IncreaseLevel();
    }

}
