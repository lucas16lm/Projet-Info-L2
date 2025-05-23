using UnityEngine;

[CreateAssetMenu(fileName = "RangedData", menuName = "Scriptable Objects/Unit/RangedData")]
public class RangedData : UnitData
{
    public int attackRange;
    public GameObject projectile;
    public float shootAngle;
    public float projectileSpeed;
    public float projectilePrecision;
}
