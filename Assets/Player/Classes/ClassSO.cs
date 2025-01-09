using UnityEngine;
[CreateAssetMenu(fileName = "ClassSO", menuName = "Scriptable Objects/ClassSO")]
public class ClassSO : ScriptableObject
{
    public Vector2 movementSpeed;
    public string className;
    public float startTimeBtwAttack;
    public int baseAttackStrength;
    public Vector2 dashSpeed;
    public float dashLength;
    public float dashCooldown;
    public int baseHp;
    public Sprite sprite;

    public void CreateClassInfo(GameObject gameObject){
        gameObject.GetComponent<IPlayer>().SetMovementSpeed(movementSpeed);
        gameObject.GetComponent<IPlayer>().SetClassName(className);
        gameObject.GetComponent<IPlayer>().SetStartTimeBtwAttack(startTimeBtwAttack);
        gameObject.GetComponent<IPlayer>().SetBaseAttackStrength(baseAttackStrength);
        gameObject.GetComponent<IPlayer>().SetDashSpeed(dashSpeed);
        gameObject.GetComponent<IPlayer>().SetDashLength(dashLength);
        gameObject.GetComponent<IPlayer>().SetDashCooldown(dashCooldown);
        gameObject.GetComponent<IPlayer>().SetBaseHp(baseHp);
        gameObject.GetComponent<IPlayer>().SetSprite(sprite);
    }
    
}
