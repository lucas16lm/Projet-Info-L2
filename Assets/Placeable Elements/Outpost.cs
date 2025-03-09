using Unity.Cinemachine;
using UnityEngine;

public class Outpost : PlaceableObject, ICamera
{
    public int orderRange;
    public override void Initialize(PlaceableData placeableData, Tile position, Player player)
    {
        OutpostData data = placeableData as OutpostData;
        
        GameManager.instance.cameraManager.RegisterCamera(this);
        
        orderRange=data.orderRange;
        player.outposts.Add(this);
        healthPoints=data.baseHealthPoints;
        this.position=position;
        position.content=this;

        foreach(Renderer renderer in transform.GetChild(0).GetComponentsInChildren<Renderer>()) renderer.material=player.factionData.bannerMaterial;
    }

    public void SetPriority()
    {
        GameManager.instance.cameraManager.DesactivateRTS();
        GameManager.instance.cameraManager.GetPOVCameras().ForEach(cam=>cam.RemovePriority());
        GetComponentInChildren<CinemachineCamera>().Priority=1;
        Cursor.lockState=CursorLockMode.Locked;
    }

    public void RemovePriority()
    {
        GetComponentInChildren<CinemachineCamera>().Priority=0;
        Cursor.lockState=CursorLockMode.Confined;
    }
}
