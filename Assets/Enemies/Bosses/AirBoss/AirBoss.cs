using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBoss : Enemy
{
    [SerializeField] float timeBtwAttack;
    [SerializeField] float startTimeBtwAttack;
    [SerializeField] float startTimeBtwAttackTriple;
    Queue<GameObject> miniMes = new Queue<GameObject>();
    [SerializeField] float launchForce;
    [SerializeField] float launchForceTriple;
    int miniMeLimit = 50;
    [SerializeField] GameObject aMiniMe;

    //transforms for shooting projectiles
    [SerializeField] Transform shotPoint;
    [SerializeField] Transform shotPointTop;
    [SerializeField] Transform shotPointBottom;
    [SerializeField] Transform leftRightMiddleTarget;
    [SerializeField] Transform leftRightTopTarget;
    [SerializeField] Transform leftRightBottomTarget;

    [SerializeField] Transform upMiddleShotPoint;
    [SerializeField] Transform upTopShotPoint;
    [SerializeField] Transform upBottomShotPoint;
    [SerializeField] Transform upTopTarget;
    [SerializeField] Transform UpMiddleTarget;
    [SerializeField] Transform upBottomTarget;

    [SerializeField] Transform downMiddleShotPoint;
    [SerializeField] Transform downTopShotPoint;
    [SerializeField] Transform downBottomShotPoint;
    [SerializeField] Transform downTopTarget;
    [SerializeField] Transform downMiddleTarget;
    [SerializeField] Transform downBottomTarget;

    //references that change on the direction to shoot based on where player is
    [SerializeField] Transform topAttackPosToUse;
    [SerializeField] Transform middleAttackPosToUse;
    [SerializeField] Transform bottomAttackPosToUse;
    [SerializeField] Transform topTargetToUse;
    [SerializeField] Transform middleTargetToUse;
    [SerializeField] Transform bottomTargetToUse;

    public enum BossState { StageOne, StageTwo, StageThree, Transition}
    public BossState currentBossState {get; private set;}

    void Start()
    {
        CreateMiniMePool(aMiniMe);
        currentBossState = BossState.StageOne;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerPos.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }else if(playerPos.position.x < transform.position.x && facingRight)
        {
            Flip();
        }

        if(playerPos.position.y > transform.position.y + 1.5)
        {
            topAttackPosToUse = upTopShotPoint;
            middleAttackPosToUse = upMiddleShotPoint;
            bottomAttackPosToUse = upBottomShotPoint;
            topTargetToUse = upTopTarget;
            middleTargetToUse = UpMiddleTarget;
            bottomTargetToUse = upBottomTarget;
        }

        if(playerPos.position.y < transform.position.y - 1.5)
        {
            topAttackPosToUse = downTopShotPoint;
            middleAttackPosToUse = downMiddleShotPoint;
            bottomAttackPosToUse = downBottomShotPoint;
            topTargetToUse = downTopTarget;
            middleTargetToUse = downMiddleTarget;
            bottomTargetToUse = downBottomTarget;
        }

        if(playerPos.position.x > transform.position.x + 1.5 || playerPos.position.x < transform.position.x - 1.5)
        {
		    topAttackPosToUse = shotPointTop;
		    middleAttackPosToUse = shotPoint;
            bottomAttackPosToUse = shotPointBottom;
            topTargetToUse = leftRightTopTarget;
            middleTargetToUse = leftRightMiddleTarget;
            bottomTargetToUse = leftRightBottomTarget;
        }

        if(playerPos.position.y - anim.transform.position.y > 0.2f || playerPos.position.y - anim.transform.position.y < -0.2f)
        {
            anim.transform.position = Vector2.MoveTowards(anim.transform.position, new Vector2(Camera.main.ViewportToWorldPoint(new Vector3(0.8f, playerPos.position.y, 0)).x, playerPos.position.y), currentSpeed * Time.deltaTime);
        }

        if(timeBtwAttack <= 0)
        {
            if(currentBossState == BossState.StageTwo)
            {
                ShootTriple();
            }else if(currentBossState == BossState.StageOne)
            {
                Shoot();
            }
            timeBtwAttack = startTimeBtwAttack;
        }else
        {
            timeBtwAttack -= Time.deltaTime;
        }

    }

    void ShootTriple(){

        if(miniMes.Count > 0){
            GameObject newMiniMe = miniMes.Dequeue();
            if(facingRight)
            {
                newMiniMe.GetComponent<SpriteRenderer>().flipX = true;
            }
            newMiniMe.SetActive(true);
            newMiniMe.GetComponent<BoxCollider2D>().enabled = true;
            newMiniMe.transform.position = middleAttackPosToUse.position;
            newMiniMe.transform.rotation = middleAttackPosToUse.rotation;
            Vector3 direction = (middleTargetToUse.position - middleAttackPosToUse.position).normalized; 
            newMiniMe.GetComponent<Rigidbody2D>().linearVelocity = direction * launchForceTriple;
            StartCoroutine(RequeueAfterDelay(4, newMiniMe));
        }

        if(miniMes.Count > 0){
            GameObject newMiniMe = miniMes.Dequeue();
            if(facingRight)
            {
                newMiniMe.GetComponent<SpriteRenderer>().flipX = true;
            }
            newMiniMe.SetActive(true);
            newMiniMe.GetComponent<BoxCollider2D>().enabled = true;
            newMiniMe.transform.position = topAttackPosToUse.position;
            newMiniMe.transform.rotation = topAttackPosToUse.rotation;
            Vector3 direction = (topTargetToUse.position - topAttackPosToUse.position).normalized; 
            newMiniMe.GetComponent<Rigidbody2D>().linearVelocity = direction * launchForceTriple;
            StartCoroutine(RequeueAfterDelay(4, newMiniMe));
        }

        if(miniMes.Count > 0){
            GameObject newMiniMe = miniMes.Dequeue();
            if(facingRight)
            {
                newMiniMe.GetComponent<SpriteRenderer>().flipX = true;
            }
            newMiniMe.SetActive(true);
            newMiniMe.GetComponent<BoxCollider2D>().enabled = true;
            newMiniMe.transform.position = bottomAttackPosToUse.position;
            newMiniMe.transform.rotation = bottomAttackPosToUse.rotation;
            Vector3 direction = (bottomTargetToUse.position - bottomAttackPosToUse.position).normalized; 
            newMiniMe.GetComponent<Rigidbody2D>().linearVelocity = direction * launchForceTriple;
            StartCoroutine(RequeueAfterDelay(4, newMiniMe));
        }

    }

    void Shoot(){

        if(miniMes.Count > 0){
            GameObject newMiniMe = miniMes.Dequeue();
            if(facingRight)
            {
                newMiniMe.GetComponent<SpriteRenderer>().flipX = true;
            }
            newMiniMe.SetActive(true);
            newMiniMe.GetComponent<BoxCollider2D>().enabled = true;
            newMiniMe.transform.position = middleAttackPosToUse.position;
            newMiniMe.transform.rotation = middleAttackPosToUse.rotation;
            Vector3 direction = (middleTargetToUse.position - middleAttackPosToUse.position).normalized; 
            newMiniMe.GetComponent<Rigidbody2D>().linearVelocity = direction * launchForce;
            StartCoroutine(RequeueAfterDelay(4, newMiniMe));
        }
    }

    IEnumerator RequeueAfterDelay(int seconds, GameObject newMiniMe)
    {
        yield return new WaitForSeconds(seconds);
        if(newMiniMe != null)
        {
            newMiniMe.GetComponent<SpriteRenderer>().flipX = false;
            newMiniMe.SetActive(false);
            miniMes.Enqueue(newMiniMe);
        }
    }

    void CreateMiniMePool(GameObject miniMe)
    {
        for (int i = 0; i < miniMeLimit; i++)
        {
            GameObject newMiiniMe = Instantiate(miniMe, shotPoint.position, shotPoint.rotation);
            newMiiniMe.SetActive(false);
            miniMes.Enqueue(newMiiniMe);
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if(hp <= maxHp * 0.66f)
        {
            currentBossState = BossState.StageTwo;
            startTimeBtwAttack = startTimeBtwAttackTriple;
        }
    }

}
