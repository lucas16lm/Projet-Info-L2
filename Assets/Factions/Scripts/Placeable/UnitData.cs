using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : PlaceableElement
{
    public Sprite image;
    public UnitClass unitClass;
    public int baseDamagePoints;
    public int baseMovementPoints;
    public SpecialAction specialAction;
}

public enum UnitClass{
    Infantry,
    Cavalry,
    Artillery,
    Engineer
}