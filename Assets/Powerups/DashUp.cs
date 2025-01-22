using UnityEngine;

public class DashUp : Powerup
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
        if(powerupLevel == 1)
        {
            player.UpgradeDashSpeed(0.25f);
            description = "Upgrade dash cooldown.";
        }else if(powerupLevel == 2)
        {
            player.UpgradeDashCooldown(0.25f);
            description = "Upgrade length of dash.";
        }
        else if(powerupLevel == 3)
        {
            player.UpgradeDashLength(0.25f);
            description = "Upgrade speed of dash.";
        }
        else if(powerupLevel == 4)
        {
            player.UpgradeDashSpeed(0.25f);
            description = "Upgrade speed of dash.";
        }
        else if(powerupLevel == 5)
        {
            player.UpgradeDashSpeed(0.25f);
        }
        //Debug.Log("Damage modifier increased by 15 percent. " + player.damageModifier);
    }
}
