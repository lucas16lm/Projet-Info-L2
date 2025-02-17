using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public TurnManager turnManager;
    public ArmyManager armyManager;

    void Start()
    {
        StartGame();
    }

    public void StartGame(){
        mapGenerator.seed=Random.Range(-100000,100000);
        mapGenerator.CreateMap();

        turnManager.Initialize();
    }

    public void EndGame(){
        mapGenerator.ClearTiles();
    }

    
}
