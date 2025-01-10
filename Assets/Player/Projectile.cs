using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public string projectileName;
    public ProjectileSO pso;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        pso.CreateInfo(this.gameObject);
    }

}
