using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public EnemyLevel enemyLevel = new EnemyLevel();
    public int hp;
    public int strength;
    public float speed;
    public GameObject[] loot;

    public enum EnemyLevel{
        basic,
        intermediate,
        advanced,
        legendary
    };

    public void CreateStats(GameObject gameObject){
        gameObject.GetComponent<Enemy>().hp = hp;
        gameObject.GetComponent<Enemy>().strength = strength;
        gameObject.GetComponent<Enemy>().enemyName = enemyName;
        gameObject.GetComponent<Enemy>().speed = speed;
        gameObject.GetComponent<Enemy>().loot = loot;
    }

}
