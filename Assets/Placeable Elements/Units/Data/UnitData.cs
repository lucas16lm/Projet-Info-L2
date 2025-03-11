using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : PlaceableData
{
    public Sprite image;
    public int baseDamagePoints;
    public int baseMovementPoints;
    public float timeToMove;
}