using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public interface IEnemy
{
    // int getHp();
    // void setHp(int hp);
    void TakeDamage (int damage);
    void Freeze(float time);
    public void ApplyKnockback(Vector2 direction, float speed, float duration);
    // int getStrength();
    // void setStrength(int strength);
    // float getSpeed();
    // void setSpeed(float speed);
    // public string getEnemyName();
    // public void setEnemyName(string enemyName);
    
}
