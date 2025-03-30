using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static FactionData firstPlayerFaction;
    public int firstPlayerFactionIndex;
    public static FactionData secondPlayerFaction;
    public int secondPlayerFactionIndex;

    public List<FactionData> gameFactions;
    public GameObject mainPanel;
    public GameObject selectionPanel;

    public Transform firstFactionUnitParent;
    public Transform secondFactionUnitParent;
    public TMP_Text firstFactionText;
    public TMP_Text secondFactionText;
    
    void Start()
    {
        firstPlayerFaction = gameFactions[0];
        firstPlayerFactionIndex = 0;
        secondPlayerFaction = gameFactions[1];
        secondPlayerFactionIndex = 1;
        OnFFChanged();
        OnSFChanged();
    }

    public void OpenSelectionPanel(){
        mainPanel.SetActive(false);
        selectionPanel.SetActive(true);
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
    }

    public void FF_Previous(){
        firstPlayerFactionIndex=(firstPlayerFactionIndex-1)%gameFactions.Count;
        if(firstPlayerFactionIndex<0) firstPlayerFactionIndex = gameFactions.Count-1;
        if(firstPlayerFactionIndex==secondPlayerFactionIndex) FF_Previous();
        OnFFChanged();
    }

    public void FF_Next(){
        firstPlayerFactionIndex=(firstPlayerFactionIndex+1)%gameFactions.Count;
        if(firstPlayerFactionIndex==secondPlayerFactionIndex) FF_Next();
        OnFFChanged();
    }

    public void OnFFChanged()
    {
        firstPlayerFaction = gameFactions[firstPlayerFactionIndex];
        foreach(Transform child in firstFactionUnitParent.transform)
        {
            Destroy(child.gameObject);
        }
        GameObject go = Instantiate(firstPlayerFaction.factionUnitsData[0].gameObjectPrefab, firstFactionUnitParent.position, Quaternion.Euler(0,-20,0), firstFactionUnitParent);
        foreach(Renderer renderer in go.GetComponentsInChildren<Renderer>()) renderer.material = firstPlayerFaction.unitsMaterial;
        foreach(Renderer renderer in go.transform.GetChild(0).GetComponentsInChildren<Renderer>()) renderer.material = firstPlayerFaction.bannerMaterial;
        firstFactionText.text=firstPlayerFaction.factionName;
    }

    public void SF_Previous(){
        secondPlayerFactionIndex=(secondPlayerFactionIndex-1)%gameFactions.Count;
        if(secondPlayerFactionIndex<0) secondPlayerFactionIndex = gameFactions.Count-1;
        if(firstPlayerFactionIndex==secondPlayerFactionIndex) SF_Previous();
        OnSFChanged();
    }

    public void SF_Next(){
        secondPlayerFactionIndex=(secondPlayerFactionIndex+1)%gameFactions.Count;
        if(firstPlayerFactionIndex==secondPlayerFactionIndex) SF_Next();
        OnSFChanged();
    }

    public void OnSFChanged()
    {
        secondPlayerFaction = gameFactions[secondPlayerFactionIndex];
        foreach(Transform child in secondFactionUnitParent.transform)
        {
            Destroy(child.gameObject);
        }
        GameObject go = Instantiate(secondPlayerFaction.factionUnitsData[0].gameObjectPrefab, secondFactionUnitParent.position, Quaternion.Euler(0,20,0), secondFactionUnitParent);
        foreach(Renderer renderer in go.GetComponentsInChildren<Renderer>()) renderer.material = secondPlayerFaction.unitsMaterial;
        foreach(Renderer renderer in go.transform.GetChild(0).GetComponentsInChildren<Renderer>()) renderer.material = secondPlayerFaction.bannerMaterial;
        secondFactionText.text=secondPlayerFaction.factionName;
    }


}
