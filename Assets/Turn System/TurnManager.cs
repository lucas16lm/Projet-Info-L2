using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{   
    [Header("Settings")]
    public int nbTurn = 1;
    public GameState currentState;
    private List<ITurnObserver> observers;
    public Player firstPlayer;
    public Player secondPlayer;
    public bool gameEnded = false;

    public void AddObserver(ITurnObserver observer){
        observers.Add(observer);
    }

    public void RemoveObserver(ITurnObserver observer){
        observers.Remove(observer);
    }

    public void InitTurns()
    {
        observers = new List<ITurnObserver>();
        gameEnded=false;
        currentState = GameState.Initialization;
        StartCoroutine(RunGameLoop());
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
                    
                    yield return new WaitUntil(()=>firstPlayerPlayed && secondPlayerPlayed);
                    
                    GameManager.instance.playerManager.firstPlayer.GetPlaceableObjects().ForEach(element=>element.DisableOutlines());
                    StopCoroutine(firstCoroutine);
                    StopCoroutine(secondCoroutine);
                    
                    currentState = GameState.SecondPlayerDeployment;
 
                    break;

                case GameState.SecondPlayerDeployment:
                    GameManager.instance.cameraManager.ActivateSecondRTS();
                    
                    firstCoroutine = StartCoroutine(GameManager.instance.playerManager.firstPlayer.Wait(()=>firstPlayerPlayed=true));
                    secondCoroutine = StartCoroutine(GameManager.instance.playerManager.secondPlayer.Deployment(()=>secondPlayerPlayed=true));
                    
                    yield return new WaitUntil(()=>firstPlayerPlayed && secondPlayerPlayed);
                    
                    GameManager.instance.playerManager.secondPlayer.GetPlaceableObjects().ForEach(element=>element.DisableOutlines());
                    StopCoroutine(firstCoroutine);
                    StopCoroutine(secondCoroutine);
                    
                    currentState = GameState.FirstPlayerTurn;
                    break;
                    
                case GameState.FirstPlayerTurn:
                    GameManager.instance.playerManager.firstPlayer.general.SetPriority();

                    firstCoroutine = StartCoroutine(GameManager.instance.playerManager.firstPlayer.PlayTurn(()=>firstPlayerPlayed=true));
                    secondCoroutine = StartCoroutine(GameManager.instance.playerManager.secondPlayer.Wait(()=>secondPlayerPlayed=true));
                    yield return new WaitUntil(()=>firstPlayerPlayed && secondPlayerPlayed);
                    StopCoroutine(firstCoroutine);
                    StopCoroutine(secondCoroutine);
                    
                    currentState=GameState.SecondPlayerTurn;
                    break;
                
                case GameState.SecondPlayerTurn:
                    GameManager.instance.playerManager.secondPlayer.general.SetPriority();

                    firstCoroutine = StartCoroutine(GameManager.instance.playerManager.firstPlayer.Wait(()=>firstPlayerPlayed=true));
                    secondCoroutine = StartCoroutine(GameManager.instance.playerManager.secondPlayer.PlayTurn(()=>secondPlayerPlayed=true));
                    yield return new WaitUntil(()=>firstPlayerPlayed && secondPlayerPlayed);
                    StopCoroutine(firstCoroutine);
                    StopCoroutine(secondCoroutine);
                    
                    currentState=GameState.FirstPlayerTurn;
                    nbTurn++;
                    observers?.ForEach(observer=>observer.OnTurnEnded());
                    break;

                case GameState.GameOver:
                    break;
                
                default:
                    Debug.Log("TODO");
                    break;
            }
            

            yield return new WaitForSeconds(1);
        }

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
