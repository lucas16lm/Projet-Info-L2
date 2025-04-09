using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){pauseManager();};
    }

    void pauseManager()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BattleScene")){
            if(gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void Resume()
    {
        if (GameManager.instance.turnManager.currentState == GameState.FirstPlayerTurn || GameManager.instance.turnManager.currentState == GameState.SecondPlayerTurn){
            Cursor.lockState = CursorLockMode.Locked;
        }
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    public void loadMainMenu()
    {
        Resume();
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);
    }

    public void RestartGame()
    {
        Resume();
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("BattleScene",LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }


}
