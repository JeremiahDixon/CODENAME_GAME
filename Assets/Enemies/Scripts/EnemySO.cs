using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    [SerializeField] string enemyName;
    [SerializeField] EnemyLevel enemyLevel;
    [SerializeField] int maxHp;
    [SerializeField] int strength;
    [SerializeField] int scoreValue;
    float speed;
    [SerializeField] bool isFreezable;
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] GameObject[] loot;

    public enum EnemyLevel{
        basic,
        intermediate,
        advanced,
        legendary,
        shooter
    };

    public void CreateStats(GameObject gameObject){
        Enemy enemy = gameObject.GetComponent<Enemy>();
        speed = Random.Range(minSpeed, maxSpeed);
        enemy.MaxHp = maxHp;
        enemy.Strength = strength;
        enemy.ScoreValue = scoreValue;
        enemy.EnemyName = enemyName;
        enemy.Speed = speed;
        enemy.Loot = loot;
        enemy.IsFreezable = isFreezable;
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
