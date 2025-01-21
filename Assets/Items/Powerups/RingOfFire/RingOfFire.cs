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
            ringOfFire.GetComponent<HammerSpin>().LevelUpAbility(2);
        }else if(powerupLevel == 3)
        {
            ringOfFire.GetComponent<HammerSpin>().LevelUpAbility(3);
        }else if(powerupLevel == 4)
        {
            ringOfFire.GetComponent<HammerSpin>().LevelUpAbility(4);
        }else if(powerupLevel == 5)
        {

        }else if(powerupLevel == 6)
        {

        }else if(powerupLevel == 7)
        {

        }
    }
}
