using Unity.Cinemachine;
using UnityEngine;

public class General : PlaceableObject
{
    public override void Initialize(PlaceableData placeableData, Tile position, Player player)
    {
        if(player.playerRole==PlayerRole.FirstPlayer){
            GameManager.instance.cameraManager.firstPlayerCameras.Add(GetComponentInChildren<CinemachineCamera>());
        }
        else{
            GameManager.instance.cameraManager.secondPlayerCameras.Add(GetComponentInChildren<CinemachineCamera>());
        }
        player.general=this;
        healthPoints=placeableData.baseHealthPoints;
        this.position=position;
        position.occupied=true;

        foreach(Renderer renderer in transform.GetChild(0).GetComponentsInChildren<Renderer>()) renderer.material=player.factionData.bannerMaterial;
    }
}
