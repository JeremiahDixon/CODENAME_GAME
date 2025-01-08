using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public EnemyLevel enemyLevel = new EnemyLevel();
    public int hp;
    public int strength;
    float speed;
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
        enemy.hp = hp;
        enemy.strength = strength;
        enemy.enemyName = enemyName;
        enemy.speed = speed;
        enemy.loot = loot;
    }

}
