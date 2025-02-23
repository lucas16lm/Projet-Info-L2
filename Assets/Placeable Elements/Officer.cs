using UnityEngine;

public class Officer : PlaceableObject
{
    public override void Initialize(PlaceableData placeableData, Tile position)
    {
        healthPoints=placeableData.baseHealthPoints;
        this.position=position;
        position.occupied=true;
    }
}
