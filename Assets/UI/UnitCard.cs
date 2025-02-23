using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCard : MonoBehaviour
{
    public TMP_Text unitName;
    public Image backGround;

    public TMP_Text goldAmount;
    public TMP_Text weaponAmount;
    public TMP_Text powderAmount;
    public TMP_Text horsesAmount;
    public TMP_Text woodAmount;

    public void ApplyData(UnitData unitData){
        unitName.text=unitData.elementName;
        backGround.sprite=unitData.image;
        goldAmount.text=""+unitData.cost.gold;
        weaponAmount.text=""+unitData.cost.meleeWeapons;
        powderAmount.text=""+unitData.cost.rangedWeapons;
        horsesAmount.text=""+unitData.cost.horses;
        woodAmount.text=""+unitData.cost.wood;
    }
}
