using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    public int buildingConstructionLimit=1;
    [Header("Player factions")]
    public static GameManager instance;
    [Header("System references")]
    public MapGenerator mapGenerator;
    public PlayerManager playerManager;
    public CameraManager cameraManager;
    public TurnManager turnManager;
    public SoundManager soundManager;
    public UIManager uIManager;
    [Header("Rendering layer masks")]
    public int TileZoneLayerID;
    public int TileSelectLayerID;
    public int MoveRangeLayerID;
    public int AllyLayerId;
    public int EnnemyLayerId;
    public int RangedAttackLayerId;

    
    

    void Awake()
    {
        if(instance==null) instance=this;
        else Debug.LogError("Il y a déjà un GameManager !!!");
    }

    void Start()
    {
        mapGenerator.seed=Random.Range(-100000,100000);
        mapGenerator.CreateMap();
        playerManager.InitializePlayers(MainMenu.firstPlayerFaction, MainMenu.secondPlayerFaction);
        turnManager.InitTurns();
    }
}
