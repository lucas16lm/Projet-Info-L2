using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public MapGenerator mapGenerator;
    
    void Start()
    {
        mapGenerator.seed=Random.Range(-100000,100000);
        mapGenerator.CreateMap();
    }

    public void LaunchGame(){
        SceneManager.LoadScene("BattleScene");
    }


}
