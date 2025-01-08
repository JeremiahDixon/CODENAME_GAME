using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class AxeThrow : MonoBehaviour
{
    [SerializeField]
    private Transform axePos;
    public GameObject axe;
    public float launchForce;
    private InputAction throwAxe;
    private InputAction look;
    private InputAction joysticklook;
    public InputActionAsset playerControls;
    private PlayerInput playerInput;
    const string ACTION_MAP = "Player";
    const string LOOK_ACTION = "Look";
    const string THROW_ACTION = "Attack";
    const string JOYSTICK_LOOK_ACTION = "JoystickLook";
    const string GAMEPAD_SCHEME = "Gamepad";
    const string KM_SCHEME = "Keyboard&Mouse";
    private IPlayer thePlayer;
    private int axeLimit = 4;
    private Queue<GameObject> axes = new Queue<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        thePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<IPlayer>();
        transform.parent = thePlayer.transform;
        playerInput = GetComponentInParent<PlayerInput>();
        playerControls = playerInput.actions;
        axePos = transform.GetChild(0).gameObject.transform;
    }

    private void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
        playerControls.Enable();
        look = playerControls.FindActionMap(ACTION_MAP).FindAction(LOOK_ACTION);
        throwAxe = playerControls.FindActionMap(ACTION_MAP).FindAction(THROW_ACTION);
        joysticklook = playerControls.FindActionMap(ACTION_MAP).FindAction(JOYSTICK_LOOK_ACTION);
        throwAxe.Enable();
        look.Enable();
        joysticklook.Enable();
        throwAxe.performed += ThrowAxe;
    }
    void Start()
    {
        CreateObjectPool();
    }

    private void OnDisable(){
        if(playerControls != null){
            playerControls.Disable();
            throwAxe.Disable();
            look.Disable();
            joysticklook.Disable();
            throwAxe.performed -= ThrowAxe;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "VS"){
            ClearTheQueue();
            CreateObjectPool();
        }
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

    void CreateObjectPool()
    {
        for (int i = 0; i < axeLimit; i++)
        {
            GameObject newAxe = Instantiate(axe, axePos.position, axePos.rotation);
            newAxe.SetActive(false);
            axes.Enqueue(newAxe);
        }
    }

    void ClearTheQueue()
    {
        axes.Clear();
    }

    void ThrowAxe(InputAction.CallbackContext context){
        if(axes.Count > 0){
            GameObject newAxe = axes.Dequeue();
            newAxe.SetActive(true);
            newAxe.transform.position = axePos.position;
            newAxe.transform.rotation = axePos.rotation;
            newAxe.GetComponent<Rigidbody2D>().linearVelocity = transform.right * launchForce;
        }
    }

    public void RequeueAxe(GameObject axe)
    {
        axe.SetActive(false);
        axes.Enqueue(axe);
    }

}
