using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCard : MonoBehaviour
{
    public TMP_Text unitName;
    public Image image;
    public UnitData unitData;

    public TMP_Text goldAmount;

    public void ApplyData(UnitData unitData){
        this.unitData=unitData;
        unitName.text=unitData.elementName;
        image.sprite=unitData.image;
        goldAmount.text=""+unitData.cost.gold;
    }
}
