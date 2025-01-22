using UnityEngine;

public class IceBow : Powerup
{
    IPlayer player;
    Bow bow;
    SpriteRenderer setLWeaponBowGFX;
    public GameObject arrow;

    
    void Awake()
    {
        powerupSO.CreateInfo(this.gameObject);
        player = GameManager.Instance.thePlayer;
        bow = GameObject.Find("Bow").GetComponent<Bow>();
        setLWeaponBowGFX = GameObject.Find("SetLWeaponBowGFX").GetComponent<SpriteRenderer>();
    }

    public override void PowerupSelected()
    {
        IncreaseLevel();
        if(powerupLevel == 1)
        {
            Debug.Log("IceBow selected for first time");
            description = "Increase the chance to freeze and damage dealt.";
            setLWeaponBowGFX.sprite = this.sprite;
            bow.arrow = this.arrow;
            bow.ClearAndRepopulateArrowQueue(arrow);
        }else
        {
            Debug.Log("IceBow selected for subsequent time");
            GameManager.Instance.thePlayer.damageModifier += 0.1f;
            bow.IcreaseArrowStat("Freeze Time", 0.5f);
        }
    }
}
