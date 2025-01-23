using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bow : MonoBehaviour
{
    [SerializeField] Transform shotPoint;
    public GameObject arrow;
    [SerializeField] float launchForce;
    InputAction shoot;
    InputAction look;
    InputAction joysticklook;
    [SerializeField] InputActionAsset playerControls;
    PlayerInput playerInput;
    const string ACTION_MAP = "Player";
    const string LOOK_ACTION = "Look";
    const string SHOOT_ACTION = "Attack";
    const string JOYSTICK_LOOK_ACTION = "JoystickLook";
    const string GAMEPAD_SCHEME = "Gamepad";
    const string KM_SCHEME = "Keyboard&Mouse";
    const string PLAYER_TAG = "Player";
    const string FREEZE_STAT_NAME = "Freeze Time";
    IPlayer thePlayer;
    int arrowLimit = 40;
    Queue<GameObject> arrows = new Queue<GameObject>();
    List<GameObject> dequeuedArrows = new List<GameObject>();
    [SerializeField] float timeBtwAttack;
    [SerializeField] float startTimeBtwAttack;
    float delayLength = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        thePlayer = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<IPlayer>();
        playerInput = GetComponentInParent<PlayerInput>();
        playerControls = playerInput.actions;
        shotPoint = transform.GetChild(1).gameObject.transform;
        CreateArrowPool(arrow);
    }

    void OnEnable(){
        playerControls.Enable();
        look = playerControls.FindActionMap(ACTION_MAP).FindAction(LOOK_ACTION);
        shoot = playerControls.FindActionMap(ACTION_MAP).FindAction(SHOOT_ACTION);
        joysticklook = playerControls.FindActionMap(ACTION_MAP).FindAction(JOYSTICK_LOOK_ACTION);
        shoot.Enable();
        look.Enable();
        joysticklook.Enable();
    }

    void OnDisable(){
        if(playerControls != null){
            playerControls.Disable();
            shoot.Disable();
            look.Disable();
            joysticklook.Disable();
        }
    }

    void Update()
    {
        if(playerInput.currentControlScheme == GAMEPAD_SCHEME){
            Vector2 joystickPos = joysticklook.ReadValue<Vector2>();
            if(joystickPos != new Vector2(0, 0)){
                transform.right = joystickPos;
            }
        }else if(playerInput.currentControlScheme == KM_SCHEME){
            Vector2 bowPosition = transform.position;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(look.ReadValue<Vector2>());
            Vector2 direction = mousePosition - bowPosition;
            transform.right = direction;
        }

        if(timeBtwAttack <= 0)
        {
            if(shoot.IsPressed())
            {
                timeBtwAttack = startTimeBtwAttack;
                Shoot();
                if(GameManager.Instance.thePlayer.IsDoubleProjectile)
                {
                    StartCoroutine(ShootAgain(startTimeBtwAttack * 0.25f));
                }
            }

        }else
        {
            timeBtwAttack -= Time.deltaTime;
        }

    }

    void Shoot(){

        if(arrows.Count > 0){
            GameObject newArrow = arrows.Dequeue();
            dequeuedArrows.Add(newArrow);
            newArrow.SetActive(true);
            newArrow.GetComponent<BoxCollider2D>().enabled = true;
            newArrow.transform.position = shotPoint.position;
            newArrow.transform.rotation = shotPoint.rotation;
            newArrow.GetComponent<Rigidbody2D>().linearVelocity = transform.right * launchForce;
            StartCoroutine(RequeueAfterDelay(delayLength, newArrow));
        }

    }

    IEnumerator RequeueAfterDelay(float seconds, GameObject arrow)
    {
        yield return new WaitForSeconds(seconds);
        if(arrow != null)
        {
            dequeuedArrows.Remove(arrow);
            arrow.transform.parent = null;
            arrow.SetActive(false);
            arrows.Enqueue(arrow);
        }
    }

    IEnumerator ShootAgain(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Shoot();
    }

    void CreateArrowPool(GameObject arrow)
    {
        for (int i = 0; i < arrowLimit; i++)
        {
            GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
            newArrow.SetActive(false);
            arrows.Enqueue(newArrow);
        }
    }

    public void ClearAndRepopulateArrowQueue(GameObject arrow)
    {
        foreach (GameObject gObj in arrows)
        {
            Destroy(gObj);
        }
        arrows.Clear();
        foreach (GameObject gObj in dequeuedArrows)
        {
            Destroy(gObj);
        }
        dequeuedArrows.Clear();
        CreateArrowPool(arrow);
    }

    public void IcreaseArrowStat(string stat, float amount)
    {
        switch(stat)
        {
            case FREEZE_STAT_NAME:
                foreach (GameObject arrow in arrows)
                {
                    arrow.GetComponent<IceArrow>().freezeTime += amount;
                }
                break;
            default:
                break;
        }
    }

}
