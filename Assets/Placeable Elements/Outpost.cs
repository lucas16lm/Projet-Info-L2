using Unity.Cinemachine;
using UnityEngine;

public class Outpost : PlaceableObject, ICamera, ITurnObserver
{
    public int orderRange;
    private int turnToBuild = 0;
    public override void Initialize(PlaceableData placeableData, Tile position, Player player)
    {
        OutpostData data = placeableData as OutpostData;
        
        GameManager.instance.cameraManager.RegisterCamera(this);
        GameManager.instance.turnManager.AddObserver(this);

        turnToBuild=data.turnToBuild;
        
        orderRange=data.orderRange;
        player.outposts.Add(this);
        healthPoints=data.baseHealthPoints;
        this.position=position;
        position.content=this;

        foreach(Renderer renderer in transform.GetChild(0).GetComponentsInChildren<Renderer>()) renderer.material=player.factionData.bannerMaterial;
    }

    public bool IsConstructed(){
        return turnToBuild<=0;
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

    public void OnTurnEnded()
    {
        AddBuildingProgress();
        if(turnToBuild<=0){
            Debug.Log(name+"finished");
            StartCoroutine(GameManager.instance.turnManager.RemoveObserver(this));
        }
    }

    public void AddBuildingProgress()
    {
        turnToBuild--;
        switch(turnToBuild){
            case 0:
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(3).gameObject.SetActive(false);
                break;
            case 1:
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(3).gameObject.SetActive(false);
                break;
            case 2:
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(false);
                transform.GetChild(3).gameObject.SetActive(true);
                break;
        }
    }

    public override void DammagedBy(Unit unit, int bonusDamage)
    {
        healthPoints-=(unit.unitData.baseDamagePoints+bonusDamage);
        if(healthPoints<=0){
            GameManager.instance.cameraManager.UnregisterCamera(this);
            //TODO gérer destruction pendant construction
            Kill();
        }
    }

    public override void Kill()
    {
        transform.parent.GetComponent<Player>().outposts.Remove(this);
        position.content=null;
        Destroy(gameObject);
    }
}
