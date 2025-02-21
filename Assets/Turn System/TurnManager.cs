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
                    yield return FirstPlayerGeneralPlacement();
                    GameManager.instance.cameraManager.firstPlayerCameras.Add(GameManager.instance.factionManager.firstFaction.general.gameObject.GetComponentInChildren<CinemachineCamera>());
                    currentState = GameState.FirstPlayerDeployment;
                    break;

                case GameState.FirstPlayerDeployment:
                    yield return StartCoroutine(FirstPlayerUnitPlacement());
                    currentState = GameState.SecondPlayerGeneralPlacement;
                    break;

                case GameState.SecondPlayerGeneralPlacement:
                    GameManager.instance.cameraManager.SetCameraState(CameraState.SecondPlayerRTS);
                    yield return SecondPlayerGeneralPlacement();
                    GameManager.instance.cameraManager.secondPlayerCameras.Add(GameManager.instance.factionManager.secondFaction.general.gameObject.GetComponentInChildren<CinemachineCamera>());
                    currentState = GameState.SecondPlayerDeployment;
                    break;

                case GameState.SecondPlayerDeployment:
                    yield return StartCoroutine(SecondPlayerUnitPlacement());
                    currentState = GameState.FirstPlayerTurn;
                    break;

                case GameState.FirstPlayerTurn:
                    GameManager.instance.cameraManager.SetCameraState(CameraState.FirstPlayerPOV);
                    Debug.Log("Tour de "+GameManager.instance.factionManager.firstFaction.data.name);
                    yield return PlayerTurn(GameManager.instance.factionManager.firstFaction);
                    currentState = GameState.SecondPlayerTurn;
                    break;

                case GameState.SecondPlayerTurn:
                    GameManager.instance.cameraManager.SetCameraState(CameraState.SecondPlayerPOV);
                    Debug.Log("Tour de "+GameManager.instance.factionManager.secondFaction.data.name);
                    yield return PlayerTurn(GameManager.instance.factionManager.secondFaction);
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
        GameManager.instance.uIManager.PrintMessage(player.data.name+", place your general !");
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
        GameManager.instance.uIManager.PrintMessage(player.data.name+", compose your army !");
        
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
            
            yield return player.SelectMatchingGameObject(
                go =>{
                    if(go?.GetComponent<Tile>()==null && go?.GetComponent<Unit>()==null) return false;
                    Tile tile = go?.GetComponent<Tile>();
                    Unit unit = go?.GetComponent<Unit>();
                    if(tile!=null) return unitToPlace!=null && possibleTiles.Contains(tile) && tile.isFree && tile.biome!=Biome.mountain && tile.biome!=Biome.water;
                    else return player.units.Contains(unit);
                    },
                go=>{
                    Tile tile = go?.GetComponent<Tile>();
                    Unit unit = go?.GetComponent<Unit>();
                    if(tile != null){
                        GameObject unitGO = player.PlaceElement(unitToPlace, go.GetComponent<Tile>());
                        if(unitGO!=null){
                            outlineManagers.Add(unitGO.GetComponent<OutlineManager>());
                            unitGO.GetComponent<OutlineManager>().Outline();
                            unitToPlace=null;
                        }
                    }
                    else{
                        player.ressourceBalance.AddRessources(unit.data.cost);
                        GameManager.instance.uIManager.UpdateRessourcePanel(player);
                        unit.position.isFree=true;
                        player.units.Remove(unit);
                        outlineManagers.Remove(unit.GetComponent<OutlineManager>());
                        Destroy(go);
                    }
                });

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

    IEnumerator PlayerTurn(Faction player){
        
        GameManager.instance.uIManager.PrintMessage(player.data.name+", it's your turn !");
        
        bool turnEnded=false;
        
        InputAction endTurnAction = InputSystem.actions.FindAction("EndTurn");
        endTurnAction.performed+=ctx=>turnEnded=true;
        
        while(!turnEnded){
            Unit selectedUnit = null;
            yield return player.SelectMatchingGameObject(
                go=>go?.GetComponent<Unit>()!=null,
                go=>{selectedUnit=go.GetComponent<Unit>();}
            );
            yield return player.SelectMatchingGameObject(
                go=>go?.GetComponent<Tile>()!=null,
                go=>selectedUnit.Move(go.GetComponent<Tile>())
            );
        }
        
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
