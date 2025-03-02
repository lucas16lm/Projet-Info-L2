using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{   
    [Header("Settings")]
    public int maxTurnNumber = 30;
    public GameState currentState;

    public Player firstPlayer;
    public Player secondPlayer;
    public bool gameEnded = false;


    public void InitTurns()
    {
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

            switch (currentState)
            {
                case GameState.Initialization:
                    currentState=GameState.FirstPlayerDeployment;
                    break;

                case GameState.FirstPlayerDeployment:
                    GameManager.instance.cameraManager.SetCameraState(CameraState.FirstPlayerRTS);
                    
                    firstCoroutine = StartCoroutine(GameManager.instance.playerManager.firstPlayer.Deployment(()=>firstPlayerPlayed=true));
                    secondCoroutine = StartCoroutine(GameManager.instance.playerManager.secondPlayer.Wait(()=>secondPlayerPlayed=true));
                    yield return new WaitUntil(()=>firstPlayerPlayed && secondPlayerPlayed);
                    StopCoroutine(firstCoroutine);
                    StopCoroutine(secondCoroutine);
                    
                    currentState = GameState.SecondPlayerDeployment;
                    break;

                case GameState.SecondPlayerDeployment:
                    GameManager.instance.cameraManager.SetCameraState(CameraState.SecondPlayerRTS);
                    
                    firstCoroutine = StartCoroutine(GameManager.instance.playerManager.firstPlayer.Wait(()=>firstPlayerPlayed=true));
                    secondCoroutine = StartCoroutine(GameManager.instance.playerManager.secondPlayer.Deployment(()=>secondPlayerPlayed=true));
                    yield return new WaitUntil(()=>firstPlayerPlayed && secondPlayerPlayed);
                    StopCoroutine(firstCoroutine);
                    StopCoroutine(secondCoroutine);
                    
                    currentState = GameState.SecondPlayerDeployment;
                    break;
                    
                case GameState.FirstPlayerTurn:
                    GameManager.instance.cameraManager.SetCameraState(CameraState.FirstPlayerPOV);

                    firstCoroutine = StartCoroutine(GameManager.instance.playerManager.firstPlayer.PlayTurn(()=>firstPlayerPlayed=true));
                    secondCoroutine = StartCoroutine(GameManager.instance.playerManager.secondPlayer.Wait(()=>secondPlayerPlayed=true));
                    yield return new WaitUntil(()=>firstPlayerPlayed && secondPlayerPlayed);
                    StopCoroutine(firstCoroutine);
                    StopCoroutine(secondCoroutine);
                    
                    currentState=GameState.SecondPlayerDeployment;
                    break;
                
                case GameState.SecondPlayerTurn:
                    GameManager.instance.cameraManager.SetCameraState(CameraState.SecondPlayerPOV);

                    firstCoroutine = StartCoroutine(GameManager.instance.playerManager.firstPlayer.Wait(()=>firstPlayerPlayed=true));
                    secondCoroutine = StartCoroutine(GameManager.instance.playerManager.secondPlayer.PlayTurn(()=>secondPlayerPlayed=true));
                    yield return new WaitUntil(()=>firstPlayerPlayed && secondPlayerPlayed);
                    StopCoroutine(firstCoroutine);
                    StopCoroutine(secondCoroutine);
                    
                    currentState=GameState.FirstPlayerTurn;
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
