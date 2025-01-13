using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBoss : Enemy
{
    [SerializeField]
    private float timeBtwAttack;
    [SerializeField]
    float startTimeBtwAttack;
    private Queue<GameObject> miniMes = new Queue<GameObject>();
    public float launchForce;
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


    public enum BossState { Shooting, TripleShooting}
    private BossState currentBossState;
    public bool facingUp = true;

    // private void Awake()
    // {
        //thePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<IPlayer>();
        // shotPoint = transform.GetChild(0).gameObject.transform;
        // shotPointTop = transform.GetChild(1).gameObject.transform;
        // shotPointBottom = transform.GetChild(2).gameObject.transform;
    //     CreateMiniMePool(aMiniMe);
    //     currentBossState = BossState.Shooting;
    // }

    void OnEnable()
    {
        CreateMiniMePool(aMiniMe);
        currentBossState = BossState.Shooting;
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

        if(Vector2.Distance(anim.transform.position, playerPos.position) > 2.5f)
        {
            anim.transform.position = Vector2.MoveTowards(anim.transform.position, playerPos.position, currentSpeed * Time.deltaTime);
        }

        // if(Vector2.Distance(transform.position, playerPos.position) <= 2.5f)
        // {
        //     if(timeBtwAttack <= 0)
        //     {
        //         timeBtwAttack = 3f;
        //     }
        // }
        // if(timeBtwAttack >= 0)
        // {
        //     timeBtwAttack -= Time.deltaTime;
        // }

        if(timeBtwAttack <= 0)
        {
            if(currentBossState == BossState.TripleShooting)
            {
                ShootTriple();
            }else if(currentBossState == BossState.Shooting)
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
            newMiniMe.GetComponent<Rigidbody2D>().linearVelocity = direction * launchForce;
            StartCoroutine(RequeueAfterDelay(3, newMiniMe));
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
            newMiniMe.GetComponent<Rigidbody2D>().linearVelocity = direction * launchForce;
            StartCoroutine(RequeueAfterDelay(3, newMiniMe));
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
            newMiniMe.GetComponent<Rigidbody2D>().linearVelocity = direction * launchForce;
            StartCoroutine(RequeueAfterDelay(3, newMiniMe));
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
            StartCoroutine(RequeueAfterDelay(3, newMiniMe));
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
            currentBossState = BossState.TripleShooting;
        }
    }

}
