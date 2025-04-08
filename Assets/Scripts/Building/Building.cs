using UnityEngine;

public abstract class Building : PlaceableObject
{
    public BuildingData BuildingData{get{return data as BuildingData;}}
    public abstract bool IsConstructed();
}
