using Unity.Cinemachine;
using UnityEngine;

public class General : PlaceableObject
{
    public int orderRange;
    public override void Initialize(PlaceableData placeableData, Tile position, Player player)
    {
        GeneralData data = placeableData as GeneralData;
        if(player.playerRole==PlayerRole.FirstPlayer){
            GameManager.instance.cameraManager.firstPlayerCameras.Add(GetComponentInChildren<CinemachineCamera>());
        }
        else{
            GameManager.instance.cameraManager.secondPlayerCameras.Add(GetComponentInChildren<CinemachineCamera>());
        }
        orderRange=data.orderRange;
        player.general=this;
        healthPoints=data.baseHealthPoints;
        this.position=position;
        position.occupied=true;

        foreach(Renderer renderer in transform.GetChild(0).GetComponentsInChildren<Renderer>()) renderer.material=player.factionData.bannerMaterial;
    }
}
