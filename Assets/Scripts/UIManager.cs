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
                
                crossHair.SetActive(false);
                
                break;

            case GameState.FirstPlayerDeployment:

                crossHair.SetActive(false);

                UpdateRessourcePanel(GameManager.instance.playerManager.firstPlayer);
                
                break;

            case GameState.SecondPlayerDeployment:
                
                crossHair.SetActive(false);

                UpdateRessourcePanel(GameManager.instance.playerManager.secondPlayer);
                
                break;

            case GameState.FirstPlayerTurn:
                
                crossHair.SetActive(true);
            
                UpdateRessourcePanel(GameManager.instance.playerManager.firstPlayer);
                
                break;

            case GameState.SecondPlayerTurn:
                
                crossHair.SetActive(true);

                UpdateRessourcePanel(GameManager.instance.playerManager.secondPlayer);
                
                break;

            case GameState.GameOver:
                
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
        Tween.Delay(1.5f, ()=>{messagePanel.SetActive(false);});
    }     
}
