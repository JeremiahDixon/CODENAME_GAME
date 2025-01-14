using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public EnemyLevel enemyLevel;
    public int maxHp;
    public int strength;
    public int scoreValue;
    float speed;
    public bool isFreezable;
    public float minSpeed;
    public float maxSpeed;
    public GameObject[] loot;

    public enum EnemyLevel{
        basic,
        intermediate,
        advanced,
        legendary
    };

    public void CreateStats(GameObject gameObject){
        Enemy enemy = gameObject.GetComponent<Enemy>();
        speed = Random.Range(minSpeed, maxSpeed);
        enemy.maxHp = maxHp;
        enemy.strength = strength;
        enemy.scoreValue = scoreValue;
        enemy.enemyName = enemyName;
        enemy.speed = speed;
        enemy.loot = loot;
        enemy.isFreezable = isFreezable;
        enemy.enemyLevel = GetEnemyLevel(enemyLevel);
    }

    public Enemy.EnemyLevel GetEnemyLevel(EnemyLevel enemyLevel)
    {
        switch(enemyLevel)
        {
            case EnemyLevel.basic:
                return Enemy.EnemyLevel.basic;
            case EnemyLevel.intermediate:
                return Enemy.EnemyLevel.intermediate;
            case EnemyLevel.advanced:
                return Enemy.EnemyLevel.advanced;
            case EnemyLevel.legendary:
                return Enemy.EnemyLevel.legendary;
            default:
                return Enemy.EnemyLevel.basic;
        }
    }

}
