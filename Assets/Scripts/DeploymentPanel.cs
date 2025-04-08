using UnityEngine;

public class DeploymentPanel : MonoBehaviour, ICardObserver
{
    [SerializeField]
    private Transform cardContainer;
    private UnitData selectedUnit;

    public UnitData GetSelectedUnit(){
        return selectedUnit;
    }

    public void OpenFor(Player player)
    {
        cardContainer.gameObject.SetActive(true);
        GameManager.instance.uIManager.ressourcePanel.SetActive(true);
        Clear();
        foreach(UnitData unitData in player.factionData.factionUnitsData){
            UnitCard.Instantiate(unitData, cardContainer).RegisterObserver(this);
        }
    }

    public void Close()
    {
        selectedUnit=null;
        Clear();
        cardContainer.gameObject.SetActive(false);
        GameManager.instance.uIManager.ressourcePanel.SetActive(false);
        GameManager.instance.uIManager.cardTooltip.HideCardTooltip();
    }

    private void Clear(){
        foreach(Transform child in cardContainer.transform){
            if(child.name != "Ressouce UI") Destroy(child.gameObject);
        }
    }

    public void OnCardSelected(PlaceableData data)
    {
        selectedUnit= data as UnitData;
    }
}
