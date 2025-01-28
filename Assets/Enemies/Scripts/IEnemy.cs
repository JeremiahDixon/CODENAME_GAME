using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public interface IEnemy
{
    void TakeDamage (int damage);
    void Freeze(float time);
    void ApplyKnockback(Vector2 direction, float speed, float duration);
    bool CanBeKnockedBack{ get; set;}
}
