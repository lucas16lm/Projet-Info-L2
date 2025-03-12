using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitCard : MonoBehaviour
{
    public TMP_Text unitName;
    public Image image;
    public PlaceableData data;

    public TMP_Text goldAmount;

    private ICardObserver observer;


    private void ApplyData(PlaceableData data){
        this.data=data;
        unitName.text=data.elementName;
        if(data is UnitData) image.sprite=(data as UnitData).image;
        goldAmount.text=""+data.cost.gold;
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

    public void RegisterObserver(ICardObserver observer)
    {
        this.observer = observer;
    }

    public void UnregisterObserver()
    {
        observer = null;
    }

}
