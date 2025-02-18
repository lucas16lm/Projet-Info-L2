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
        possibleTiles.ForEach(tile => tile.GetComponent<OutlineManager>().enabled = true);
        bool generalPlaced = false;
        InputAction inputAction = InputSystem.actions.FindAction("Select");
        
        while (!generalPlaced)
        {
            if (inputAction.IsPressed())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit)){
                    Tile tile = hit.collider.gameObject?.GetComponent<Tile>();
                    if(tile.biome != Biome.mountain && tile.biome != Biome.water && possibleTiles.Contains(tile)){
                        player.PlaceGeneral(tile);
                        generalPlaced = true;
                        possibleTiles.ForEach(tile => tile.GetComponent<OutlineManager>().enabled = false);
                        Debug.Log(player.data.name+" a placé son général");
                    }
                    else{
                        Debug.Log("Position invalide");
                    }
                    
                }
            }
            yield return null;
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

    IEnumerator UnitPlacement(Faction player, List<Tile> possibleTiles){
        Debug.Log("Placement des troupes de "+player.data.name);
        
        bool turnEnded=false;
        GameManager.instance.uIManager.endTurnButton.GetComponent<Button>().onClick.AddListener(delegate{turnEnded=true ; Debug.Log("turn ended");});

        while(!turnEnded){
            yield return null;
        }
        
    }

    IEnumerator FirstPlayerUnitPlacement(){
        Faction player = GameManager.instance.factionManager.firstFaction;
        List<Tile> possibleTiles = Tile.GetTilesBetween(0, 3);
        yield return UnitPlacement(player, possibleTiles);
    }

    IEnumerator SecondPlayerUnitPlacement(){
        Faction player = GameManager.instance.factionManager.secondFaction;
        int maxY = (int)(GameManager.instance.mapGenerator.width*GameManager.instance.mapGenerator.heightRatio);
        List<Tile> possibleTiles = Tile.GetTilesBetween(maxY-4, maxY);
        yield return UnitPlacement(player, possibleTiles);
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
