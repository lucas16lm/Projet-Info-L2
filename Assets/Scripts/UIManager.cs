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
    public DeploymentPanel deploymentPanel;
    public GameObject messagePanel;
    public GameObject crossHair;
    public ReinforcementPanel reinforcementPanel;
    public CardTooltip cardTooltip;


    public void UpdateUI(GameState gameState){
        switch(gameState){
            case GameState.Initialization:
                
                ressourcePanel.SetActive(false);
                crossHair.SetActive(false);
                
                break;

            case GameState.FirstPlayerDeployment:

                ressourcePanel.SetActive(true);
                crossHair.SetActive(false);

                UpdateRessourcePanel(GameManager.instance.playerManager.firstPlayer);
                
                break;

            case GameState.SecondPlayerDeployment:
                
                ressourcePanel.SetActive(true);
                crossHair.SetActive(false);

                UpdateRessourcePanel(GameManager.instance.playerManager.secondPlayer);
                
                break;

            case GameState.FirstPlayerTurn:
                
                ressourcePanel.SetActive(true);
                crossHair.SetActive(true);
            
                UpdateRessourcePanel(GameManager.instance.playerManager.firstPlayer);
                
                break;

            case GameState.SecondPlayerTurn:
                
                ressourcePanel.SetActive(true);
                crossHair.SetActive(true);

                UpdateRessourcePanel(GameManager.instance.playerManager.secondPlayer);
                
                break;

            case GameState.GameOver:
                
                ressourcePanel.SetActive(false);
                crossHair.SetActive(false);
                
                break;
            
            default:
                break;
        }
    }

    public void UpdateRessourcePanel(Player player){
        ressourcePanel.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text=""+player.ressourceBalance.gold;
    }

    public void PrintMessage(string message){
        messagePanel.SetActive(true);
        messagePanel.GetComponentInChildren<TMP_Text>().text=message;
        Tween.Delay(3, ()=>{messagePanel.SetActive(false);});
    }     
}
