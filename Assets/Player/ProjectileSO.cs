using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSO", menuName = "Scriptable Objects/ProjectileSO")]
public class ProjectileSO : ScriptableObject
{
    public string projectileName;
    public int damage;

     public void CreateInfo(GameObject gameObject)
    {
        gameObject.GetComponent<Projectile>().ProjectileName = projectileName;
        gameObject.GetComponent<Projectile>().Damage = damage;
    }
}
