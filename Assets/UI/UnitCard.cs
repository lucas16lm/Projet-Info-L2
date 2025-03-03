using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitCard : MonoBehaviour
{
    public TMP_Text unitName;
    public Image image;
    public UnitData unitData;

    public TMP_Text goldAmount;

    private void ApplyData(UnitData unitData){
        this.unitData=unitData;
        unitName.text=unitData.elementName;
        image.sprite=unitData.image;
        goldAmount.text=""+unitData.cost.gold;
    }

    public static void Instantiate(UnitData unitData){
        GameObject card = Instantiate(GameManager.instance.uIManager.unitCard, GameManager.instance.uIManager.deploymentPanel.transform);
        card.GetComponent<UnitCard>().ApplyData(unitData);
    }
}
