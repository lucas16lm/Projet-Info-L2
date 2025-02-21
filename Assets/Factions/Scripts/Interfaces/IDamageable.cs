using UnityEngine;

public interface IDamageable
{
    int GetCurrentHealth();
    void ApplyDamage(int amount);
    void Kill();
}
