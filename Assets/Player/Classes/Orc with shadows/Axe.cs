using System;
using UnityEngine;
public class Axe : MonoBehaviour
{
    Rigidbody2D rb;
    const string ENEMY_TAG = "Enemy";
    [SerializeField]
    private int damage;
    private bool frozen = false;
    int count = 0;
    [SerializeField]
    Vector3 thrownTransform;
    [SerializeField]
    float traveledDistance;
    int roationSpeed = 1080;
    bool firstPass;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable(){
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    
    void OnDisable()
    {
        frozen = false;
        GetComponent<PolygonCollider2D>().enabled = false;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.bodyType = RigidbodyType2D.Dynamic;
        roationSpeed = 1080;
        traveledDistance = 0;
        firstPass = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(firstPass){
            thrownTransform = GameObject.FindGameObjectWithTag("Player").transform.position;
            firstPass = false;
        }
        if(!frozen){
            transform.Rotate( Vector3.back.normalized * roationSpeed * Time.deltaTime );
        }
        traveledDistance = Vector3.Distance(thrownTransform, transform.position);
        if(traveledDistance >= 4.0f)
        {
            Freeze();
            count = 0;
        }
        
    }

    void OnCollisionEnter2D(Collision2D other){
        if(!frozen)
        {
            count += 1;
            if(other.gameObject.CompareTag(ENEMY_TAG)){
                other.gameObject.GetComponent<IEnemy>().TakeDamage(damage);
            }
            if(count == 3){
                Freeze();
            }else{
                rb.linearVelocity -= rb.linearVelocity * 0.2f;
                roationSpeed -= 360;
            }
        }
        if(other.gameObject.CompareTag("Terrain")){
            Freeze();
        }
    }

    void Freeze()
    {
        frozen = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        GetComponent<PolygonCollider2D>().enabled = true;
    }
}
