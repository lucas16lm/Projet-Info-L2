using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static FactionData firstPlayerFaction;
    public static FactionData secondPlayerFaction;

    public MapGenerator mapGenerator;
    public List<FactionData> gameFactions;
    public GameObject mainPanel;
    public GameObject selectionPanel;

    public TMP_Dropdown firstFactionDropDown;
    public TMP_Dropdown secondFactionDropDown;
    
    void Start()
    {
        mapGenerator.seed=Random.Range(-100000,100000);
        mapGenerator.CreateMap();

        foreach(FactionData factionData in gameFactions){
            firstFactionDropDown.options.Add(new TMP_Dropdown.OptionData(factionData.factionName));
            secondFactionDropDown.options.Add(new TMP_Dropdown.OptionData(factionData.factionName));
        }
    }

    public void OpenSelectionPanel(){
        mainPanel.SetActive(false);
        selectionPanel.SetActive(true);
        firstPlayerFaction = null;
        secondPlayerFaction = null;
    }

    public void LaunchGame(){
        if(firstPlayerFaction != null && secondPlayerFaction != null && firstPlayerFaction != secondPlayerFaction){
            SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
        }
    }

    public void ExitGame(){
        Application.Quit();
    }

    public void BackButton(){
        mainPanel.SetActive(true);
        selectionPanel.SetActive(false);
        firstPlayerFaction = null;
        secondPlayerFaction = null;
    }

    public void OnFirstFactionChanged(){
        if(firstFactionDropDown.value!=0){
            firstPlayerFaction = gameFactions[firstFactionDropDown.value-1];
        }
        else{
            firstPlayerFaction = null;
        }
    }

    public void OnSecondFactionChanged(){
        if(secondFactionDropDown.value!=0){
            secondPlayerFaction = gameFactions[secondFactionDropDown.value-1];
        }
        else{
            secondPlayerFaction = null;
        }
    }
}
