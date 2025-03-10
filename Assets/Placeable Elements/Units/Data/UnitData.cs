using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : PlaceableData
{
    public Sprite image;
    public UnitClass unitClass;
    public int baseDamagePoints;
    public int baseMovementPoints;
}

public enum UnitClass{
    Infantry,
    Ranged,
    MeleeCavalry,
    RangedCavalry,
    Engineer
}