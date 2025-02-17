using UnityEngine;

[CreateAssetMenu(fileName = "UnifInfo", menuName = "Scriptable Objects/UnifInfo")]
public class UnitInfo : ScriptableObject
{
    public string unitName;
    public GameObject troopGameObject;
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