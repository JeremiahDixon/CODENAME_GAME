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
    public InputSystem_Actions playerControls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }
    void Start()
    {
        
    }

    private void OnEnable(){
        playerControls.Enable();
        look = playerControls.Player.Look;
        shoot = playerControls.Player.Attack;
        shoot.Enable();
        look.Enable();
        shoot.performed += Shoot;
    }

    private void OnDisable(){
        playerControls.Disable();
        shoot.Disable();
        look.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 bowPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(look.ReadValue<Vector2>());
        Vector2 direction = mousePosition - bowPosition;
        transform.right = direction;

        // if(Input.GetMouseButtonDown(0)){
        //     Shoot();
        // }
    }

    void Shoot(InputAction.CallbackContext context){
        GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
        newArrow.GetComponent<Rigidbody2D>().linearVelocity = transform.right * launchForce;
    }
}
