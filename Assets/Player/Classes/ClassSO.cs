using UnityEditor.Animations;
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
    public AnimatorController animController;
    public GameObject[] classPrefabs;

    public void CreateClassInfo(GameObject gameObject){
        gameObject.GetComponent<VSPlayer>().SetMovementSpeed(movementSpeed);
        gameObject.GetComponent<VSPlayer>().SetClassName(className);
        gameObject.GetComponent<VSPlayer>().SetStartTimeBtwAttack(startTimeBtwAttack);
        gameObject.GetComponent<VSPlayer>().SetBaseAttackStrength(baseAttackStrength);
        gameObject.GetComponent<VSPlayer>().SetDashSpeed(dashSpeed);
        gameObject.GetComponent<VSPlayer>().SetDashLength(dashLength);
        gameObject.GetComponent<VSPlayer>().SetDashCooldown(dashCooldown);
        gameObject.GetComponent<VSPlayer>().SetBaseHp(baseHp);
        gameObject.GetComponent<VSPlayer>().SetSprite(sprite);
        gameObject.GetComponent<VSPlayer>().SetAnimatorController(animController);
        Instantiate(getClassPrefab(className), gameObject.transform.position, gameObject.transform.rotation);
    }

    GameObject getClassPrefab(string className)
    {
        GameObject classPrefab = null;
        for (int i = 0; i < classPrefabs.Length; i++)
        {
            if(classPrefabs[i].name == className){
                classPrefab = classPrefabs[i];
            }
        }

        return classPrefab;
    }

    
}
