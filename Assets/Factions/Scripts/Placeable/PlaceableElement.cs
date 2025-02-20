using UnityEngine;

public abstract class PlaceableElement : ScriptableObject
{
    public GameObject gameObjectPrefab;
    public string elementName;
    public int healthPoints;
    public RessourceBalance cost;
}
