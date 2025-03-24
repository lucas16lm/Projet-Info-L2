using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{   
    [Header("Settings")]
    public int nbTurn = 1;
    public GameState currentState;
    private List<ITurnObserver> observers;
    public bool gameEnded = false;

    public void AddObserver(ITurnObserver observer){
        observers.Add(observer);
    }

    public IEnumerator RemoveObserver(ITurnObserver observer){
        yield return null;
        if(observers.Contains(observer)) observers.Remove(observer);
    }

    public void InitTurns()
    {
        observers = new List<ITurnObserver>();
        gameEnded=false;
        currentState = GameState.Initialization;
        StartCoroutine(RunGameLoop());
    }

    public void PlayerWon(Player player){
        gameEnded=true;
        currentState=GameState.GameOver;
        GameManager.instance.uIManager.PrintMessage(player.factionData.factionName+" has won !");
    }

    IEnumerator RunGameLoop()
    {
        while (currentState != GameState.GameOver)
        {
            Coroutine firstCoroutine;
            Coroutine secondCoroutine;
            bool firstPlayerPlayed = false;
            bool secondPlayerPlayed = false;

            GameManager.instance.uIManager.UpdateUI(currentState);

            switch (currentState)
            {
                case GameState.Initialization:
                    currentState=GameState.FirstPlayerDeployment;
                    nbTurn=1;
                    break;

                case GameState.FirstPlayerDeployment:
                    GameManager.instance.cameraManager.ActivateFirstRTS();
                    
                    firstCoroutine = StartCoroutine(GameManager.instance.playerManager.firstPlayer.Deployment(()=>firstPlayerPlayed=true));
                    secondCoroutine = StartCoroutine(GameManager.instance.playerManager.secondPlayer.Wait(()=>secondPlayerPlayed=true));
                    
                    yield return new WaitUntil(()=>(firstPlayerPlayed && secondPlayerPlayed) || gameEnded);
                    
                    GameManager.instance.playerManager.firstPlayer.GetPlaceableObjects().ForEach(element=>element.DisableOutlines());
                    StopCoroutine(firstCoroutine);
                    StopCoroutine(secondCoroutine);
                    
                    if(currentState!=GameState.GameOver)currentState = GameState.SecondPlayerDeployment;
 
                    break;

                case GameState.SecondPlayerDeployment:
                    GameManager.instance.cameraManager.ActivateSecondRTS();
                    
                    firstCoroutine = StartCoroutine(GameManager.instance.playerManager.firstPlayer.Wait(()=>firstPlayerPlayed=true));
                    secondCoroutine = StartCoroutine(GameManager.instance.playerManager.secondPlayer.Deployment(()=>secondPlayerPlayed=true));
                    
                    yield return new WaitUntil(()=>(firstPlayerPlayed && secondPlayerPlayed) || gameEnded);
                    
                    GameManager.instance.playerManager.secondPlayer.GetPlaceableObjects().ForEach(element=>element.DisableOutlines());
                    StopCoroutine(firstCoroutine);
                    StopCoroutine(secondCoroutine);
                    
                    if(currentState!=GameState.GameOver)currentState = GameState.FirstPlayerTurn;
                    break;
                    
                case GameState.FirstPlayerTurn:
                    GameManager.instance.playerManager.firstPlayer.general.SetPriority();


                    firstCoroutine = StartCoroutine(GameManager.instance.playerManager.firstPlayer.PlayTurn(()=>firstPlayerPlayed=true));
                    secondCoroutine = StartCoroutine(GameManager.instance.playerManager.secondPlayer.Wait(()=>secondPlayerPlayed=true));
                    
                    yield return new WaitUntil(()=>(firstPlayerPlayed && secondPlayerPlayed) || gameEnded);
                    
                    StopCoroutine(firstCoroutine);
                    StopCoroutine(secondCoroutine);
                    
                    if(currentState!=GameState.GameOver)currentState = GameState.SecondPlayerTurn;
                    break;
                
                case GameState.SecondPlayerTurn:
                    GameManager.instance.playerManager.secondPlayer.general.SetPriority();

                    firstCoroutine = StartCoroutine(GameManager.instance.playerManager.firstPlayer.Wait(()=>firstPlayerPlayed=true));
                    secondCoroutine = StartCoroutine(GameManager.instance.playerManager.secondPlayer.PlayTurn(()=>secondPlayerPlayed=true));
                    
                    yield return new WaitUntil(()=>(firstPlayerPlayed && secondPlayerPlayed) || gameEnded);
                    
                    StopCoroutine(firstCoroutine);
                    StopCoroutine(secondCoroutine);
                    
                    if(currentState!=GameState.GameOver)currentState = GameState.FirstPlayerTurn;
                    
                    nbTurn++;
                    observers?.ForEach(observer=>observer.OnTurnEnded());
                    break;

                case GameState.GameOver:
                    break;
                
                default:
                    Debug.Log("Not supposed to be here");
                    break;
            }
            

            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(3);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        Debug.Log("Fin du jeu !");
    }

}



public enum GameState{
    Initialization,
    FirstPlayerDeployment,
    SecondPlayerDeployment,
    FirstPlayerTurn,
    SecondPlayerTurn,
    GameOver
}

public interface ITurnObserver{
    void OnTurnEnded();
}
