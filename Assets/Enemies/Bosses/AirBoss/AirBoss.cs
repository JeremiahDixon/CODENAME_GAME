using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBoss : Enemy
{
    [SerializeField]
    private float timeBtwAttack;
    [SerializeField]
    float startTimeBtwAttack;
    [SerializeField]
    float startTimeBtwAttackTriple;
    private Queue<GameObject> miniMes = new Queue<GameObject>();
    public float launchForce;
    public float launchForceTriple;
    int miniMeLimit = 50;
    public GameObject aMiniMe;

    [SerializeField]
    private Transform shotPoint;
    [SerializeField]
    private Transform shotPointTop;
    [SerializeField]
    private Transform shotPointBottom;
    [SerializeField]
    private Transform leftRightMiddleTarget;
    [SerializeField]
    private Transform leftRightTopTarget;
    [SerializeField]
    private Transform leftRightBottomTarget;

    [SerializeField]
    private Transform upMiddleShotPoint;
    [SerializeField]
    private Transform upTopShotPoint;
    [SerializeField]
    private Transform upBottomShotPoint;
    [SerializeField]
    private Transform upTopTarget;
    [SerializeField]
    private Transform UpMiddleTarget;
    [SerializeField]
    private Transform upBottomTarget;

    [SerializeField]
    private Transform downMiddleShotPoint;
    [SerializeField]
    private Transform downTopShotPoint;
    [SerializeField]
    private Transform downBottomShotPoint;
    [SerializeField]
    private Transform downTopTarget;
    [SerializeField]
    private Transform downMiddleTarget;
    [SerializeField]
    private Transform downBottomTarget;

    public Transform topAttackPosToUse;
    public Transform middleAttackPosToUse;
    public Transform bottomAttackPosToUse;
    public Transform topTargetToUse;
    public Transform middleTargetToUse;
    public Transform bottomTargetToUse;


    public enum BossState { StageOne, StageTwo, StageThree, Transition}
    private BossState currentBossState;
    public bool facingUp = true;

    void OnEnable()
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

    private IEnumerator RequeueAfterDelay(int seconds, GameObject newMiniMe)
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
