using UnityEngine;

public class IceArrow : Arrow
{
    const string ENEMY_TAG = "Enemy";
    public float freezeTime;

    void OnCollisionEnter2D(Collision2D other)
    {
        rb.linearVelocity = Vector2.zero;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        if(other.gameObject.CompareTag(ENEMY_TAG)){
            transform.parent = other.transform;
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
            transform.parent = other.transform;
        }
    }
}
