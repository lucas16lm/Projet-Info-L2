using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{   
    [Header("Settings")]
    public int maxTurnNumber = 30;
    public GameState currentState;

    

    public void InitTurns()
    {
        currentState = GameState.FirstPlayerGeneralPlacement;
        StartCoroutine(RunGameLoop());
    }



    IEnumerator RunGameLoop()
    {
        while (currentState != GameState.GameOver)
        {
            GameManager.instance.uIManager.SetUpUI(currentState);
            switch (currentState)
            {
                case GameState.FirstPlayerGeneralPlacement:
                    GameManager.instance.cameraManager.SetCameraState(CameraState.FirstPlayerRTS);
                    yield return StartCoroutine(FirstPlayerGeneralPlacement());
                    currentState = GameState.FirstPlayerDeployment;
                    break;

                case GameState.FirstPlayerDeployment:
                    yield return StartCoroutine(FirstPlayerUnitPlacement());
                    currentState = GameState.SecondPlayerGeneralPlacement;
                    break;

                case GameState.SecondPlayerGeneralPlacement:
                    GameManager.instance.cameraManager.SetCameraState(CameraState.SecondPlayerRTS);
                    yield return StartCoroutine(SecondPlayerGeneralPlacement());
                    currentState = GameState.SecondPlayerDeployment;
                    break;

                case GameState.SecondPlayerDeployment:
                    yield return StartCoroutine(SecondPlayerUnitPlacement());
                    currentState = GameState.FirstPlayerTurn;
                    break;

                
                default:
                Debug.Log("TODO");
                break;
            }

            yield return new WaitForSeconds(1);
        }

        Debug.Log("Fin du jeu !");
    }

    IEnumerator PlaceGeneral(Faction player, List<Tile> possibleTiles){
        Debug.Log("Placement du général de "+player.data.name);
        possibleTiles.ForEach(tile => tile.GetComponent<OutlineManager>().Outline());
        bool generalPlaced = false;
        
        while (!generalPlaced)
        {
            StartCoroutine(player.SelectMatchingGameObject(
                go =>{
                Tile tile = go?.GetComponent<Tile>();
                if(tile == null) return false;
                return possibleTiles.Contains(tile) && tile.isFree && tile.biome!=Biome.mountain && tile.biome!=Biome.water;
                },
                go=>{
                    player.PlaceElement(player.data.generalData, go.GetComponent<Tile>());
                    player.general.GetComponent<OutlineManager>().Outline();
                    generalPlaced=true;
                }));
            
            yield return new WaitUntil(()=>generalPlaced);
        }
    }
    IEnumerator FirstPlayerGeneralPlacement()
    {
        Faction player = GameManager.instance.factionManager.firstFaction;
        List<Tile> possibleTiles = Tile.GetTilesBetween(0, 3);
        yield return PlaceGeneral(player, possibleTiles);
    }

    IEnumerator SecondPlayerGeneralPlacement()
    {
        Faction player = GameManager.instance.factionManager.secondFaction;
        int maxY = (int)(GameManager.instance.mapGenerator.width*GameManager.instance.mapGenerator.heightRatio);
        List<Tile> possibleTiles = Tile.GetTilesBetween(maxY-4, maxY);
        yield return PlaceGeneral(player, possibleTiles);
    }

    IEnumerator UnitDeployment(Faction player, List<Tile> possibleTiles){
        Debug.Log("Placement des troupes de "+player.data.name);
        
        List<OutlineManager> outlineManagers = new List<OutlineManager>();
        possibleTiles.ForEach(tile => outlineManagers.Add(tile.GetComponent<OutlineManager>()));
        outlineManagers.Add(player.general.GetComponent<OutlineManager>());

        bool turnEnded=false;
        GameManager.instance.uIManager.endTurnButton.GetComponent<Button>().onClick.AddListener(delegate{turnEnded=true; Debug.Log("turn ended");});

        UnitData unitToPlace = null;
        
        for (int i = 0; i < player.data.factionUnitsData.Count; i++)
        {
            UnitData unitData = player.data.factionUnitsData[i];
            GameManager.instance.uIManager.recruitmentPanel.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate{unitToPlace=unitData ; Debug.Log("selected : "+unitData.elementName);});
        }

        while(!turnEnded){
            StartCoroutine(player.SelectMatchingGameObject(
                go =>{
                Tile tile = go?.GetComponent<Tile>();
                if(tile == null) return false;
                return unitToPlace!=null && possibleTiles.Contains(tile) && tile.isFree && tile.biome!=Biome.mountain && tile.biome!=Biome.water;
                },
                go=>{
                    GameObject unitGO = player.PlaceElement(unitToPlace, go.GetComponent<Tile>());
                    if(unitGO==null)return;
                    outlineManagers.Add(unitGO.GetComponent<OutlineManager>());
                    unitGO.GetComponent<OutlineManager>().Outline();
                    unitToPlace=null;
                }));

            yield return null;
        }
        outlineManagers.ForEach(outline => outline.DisableOutline());
    }

    IEnumerator FirstPlayerUnitPlacement(){
        Faction player = GameManager.instance.factionManager.firstFaction;
        List<Tile> possibleTiles = Tile.GetTilesBetween(0, 3);
        yield return UnitDeployment(player, possibleTiles);
    }

    IEnumerator SecondPlayerUnitPlacement(){
        Faction player = GameManager.instance.factionManager.secondFaction;
        int maxY = (int)(GameManager.instance.mapGenerator.width*GameManager.instance.mapGenerator.heightRatio);
        List<Tile> possibleTiles = Tile.GetTilesBetween(maxY-4, maxY);
        yield return UnitDeployment(player, possibleTiles);
    }
}

public enum GameState{
    FirstPlayerGeneralPlacement,
    FirstPlayerDeployment,
    SecondPlayerGeneralPlacement,
    SecondPlayerDeployment,
    FirstPlayerTurn,
    SecondPlayerTurn,
    GameOver
}
