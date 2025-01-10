using UnityEngine;

public class IceArrow : Arrow
{
    const string ENEMY_TAG = "Enemy";
    public float freezeTime;

    void OnCollisionEnter2D(Collision2D other)
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        transform.parent = other.transform;
        if(other.gameObject.CompareTag(ENEMY_TAG)){
            other.gameObject.GetComponent<IEnemy>().TakeDamage(damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.damageModifier));
            Debug.Log("Dealing x damage: " + (damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.damageModifier)));
            if(other.gameObject.activeInHierarchy){
                if(Random.Range(1, 101) > 75){
                    other.gameObject.GetComponent<IEnemy>().Freeze(freezeTime);
                }
            }
            Destroy(gameObject);
        }else{
            transform.parent = other.transform;
            StartCoroutine(DestroyAfterDelay(5));
        }
    }
}
