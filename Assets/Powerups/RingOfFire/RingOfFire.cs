using UnityEngine;

public class RingOfFire : Powerup
{
    IPlayer player;
    [SerializeField] GameObject hammerSpinPrefab;
    GameObject ringOfFire;
    void Awake()
    {
        powerupSO.CreateInfo(this.gameObject);
        player = GameManager.Instance.thePlayer;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PowerupSelected()
    {
        IncreaseLevel();
        if(powerupLevel == 1)
        {
            ringOfFire = Instantiate(hammerSpinPrefab, this.transform.position, Quaternion.identity);
        }else if(powerupLevel == 2)
        {
            ringOfFire.GetComponent<HammerSpin>().LevelUpAbility(2, 30, 1.25f);
        }else if(powerupLevel == 3)
        {
            ringOfFire.GetComponent<HammerSpin>().LevelUpAbility(3, 30, 1.5f);
        }else if(powerupLevel == 4)
        {
            ringOfFire.GetComponent<HammerSpin>().LevelUpAbility(4, 40, 1.5f);
        }else if(powerupLevel == 5)
        {
            ringOfFire.GetComponent<HammerSpin>().LevelUpAbility(4, 50, 1.75f);
        }else if(powerupLevel == 6)
        {
            ringOfFire.GetComponent<HammerSpin>().LevelUpAbility(5, 60, 1.75f);
        }else if(powerupLevel == 7)
        {
            ringOfFire.GetComponent<HammerSpin>().LevelUpAbility(6, 70, 2f);
        }
    }
}
