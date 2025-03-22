using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    public PlaceableData data;

    public TMP_Text goldAmount;

    private ICardObserver observer;


    private void ApplyData(PlaceableData data){
        this.data=data;
        if(data is UnitData) image.sprite=(data as UnitData).image;
        goldAmount.text=""+data.cost.gold;
        
        foreach(Transform transform in transform.GetChild(0).GetChild(2)){
            transform.gameObject.SetActive(false);
        }

        switch(data){
            case InfantryData:
                InfantryData infantryData = data as InfantryData;
                if(infantryData.gameObjectPrefab?.GetComponent<ShieldInfantry>()!=null){
                    transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(true);
                }
                else if(infantryData.gameObjectPrefab?.GetComponent<PikeInfantry>()!=null){
                    transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(true);
                }
                else{
                    transform.GetChild(0).GetChild(2).GetChild(2).gameObject.SetActive(true);
                }
                
                break;
            case MeleeCavalryData:
                transform.GetChild(0).GetChild(2).GetChild(3).gameObject.SetActive(true);
                break;
            case RangedData:
                RangedData rangedData = data as RangedData;
                if(rangedData.gameObjectPrefab?.GetComponent<Archer>()!=null){
                    transform.GetChild(0).GetChild(2).GetChild(4).gameObject.SetActive(true);
                }
                else{
                    transform.GetChild(0).GetChild(2).GetChild(5).gameObject.SetActive(true);
                }
                break;
            case OutpostData:
                transform.GetChild(0).GetChild(2).GetChild(6).gameObject.SetActive(true);
                break;
        }
    }

    public static UnitCard Instantiate(PlaceableData data, Transform transform){
        GameObject card = Instantiate(GameManager.instance.uIManager.unitCard, transform);
        card.GetComponent<UnitCard>().ApplyData(data);
        return card.GetComponent<UnitCard>();
    }

    public void OnClick(){
        if(observer!=null){
            observer.OnCardSelected(data);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(data is UnitData){
            GameManager.instance.uIManager.cardTooltip.ShowCardTooltip(data as UnitData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.instance.uIManager.cardTooltip.HideCardTooltip();
    }


    void OnMouseExit(){
        GameManager.instance.uIManager.cardTooltip.HideCardTooltip();
    }

    public void RegisterObserver(ICardObserver observer)
    {
        this.observer = observer;
    }

    public void UnregisterObserver()
    {
        observer = null;
    }

}
