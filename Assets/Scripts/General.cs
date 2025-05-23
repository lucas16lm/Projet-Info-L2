using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;

public class General : Building, ICamera
{
    public GeneralData GeneralData { get { return data as GeneralData; } }
    public int orderRange;
    
    public override void Initialize(PlaceableData placeableData, Tile position, Player player)
    {
        data = placeableData;
        
        GameManager.instance.cameraManager.RegisterCamera(this);

        orderRange=GeneralData.orderRange;
        player.general=this;
        healthPoints=data.baseHealthPoints;
        this.position=position;
        position.Content=this;

        foreach(Renderer renderer in transform.GetChild(0).GetComponentsInChildren<Renderer>()) renderer.material=player.factionData.bannerMaterial;
        foreach(Renderer renderer in transform.GetChild(3).GetComponentsInChildren<Renderer>()) renderer.material=player.factionData.unitsMaterial;

    }

    public void SetPriority()
    {
        GameManager.instance.cameraManager.DesactivateRTS();
        GameManager.instance.cameraManager.GetPOVCameras().ForEach(cam=>cam.RemovePriority());

        GetComponentInChildren<CinemachinePanTilt>().PanAxis.Value=0;
        GetComponentInChildren<CinemachinePanTilt>().TiltAxis.Value=0;
        
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
        GetComponentInChildren<LocalCanvas>().UpdateCanvas();
        if(healthPoints<=0){
            Kill();
        }
    }

    public override void Kill()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        GetComponent<Animator>().SetTrigger("Destroy");
        GetComponent<AudioSource>().PlayOneShot(data.deathSound);
        transform.parent.GetComponent<Player>().general=null;
        position.Content=null;
        if(transform.parent.GetComponent<Player>().playerRole==PlayerRole.FirstPlayer){
            GameManager.instance.turnManager.PlayerWon(GameManager.instance.playerManager.secondPlayer);
        }
        else{
            GameManager.instance.turnManager.PlayerWon(GameManager.instance.playerManager.firstPlayer);
        }
        Tween.Delay(4, ()=>Destroy(gameObject));
    }

    public override bool IsConstructed()
    {
        return true;
    }
}
