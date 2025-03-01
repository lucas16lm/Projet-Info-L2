using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCard : MonoBehaviour
{
    public TMP_Text unitName;
    public Image backGround;

    public TMP_Text goldAmount;

    public void ApplyData(UnitData unitData){
        unitName.text=unitData.elementName;
        backGround.sprite=unitData.image;
        goldAmount.text=""+unitData.cost.gold;
    }
}
