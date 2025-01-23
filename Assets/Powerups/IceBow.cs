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
            description = "Increase the chance to freeze and damage dealt.";
            setLWeaponBowGFX.sprite = this.sprite;
            bow.arrow = this.arrow;
            bow.ClearAndRepopulateArrowQueue(arrow);
        }else
        {
            //this needs to change to increase arrow damage not all base damage
            GameManager.Instance.thePlayer.damageModifier += 0.1f;
            bow.IcreaseArrowStat("Freeze Time", 0.5f);
        }
    }
}
