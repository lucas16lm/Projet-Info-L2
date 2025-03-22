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
        foreach(Renderer renderer in transform.GetChild(3).GetComponentsInChildren<Renderer>()) renderer.material=player.factionData.unitsMaterial;

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

    public override void DammagedBy(Unit unit, int damagePoints)
    {
        healthPoints-=damagePoints;
        if(healthPoints<=0){
            Kill();
        }
    }

    public override void Kill()
    {
        transform.parent.GetComponent<Player>().general=null;
        position.content=null;
        Destroy(gameObject);
        if(transform.parent.GetComponent<Player>().playerRole==PlayerRole.FirstPlayer){
            GameManager.instance.turnManager.PlayerWon(GameManager.instance.playerManager.secondPlayer);
        }
        else{
            GameManager.instance.turnManager.PlayerWon(GameManager.instance.playerManager.firstPlayer);
        }
    }
}
