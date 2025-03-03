using Unity.Cinemachine;
using UnityEngine;

public class Officer : PlaceableObject
{
    public override void Initialize(PlaceableData placeableData, Tile position, Player player)
    {
        if(player.playerRole==PlayerRole.FirstPlayer){
            GameManager.instance.cameraManager.firstPlayerCameras.Add(GetComponentInChildren<CinemachineCamera>());
        }
        else{
            GameManager.instance.cameraManager.secondPlayerCameras.Add(GetComponentInChildren<CinemachineCamera>());
        }
        player.officers.Add(this);
        healthPoints=placeableData.baseHealthPoints;
        this.position=position;
        position.occupied=true;
    }
}
