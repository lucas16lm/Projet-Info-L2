using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : ScriptableObject
{
    public string unitName;
    public GameObject unitGameObject;
    public UnitClass unitClass;
    public int healthPoints;
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