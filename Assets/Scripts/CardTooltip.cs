using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardTooltip : MonoBehaviour
{
    [SerializeField]
    private TMP_Text unitNameText;
    [SerializeField]
    private TMP_Text unitClassText;
    [SerializeField]
    private TMP_Text unitHealthText;
    [SerializeField]
    private TMP_Text unitAttackText;
    [SerializeField]
    private TMP_Text unitMovementText;

    public void ShowCardTooltip(UnitData unitData){
        gameObject.SetActive(true);
        
        unitNameText.text=unitData.elementName;
        
        if(unitData.gameObjectPrefab?.GetComponent<ShieldInfantry>() != null){
            unitClassText.text="Shield infantry";
        }
        else if(unitData.gameObjectPrefab?.GetComponent<PikeInfantry>() != null){
            unitClassText.text="Pike infantry";
        }
        else if(unitData.gameObjectPrefab?.GetComponent<TwoHandedInfantry>() != null){
            unitClassText.text="Two handed infantry";
        }
        else if(unitData.gameObjectPrefab?.GetComponent<Archer>() != null){
            unitClassText.text="Archer";
        }
        else if(unitData.gameObjectPrefab?.GetComponent<Crossbow>() != null){
            unitClassText.text="Crossbow";
        }
        else{
            unitClassText.text="Melee cavalry";
        }

        unitHealthText.text=""+unitData.baseHealthPoints;
        unitAttackText.text=""+unitData.baseDamagePoints;
        unitMovementText.text=""+unitData.baseMovementPoints;

        GetComponent<RectTransform>().position = Mouse.current.position.ReadValue()+Vector2.up*5;
    }

    public void HideCardTooltip(){
        gameObject.SetActive(false);
    }
}
