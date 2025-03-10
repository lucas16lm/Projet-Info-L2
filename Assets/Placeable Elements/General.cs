using Unity.Cinemachine;
using UnityEngine;

public class General : PlaceableObject, ICamera
{
    public int orderRange;
    public override void Initialize(PlaceableData placeableData, Tile position, Player player)
    {
        GeneralData data = placeableData as GeneralData;
        
        GameManager.instance.cameraManager.RegisterCamera(this);

        orderRange=data.orderRange;
        player.general=this;
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

    public Tile GetPosition()
    {
        return position;
    }

    public int GetOrderRadius()
    {
        return orderRange;
    }
}
