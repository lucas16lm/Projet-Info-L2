using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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
            
            case GameState.FirstPlayerDeployment:
                GetBattleUIElements().ForEach(e => e.SetActive(true));
                messagePanel.SetActive(false);
                break;
            
            case GameState.SecondPlayerDeployment:
                GetBattleUIElements().ForEach(e => e.SetActive(true));
                messagePanel.SetActive(false);
                break;
            
            case GameState.FirstPlayerTurn:
                GetBattleUIElements().ForEach(e => e.SetActive(true));
                messagePanel.SetActive(false);
                break;
        }
    }

    private List<GameObject> GetBattleUIElements(){
        return new List<GameObject>(){endTurnButton, ressourcePanel, recruitmentPanel};
    }

     
}
