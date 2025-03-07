using System;
using System.Collections;
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
    public GameObject crossHair;
    public GameObject reinforcementPanel;


    public void UpdateUI(GameState gameState){
        switch(gameState){
            case GameState.Initialization:
                
                ressourcePanel.SetActive(false);
                deploymentPanel.SetActive(false);
                crossHair.SetActive(false);
                
                break;

            case GameState.FirstPlayerDeployment:

                ressourcePanel.SetActive(true);
                deploymentPanel.SetActive(false);
                crossHair.SetActive(false);

                UpdateDeploymentPanel(GameManager.instance.playerManager.firstPlayer);
                UpdateRessourcePanel(GameManager.instance.playerManager.firstPlayer);
                
                break;

            case GameState.SecondPlayerDeployment:
                
                ressourcePanel.SetActive(true);
                deploymentPanel.SetActive(false);
                crossHair.SetActive(false);

                UpdateDeploymentPanel(GameManager.instance.playerManager.secondPlayer);
                UpdateRessourcePanel(GameManager.instance.playerManager.secondPlayer);
                
                break;

            case GameState.FirstPlayerTurn:
                
                ressourcePanel.SetActive(true);
                deploymentPanel.SetActive(false);
                crossHair.SetActive(true);
            
                UpdateRessourcePanel(GameManager.instance.playerManager.firstPlayer);
                
                break;

            case GameState.SecondPlayerTurn:
                
                ressourcePanel.SetActive(true);
                deploymentPanel.SetActive(false);
                crossHair.SetActive(true);

                UpdateRessourcePanel(GameManager.instance.playerManager.secondPlayer);
                
                break;

            case GameState.GameOver:
                
                ressourcePanel.SetActive(false);
                deploymentPanel.SetActive(false);
                crossHair.SetActive(false);
                
                break;
            
            default:
                break;
        }
    }

    public void UpdateRessourcePanel(Player player){
        ressourcePanel.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text=""+player.ressourceBalance.gold;
    }

    public void UpdateDeploymentPanel(Player player)
    {
        ClearDeploymentPanel();
        for (int i = 0; i < player.factionData.factionUnitsData.Count; i++)
        {
            UnitCard.Instantiate(player.factionData.factionUnitsData[i], deploymentPanel.transform);
        }
    }

    private void ClearDeploymentPanel(){
        for (int i = 0; i < deploymentPanel.transform.childCount; i++)
        {
            Destroy(deploymentPanel.transform.GetChild(i).gameObject);
        }
    }

    public void PrintMessage(string message){
        messagePanel.SetActive(true);
        messagePanel.GetComponentInChildren<TMP_Text>().text=message;
        Tween.Delay(3, ()=>{messagePanel.SetActive(false);});
    }

    public void OpenReinforcementPanel(Player player)
    {
        reinforcementPanel.SetActive(true);
        foreach(UnitData unitData in player.factionData.factionUnitsData)
        {
            UnitCard.Instantiate(unitData, reinforcementPanel.transform.GetChild(0).transform);
        }
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
    }
     
}
