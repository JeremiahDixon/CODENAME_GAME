using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSO", menuName = "Scriptable Objects/ProjectileSO")]
public class ProjectileSO : ScriptableObject
{
    public string projectileName;
    public int damage;

     public void CreateInfo(GameObject gameObject)
    {
        gameObject.GetComponent<Projectile>().projectileName = projectileName;
        gameObject.GetComponent<Projectile>().damage = damage;
    }
}
