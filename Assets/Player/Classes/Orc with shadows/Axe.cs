using System;
using System.Collections;
using UnityEngine;
public class Axe : Projectile
{
    Rigidbody2D rb;
    const string ENEMY_TAG = "Enemy";
    const string PLAYER_TAG = "Player";
    bool frozen = false;
    [SerializeField] Vector3 thrownTransform;
    [SerializeField] float traveledDistance;
    int roationSpeed = 1080;
    bool firstPass;
    [SerializeField] float knockbackForce = 2f;
    [SerializeField] float knockbackDuration = 0.15f;
    float maxDistance = 3.5f;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
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

    void Update()
    {
        if(firstPass){
            thrownTransform = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform.position;
            firstPass = false;
        }
        if(!frozen){
            transform.Rotate( Vector3.back.normalized * roationSpeed * Time.deltaTime );
        }
        traveledDistance = Vector3.Distance(thrownTransform, transform.position);
        if(traveledDistance >= maxDistance)
        {
            Freeze();
        }
        
    }

    void OnCollisionEnter2D(Collision2D other){
        if(!frozen)
        {
            if(other.gameObject.CompareTag(ENEMY_TAG)){
                if (other.gameObject.GetComponent<Enemy>().CanBeKnockedBack)
                {
                    Vector2 knockbackDirection = other.transform.position - GameManager.Instance.thePlayer.transform.position;
                    other.gameObject.GetComponent<IEnemy>().ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
                }
                other.gameObject.GetComponent<IEnemy>().TakeDamage(damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.DamageModifier));
                Debug.Log("Dealing x damage: " + (damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.DamageModifier)));
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

    public void IncreaseMaxDistance(float percentage)
    {
        maxDistance += maxDistance * percentage;
    }
}
