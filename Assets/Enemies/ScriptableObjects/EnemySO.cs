using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public EnemyLevel enemyLevel = new EnemyLevel();
    public int hp;
    public int strength;
    public float speed;
    private Item[] loot;
    private Item[] lootTable;

    public enum EnemyLevel{
        basic,
        intermediate,
        advanced,
        legendary
    };

    public void CreateStats(GameObject gameObject){
        switch(enemyLevel){
            case EnemyLevel.basic:
                gameObject.GetComponent<IEnemy>().setHp(hp);
                gameObject.GetComponent<IEnemy>().setStrength(strength);
                gameObject.GetComponent<IEnemy>().setEnemyName(enemyName);
                gameObject.GetComponent<IEnemy>().setSpeed(speed);
                break;
            case EnemyLevel.intermediate:
                break;
            case EnemyLevel.advanced:
                break;
            case EnemyLevel.legendary:
                break;
            default:
                break;
        }
    }

    public void CreateLoot(){
        switch(enemyLevel){
            case EnemyLevel.basic:

                break;
            case EnemyLevel.intermediate:
                break;
            case EnemyLevel.advanced:
                break;
            case EnemyLevel.legendary:
                break;
            default:
                break;
        }
    }
}
