using System;
using System.Collections;
using UnityEngine;
public class Axe : Projectile
{
    Rigidbody2D rb;
    const string ENEMY_TAG = "Enemy";
    private bool frozen = false;
    [SerializeField]
    Vector3 thrownTransform;
    [SerializeField]
    float traveledDistance;
    int roationSpeed = 1080;
    bool firstPass;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
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
        }
        
    }

    void OnCollisionEnter2D(Collision2D other){
        if(!frozen)
        {
            if(other.gameObject.CompareTag(ENEMY_TAG)){
                other.gameObject.GetComponent<IEnemy>().TakeDamage(damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.damageModifier));
                Debug.Log("Dealing x damage: " + (damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.damageModifier)));
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
