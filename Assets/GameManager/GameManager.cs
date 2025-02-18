using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("System references")]
    public MapGenerator mapGenerator;
    public CameraManager cameraManager;
    public TurnManager turnManager;
    public FactionManager factionManager;
    public UIManager uIManager;

    [Header("Faction settings")]
    public FactionData firstFactionData;
    public FactionData secondFactionData;

    void Awake()
    {
        if(instance==null) instance=this;
        else Debug.LogError("Il y a déjà un GameManager !!!");
    }

    void Start()
    {
        mapGenerator.seed=Random.Range(-100000,100000);
        mapGenerator.CreateMap();
        factionManager.InitFactions(firstFactionData, secondFactionData);
        turnManager.InitTurns();
    }
}
