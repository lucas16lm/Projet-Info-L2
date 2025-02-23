using System.Collections.Generic;
using PrimeTween;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI prefabs")]
    public GameObject unitCard;
    [Header("UI elements")]
    public GameObject endTurnButton;
    public GameObject ressourcePanel;
    public GameObject recruitmentPanel;
    public GameObject messagePanel;
    public GameObject povPanel;
    public GameObject crossHair;

    public void SetUpUI(GameState gameState){
        switch(gameState){
            case GameState.FirstPlayerGeneralPlacement or GameState.SecondPlayerGeneralPlacement:
                GetBattleUIElements().ForEach(e => e.SetActive(false));
                DesactivateCrossHair();
                break;
            
            case GameState.FirstPlayerDeployment:
                GetBattleUIElements().ForEach(e => e.SetActive(true));
                UpdateRessourcePanel(GameManager.instance.factionManager.firstFaction);
                UpdateRecruitmentPanel(GameManager.instance.factionManager.firstFaction);
                DesactivateCrossHair();
                break;

            case GameState.FirstPlayerTurn:
                GetBattleUIElements().ForEach(e => e.SetActive(false));
                UpdateRessourcePanel(GameManager.instance.factionManager.secondFaction);
                UpdateRecruitmentPanel(GameManager.instance.factionManager.secondFaction);
                ActivateCrossHair();
                break;

            case GameState.SecondPlayerDeployment:
                GetBattleUIElements().ForEach(e => e.SetActive(true));
                UpdateRessourcePanel(GameManager.instance.factionManager.secondFaction);
                UpdateRecruitmentPanel(GameManager.instance.factionManager.secondFaction);
                DesactivateCrossHair();
                break;

            case GameState.SecondPlayerTurn:
                GetBattleUIElements().ForEach(e => e.SetActive(false));
                UpdateRessourcePanel(GameManager.instance.factionManager.secondFaction);
                UpdateRecruitmentPanel(GameManager.instance.factionManager.secondFaction);
                ActivateCrossHair();
                break;

            
        }
    }

    private List<GameObject> GetBattleUIElements(){
        return new List<GameObject>(){endTurnButton, ressourcePanel, recruitmentPanel};
    }

    public void UpdateRessourcePanel(Faction faction){
        ressourcePanel.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text=""+faction.ressourceBalance.gold;
        ressourcePanel.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text=""+faction.ressourceBalance.meleeWeapons;
        ressourcePanel.transform.GetChild(2).GetComponentInChildren<TMP_Text>().text=""+faction.ressourceBalance.rangedWeapons;
        ressourcePanel.transform.GetChild(3).GetComponentInChildren<TMP_Text>().text=""+faction.ressourceBalance.horses;
        ressourcePanel.transform.GetChild(4).GetComponentInChildren<TMP_Text>().text=""+faction.ressourceBalance.wood;
    }

    private void UpdateRecruitmentPanel(Faction faction){
        ClearRecruitmentPanel();
        for (int i = 0; i < faction.data.factionUnitsData.Count; i++)
        {
            recruitmentPanel.transform.GetChild(i).gameObject.SetActive(true);
            recruitmentPanel.transform.GetChild(i).GetComponent<UnitCard>().ApplyData(faction.data.factionUnitsData[i]);

            povPanel.transform.GetChild(i).gameObject.SetActive(true);
            povPanel.transform.GetChild(i).GetComponent<UnitCard>().ApplyData(faction.data.factionUnitsData[i]);
        }
    }

    private void ClearRecruitmentPanel(){
        for (int i = 0; i < recruitmentPanel.transform.childCount; i++)
        {
            recruitmentPanel.transform.GetChild(i).gameObject.SetActive(false);
            povPanel.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void InitializeRecruitmentPanel(){
        int maxUnitNumber=Mathf.Max(GameManager.instance.factionManager.firstFaction.data.factionUnitsData.Count, GameManager.instance.factionManager.secondFaction.data.factionUnitsData.Count);
        while (maxUnitNumber>0)
        {
            Instantiate(unitCard, recruitmentPanel.transform);
            Instantiate(unitCard, povPanel.transform);
            maxUnitNumber--;
        }
    }

    public void ActivateCrossHair(){
        crossHair.SetActive(true);
    }
    public void DesactivateCrossHair(){
        crossHair.SetActive(false);
    }

    public void PrintMessage(string message){
        bool isCrossHairActive=crossHair.activeSelf;
        crossHair.SetActive(false);
        messagePanel.SetActive(true);
        messagePanel.GetComponentInChildren<TMP_Text>().text=message;
        Tween.Delay(3, ()=>{messagePanel.SetActive(false);crossHair.SetActive(isCrossHairActive);});
    }

    public void OpenPovPanel(Faction player){
        povPanel.SetActive(true);
        UpdateRecruitmentPanel(player);
        Time.timeScale=0;
        Cursor.lockState=CursorLockMode.Confined;
    }
    
    public void ClosePovPanel(){
        povPanel.SetActive(false);
        Time.timeScale=1;
        Cursor.lockState=CursorLockMode.Locked;
    }

     
}
