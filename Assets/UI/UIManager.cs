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
    public GameObject ressourcePanel;
    public GameObject deploymentPanel;
    public GameObject messagePanel;
    public GameObject reinforcementPanel;
    public GameObject crossHair;


    public void SetUpUI(UiState uiState, Player player){
        switch(uiState){
            case UiState.Nothing:
                ressourcePanel.SetActive(false);
                deploymentPanel.SetActive(false);
                reinforcementPanel.SetActive(false);
                crossHair.SetActive(false);
                break;
            case UiState.Deployment:
                UpdateRessourcePanel(player);
                deploymentPanel.SetActive(true);
                reinforcementPanel.SetActive(false);
                crossHair.SetActive(false);
                break;
            case UiState.POV:
                UpdateRessourcePanel(player);
                deploymentPanel.SetActive(false);
                reinforcementPanel.SetActive(false);
                crossHair.SetActive(true);
                Time.timeScale=1;
                break;
            default:
                break;
        }
    }

    private void UpdateRessourcePanel(Player player){
        ressourcePanel.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text=""+player.ressourceBalance.gold;
    }

    public void UpdateDeploymentPanel(Player player)
    {
        ClearDeploymentPanel();
        for (int i = 0; i < player.factionData.factionUnitsData.Count; i++)
        {
            deploymentPanel.transform.GetChild(i).gameObject.SetActive(true);
            deploymentPanel.transform.GetChild(i).GetComponent<UnitCard>().ApplyData(player.factionData.factionUnitsData[i]);

            reinforcementPanel.transform.GetChild(i).gameObject.SetActive(true);
            reinforcementPanel.transform.GetChild(i).GetComponent<UnitCard>().ApplyData(player.factionData.factionUnitsData[i]);
        }
    }

    private void ClearDeploymentPanel(){
        for (int i = 0; i < deploymentPanel.transform.childCount; i++)
        {
            deploymentPanel.transform.GetChild(i).gameObject.SetActive(false);
            reinforcementPanel.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void InitializeDeploymentPanel(){
        int maxUnitNumber=Mathf.Max(GameManager.instance.playerManager.firstPlayer.factionData.factionUnitsData.Count, GameManager.instance.playerManager.secondPlayer.factionData.factionUnitsData.Count);
        while (maxUnitNumber>0)
        {
            Instantiate(unitCard, deploymentPanel.transform);
            Instantiate(unitCard, reinforcementPanel.transform);
            maxUnitNumber--;
        }
    }


    public void PrintMessage(string message){
        bool isCrossHairActive=crossHair.activeSelf;
        crossHair.SetActive(false);
        messagePanel.SetActive(true);
        messagePanel.GetComponentInChildren<TMP_Text>().text=message;
        Tween.Delay(3, ()=>{messagePanel.SetActive(false);crossHair.SetActive(isCrossHairActive);});
    }

    public void OpenReinforcementPanel(Player player){
        reinforcementPanel.SetActive(true);
        UpdateDeploymentPanel(player);
        Time.timeScale=0;
        Cursor.lockState=CursorLockMode.Confined;
    }
    
    public void CloseReinforcementPanel(){
        reinforcementPanel.SetActive(false);
        Time.timeScale=1;
        Cursor.lockState=CursorLockMode.Locked;
    }

     
}

public enum UiState{
    Nothing,
    Deployment,
    POV,
}
