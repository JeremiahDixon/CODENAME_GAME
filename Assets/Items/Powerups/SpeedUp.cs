using UnityEngine;

public class SpeedUp : Powerup
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
        player.SetActiveMovementSpeed(player.GetActiveMovementSpeed() + new Vector2(0.1f, 0.1f));
        Debug.Log("Active speed increased by 0.1, 0.1. " + player.GetActiveMovementSpeed());
    }

}
