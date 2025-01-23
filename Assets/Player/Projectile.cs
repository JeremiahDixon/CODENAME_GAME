using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected int damage; public int Damage{get => damage; set => damage = value;}
    [SerializeField] protected string projectileName; public string ProjectileName{get => projectileName; set => projectileName = value;}
    [SerializeField] protected ProjectileSO pso;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        pso.CreateInfo(this.gameObject);
    }

}
