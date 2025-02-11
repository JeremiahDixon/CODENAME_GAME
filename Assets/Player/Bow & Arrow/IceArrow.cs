using UnityEngine;

public class IceArrow : Arrow
{
    const string ENEMY_TAG = "Enemy";
    const string TERRAIN_TAG = "Terrain";
    public float freezeTime;

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag(ENEMY_TAG)){
            rb.linearVelocity = Vector2.zero;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            transform.parent = other.transform;
            if (other.gameObject.GetComponent<Enemy>().CanBeKnockedBack)
            {
                Vector2 knockbackDirection = other.transform.position - GameManager.Instance.thePlayer.transform.position;
                other.gameObject.GetComponent<IEnemy>().ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
            }
            other.gameObject.GetComponent<IEnemy>().TakeDamage(damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.DamageModifier));
            Debug.Log("Dealing x damage: " + (damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.DamageModifier)));
            if(other.gameObject.activeInHierarchy){
                if(other.gameObject.GetComponent<Enemy>().IsFreezable)
                {
                    if(Random.Range(1, 101) > 75){
                        other.gameObject.GetComponent<IEnemy>().Freeze(freezeTime);
                    }
                }
            }
        }else if(other.gameObject.CompareTag(TERRAIN_TAG)){
            rb.linearVelocity = Vector2.zero;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            transform.parent = other.transform;
        }else if(other.gameObject.GetComponent<Damagable>() != null){
            other.gameObject.GetComponent<Damagable>().TakeDamage(damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.DamageModifier));
            rb.linearVelocity = Vector2.zero;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            transform.parent = other.transform;
        }
    }
}
