using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameState currentState;
    public int maxTurnNumber = 30;

    public void Initialize()
    {
        currentState = GameState.FirstPlayerDeployment;
        
    }

    IEnumerator RunGameLoop()
    {
        while (currentState != GameState.GameOver)
        {
            switch (currentState)
            {
                case GameState.FirstPlayerDeployment:
                    Debug.Log("Tour du Joueur 1");
                    yield return StartCoroutine(DeploymentTurn());
                    currentState = GameState.SecondPlayerDeployment;
                    break;

                case GameState.SecondPlayerDeployment:
                    Debug.Log("Tour du Joueur 2");
                    yield return StartCoroutine(DeploymentTurn());
                    currentState = GameState.FirstPlayerTurn;
                    break;
            }

            yield return null;
        }

        Debug.Log("Fin du jeu !");
    }

    IEnumerator DeploymentTurn()
    {
        bool actionDone = false;

        while (!actionDone)
        {
            if (Input.GetKeyDown(KeyCode.Space))  // Simulation d'un mouvement du joueur
            {
                Debug.Log("Action du joueur validée !");
                actionDone = true;
            }

            yield return null;  // Évite la boucle infinie
        }
    }
}

public enum GameState{
    FirstPlayerDeployment,
    SecondPlayerDeployment,
    FirstPlayerTurn,
    SecondPlayerTurn,
    GameOver
}
