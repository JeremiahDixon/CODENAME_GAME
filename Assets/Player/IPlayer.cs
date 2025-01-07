using UnityEngine;

public interface IPlayer
{
    Transform transform { get; }

    public void Die();
    public void TakeDamage(int amount);
    public void Heal(int amount);
    public void ResetPlayer();

}
