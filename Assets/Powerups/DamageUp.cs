using UnityEngine;

public class DamageUp : Powerup
{
    IPlayer player;
    void Awake()
    {
        powerupSO.CreateInfo(this.gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.Instance.thePlayer;
    }

    public override void PowerupSelected()
    {
        IncreaseLevel();
        player.DamageModifier += 0.15f;
        Debug.Log("Damage modifier increased by 15 percent. " + player.DamageModifier);
    }

}
