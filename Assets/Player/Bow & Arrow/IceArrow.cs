using UnityEngine;

public class IceArrow : Arrow
{
    const string ENEMY_TAG = "Enemy";
    public float freezeTime;

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag(ENEMY_TAG)){
            rb.linearVelocity = Vector2.zero;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            transform.parent = other.transform;
            Vector2 knockbackDirection = other.transform.position - transform.position;
            other.gameObject.GetComponent<IEnemy>().ApplyKnockback(knockbackDirection, knockbackForce, knockbackDuration);
            other.gameObject.GetComponent<IEnemy>().TakeDamage(damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.damageModifier));
            Debug.Log("Dealing x damage: " + (damage + Mathf.RoundToInt(damage * GameManager.Instance.thePlayer.damageModifier)));
            if(other.gameObject.activeInHierarchy){
                if(other.gameObject.GetComponent<Enemy>().isFreezable)
                {
                    if(Random.Range(1, 101) > 75){
                        other.gameObject.GetComponent<IEnemy>().Freeze(freezeTime);
                    }
                }
            }
        }else if(other.gameObject.CompareTag("Terrain")){
            rb.linearVelocity = Vector2.zero;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            transform.parent = other.transform;
        }
    }
}
