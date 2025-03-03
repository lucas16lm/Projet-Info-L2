using UnityEngine;

public class Building : PlaceableObject
{
    public override void Initialize(PlaceableData placeableData, Tile position, Player player)
    {
        player.buildings.Add(this);
    }
}
