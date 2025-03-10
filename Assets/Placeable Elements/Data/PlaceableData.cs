using UnityEngine;

public abstract class PlaceableData : ScriptableObject
{
    public GameObject gameObjectPrefab;
    public string elementName;
    public int baseHealthPoints;
    public RessourceBalance cost;
}
