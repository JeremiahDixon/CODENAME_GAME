using UnityEngine;

public interface IPlayer
{
    Transform transform { get; }
    public int CurrentAttackStrength{get; set;}
    public bool IsDoubleProjectile{ get; set;}
    public float DamageModifier{get; set;}
    public void Die();
    public void TakeDamage(int amount);
    public void Heal(int amount);
    public void ResetPlayer();
    public void SetDashSpeed(Vector2 dashSpeed);
    public void SetClassName(string className);
    public void SetStartTimeBtwAttack(float startTimeBtwAttack);
    public void SetDashLength(float dashLength);
    public void SetDashCooldown(float dashCooldown);
    public void SetBaseAttackStrength(int baseAttackStrength);
    public void SetBaseHp(int baseHp);
    public void SetSprite(Sprite sprite);
    public void SetMovementSpeed(Vector2 movementSpeed);
    public Vector2 GetActiveMovementSpeed();
    public void SetActiveMovementSpeed(Vector2 movementSpeed);
    public void UpgradeDashSpeed(float percent);
    public void UpgradeDashLength(float percent);
    public void UpgradeDashCooldown(float percent);
    
}
