using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI prefabs")]
    public GameObject unitCard;
    [Header("UI elements")]
    public GameObject endTurnButton;
    public GameObject ressourcePanel;
    public GameObject recruitmentPanel;
    public GameObject messagePanel;

    public void SetUpUI(GameState gameState){
        switch(gameState){
            case GameState.FirstPlayerGeneralPlacement or GameState.SecondPlayerGeneralPlacement:
                GetBattleUIElements().ForEach(e => e.SetActive(false));
                messagePanel.SetActive(true);
                messagePanel.GetComponentInChildren<TMP_Text>().text="Place your general";
                break;
            
            case GameState.FirstPlayerDeployment or GameState.FirstPlayerTurn:
                GetBattleUIElements().ForEach(e => e.SetActive(true));
                messagePanel.SetActive(false);
                UpdateRessourcePanel(GameManager.instance.factionManager.firstFaction);
                UpdateRecruitmentPanel(GameManager.instance.factionManager.firstFaction);
                break;
            
            case GameState.SecondPlayerDeployment or GameState.SecondPlayerTurn:
                GetBattleUIElements().ForEach(e => e.SetActive(true));
                messagePanel.SetActive(false);
                UpdateRessourcePanel(GameManager.instance.factionManager.secondFaction);
                UpdateRecruitmentPanel(GameManager.instance.factionManager.secondFaction);
                break;
        }
    }

    private List<GameObject> GetBattleUIElements(){
        return new List<GameObject>(){endTurnButton, ressourcePanel, recruitmentPanel};
    }

    public void UpdateRessourcePanel(Faction faction){
        ressourcePanel.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text=""+faction.ressourceBalance.gold;
        ressourcePanel.transform.GetChild(1).GetComponentInChildren<TMP_Text>().text=""+faction.ressourceBalance.weapons;
        ressourcePanel.transform.GetChild(2).GetComponentInChildren<TMP_Text>().text=""+faction.ressourceBalance.powder;
        ressourcePanel.transform.GetChild(3).GetComponentInChildren<TMP_Text>().text=""+faction.ressourceBalance.horses;
        ressourcePanel.transform.GetChild(4).GetComponentInChildren<TMP_Text>().text=""+faction.ressourceBalance.wood;
    }

    private void UpdateRecruitmentPanel(Faction faction){
        ClearRecruitmentPanel();
        for (int i = 0; i < faction.data.factionUnitsData.Count; i++)
        {
            recruitmentPanel.transform.GetChild(i).gameObject.SetActive(true);
            recruitmentPanel.transform.GetChild(i).GetComponent<UnitCard>().ApplyData(faction.data.factionUnitsData[i]);
        }
    }

    private void ClearRecruitmentPanel(){
        for (int i = 0; i < recruitmentPanel.transform.childCount; i++)
        {
            recruitmentPanel.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void InitializeRecruitmentPanel(){
        int maxUnitNumber=Mathf.Max(GameManager.instance.factionManager.firstFaction.data.factionUnitsData.Count, GameManager.instance.factionManager.secondFaction.data.factionUnitsData.Count);
        while (maxUnitNumber>0)
        {
            Instantiate(unitCard, recruitmentPanel.transform);
            maxUnitNumber--;
        }
    }

     
}
