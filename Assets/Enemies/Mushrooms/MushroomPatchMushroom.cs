using System.Linq;
using UnityEngine;

public class MushroomPatchMushroom : Enemy
{
    [SerializeField]
    private float timeBtwAttack;
    const string ATTACKING_TRIGGER = "isAttackingTrigger";
    [SerializeField] Transform attackPos;
    [SerializeField] float attackRange;
    [SerializeField] MushroomGarden mg;
    void Update()
    {
        if(Vector2.Distance(anim.transform.position, playerPos.position) > 0.75f)
        {
            anim.transform.position = Vector2.MoveTowards(anim.transform.position, playerPos.position, currentSpeed * Time.deltaTime);
        }
        if(Vector2.Distance(transform.position, playerPos.position) <= 0.75f)
        {
            anim.SetBool(RUNNING, false);
            if(timeBtwAttack <= 0)
            {
                timeBtwAttack = 3f;
                anim.SetTrigger(ATTACKING_TRIGGER);
            }
        }else
        {
            anim.SetBool(RUNNING, true);
        }
        if(timeBtwAttack >= 0)
        {
            timeBtwAttack -= Time.deltaTime;
        }
        if(playerPos.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }else if(playerPos.position.x < transform.position.x && facingRight)
        {
            Flip();
        }
    }

    //call this durring animation event to check if hit
    void tryToHitPlayer()
    {
        //create a attackpos and replace transform
        RaycastHit2D playerToDamage = Physics2D.CircleCast(attackPos.position, attackRange, transform.right, 0f, whatIsPlayer);
        if( playerToDamage )
        {
            IPlayer thePlayer = playerToDamage.collider.gameObject.GetComponent<IPlayer>();
            if(thePlayer != null && playerToDamage.collider is BoxCollider2D){
                thePlayer.TakeDamage(strength);
                //might need to add a bool playerBeenDamaged
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    public override void TakeDamage(int damage)
    {
        hp -= damage;
        if(damagedClips.Length > 0)
        {
            int randInt = Random.Range(0, damagedClips.Length);
            SoundManager.Instance.PlaySoundEffect(damagedClips[randInt], transform, _soundFxVolume);
        }
        if (hp <= 0)
        {
            playManager.IncreaseScore(scoreValue);
            if(loot.Length > 0)
            {
                float randomInt = Random.Range(0f, 100.0f);
                dropLoot(randomInt);
            }
            GetComponent<SpriteRenderer>().color = Color.white;
            currentSpeed = speed;
            hp = maxHp;
            foreach(Projectile projectile in GetComponentsInChildren<Projectile>().ToList())
            {
                projectile.gameObject.transform.parent = null;
                projectile.gameObject.SetActive(false);
            }
            mg.ReturnObject(this.gameObject);
        }
    }
}
