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
        CreateSelf();
        IncreaseLevel();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateSelf()
    {
        setLWeaponBowGFX.sprite = this.sprite;
        bow.arrow = this.arrow;
    }
}
