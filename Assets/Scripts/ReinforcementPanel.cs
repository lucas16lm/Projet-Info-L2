using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReinforcementPanel : MonoBehaviour, ICardObserver
{
    [SerializeField]
    private Transform closeButtonTransform;
    [SerializeField]
    private Transform cardContainer; 

    private PlaceableData selected;

    public IEnumerator OpenFor(Player player, Action<PlaceableData> onComplete)
    {
        bool canceled = false;
        if(closeButtonTransform!=null){
            closeButtonTransform.GetComponent<Button>().onClick.AddListener(()=>canceled=true);
        }
        

        Time.timeScale=0;
        Cursor.lockState=CursorLockMode.Confined;
        closeButtonTransform.gameObject.SetActive(true);
        cardContainer.gameObject.SetActive(true);
        Clear();
        
        UnitCard.Instantiate(player.factionData.outpostData, cardContainer).RegisterObserver(this);
        foreach(UnitData unitData in player.factionData.factionUnitsData){
            UnitCard.Instantiate(unitData, cardContainer).RegisterObserver(this);
        }
        yield return new WaitUntil(()=>selected!=null || canceled);
        if(selected!=null){
            onComplete?.Invoke(selected);
            selected=null;
        };
        Close();
    }

    public IEnumerator OpenForBuildings(Player player, Action<PlaceableData> onComplete)
    {
        bool canceled = false;
        if(closeButtonTransform!=null){
            closeButtonTransform.GetComponent<Button>().onClick.AddListener(()=>canceled=true);
        }
        

        Time.timeScale=0;
        Cursor.lockState=CursorLockMode.Confined;
        closeButtonTransform.gameObject.SetActive(true);
        cardContainer.gameObject.SetActive(true);
        Clear();
        
        UnitCard.Instantiate(player.factionData.outpostData, cardContainer).RegisterObserver(this);
        
        yield return new WaitUntil(()=>selected!=null || canceled);
        if(selected!=null){
            onComplete?.Invoke(selected);
            selected=null;
        };
        Close();
    }

    public void Close()
    {
        Time.timeScale=1;
        Cursor.lockState=CursorLockMode.Locked;
        Clear();
        closeButtonTransform.gameObject.SetActive(false);
        cardContainer.gameObject.SetActive(false);
        GameManager.instance.uIManager.cardTooltip.HideCardTooltip();
    }

    private void Clear(){
        foreach(Transform child in cardContainer.transform){
            if(child.name != "Clock") Destroy(child.gameObject);
        }
    }

    public void OnCardSelected(PlaceableData data)
    {
        selected = data;
        Debug.Log("Unité sélectionnée : " + selected.elementName);
    }
}

public interface ICardObserver
{
    void OnCardSelected(PlaceableData data);
}
