using UnityEngine;

public class DoubleProjectile : Powerup
{
    IPlayer player;
    void Awake()
    {
        powerupSO.CreateInfo(this.gameObject);
        player = GameManager.Instance.thePlayer;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void PowerupSelected()
    {
        IncreaseLevel();
        player.isDoubleProjectile = true;
    }

}
