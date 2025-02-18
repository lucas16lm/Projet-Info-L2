using UnityEngine;

public class Faction : MonoBehaviour
{
    public FactionData data;

    void Start()
    {
        transform.name=data.name;
    }

    public void PlaceGeneral(Tile tile){
        GameObject general = Instantiate(data.generalTower, tile.gameObject.transform.position+(tile.gameObject.transform.localScale.y*Vector3.up)/2, Quaternion.identity, transform);
    }

    public void PlaceUnit(Unit unit, Tile tile){
        GameObject general = Instantiate(unit.data.unitGameObject, tile.gameObject.transform.position+(tile.gameObject.transform.localScale.y*Vector3.up)/2, Quaternion.identity, transform);
    }

    public void PlaceBuilding(){
        //TODO
    }

    public void SelectUnit(){
        //TODO
    }
}
