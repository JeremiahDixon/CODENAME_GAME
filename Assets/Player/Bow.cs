using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Bow : MonoBehaviour
{

    public Transform shotPoint;
    public GameObject arrow;
    public float launchForce;
    private InputAction shoot;
    private InputAction look;
    private InputAction joysticklook;
    public InputActionAsset playerControls;
    private PlayerInput playerInput;
    const string ACTION_MAP = "Player";
    const string LOOK_ACTION = "Look";
    const string SHOOT_ACTION = "Attack";
    const string JOYSTICK_LOOK_ACTION = "JoystickLook";
    const string GAMEPAD_SCHEME = "Gamepad";
    const string KM_SCHEME = "Keyboard&Mouse";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerControls = playerInput.actions;
    }
    void Start()
    {
        
    }

    private void OnEnable(){
        playerControls.Enable();
        look = playerControls.FindActionMap(ACTION_MAP).FindAction(LOOK_ACTION);
        shoot = playerControls.FindActionMap(ACTION_MAP).FindAction(SHOOT_ACTION);
        joysticklook = playerControls.FindActionMap(ACTION_MAP).FindAction(JOYSTICK_LOOK_ACTION);
        shoot.Enable();
        look.Enable();
        joysticklook.Enable();
        shoot.performed += Shoot;
    }

    private void OnDisable(){
        playerControls.Disable();
        shoot.Disable();
        look.Disable();
        joysticklook.Disable();
    }

    // Update is called once per frame
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

    }

    void Shoot(InputAction.CallbackContext context){
        GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
        newArrow.GetComponent<Rigidbody2D>().linearVelocity = transform.right * launchForce;
    }
}
