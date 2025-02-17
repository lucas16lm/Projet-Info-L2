using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MapGenerator mapGenerator;

    void Start()
    {
        InitGame();
    }

    public void InitGame(){
        mapGenerator.CreateMap();
    }

    
}
