using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera firstPlayerRtsCamera;
    public CinemachineCamera secondPlayerRtsCamera;
    
    private List<ICamera> cameras;

    void Awake()
    {
        cameras = new List<ICamera>();
    }

    public void ActivateFirstRTS(){
        cameras?.ForEach(cam=>cam.RemovePriority());
        firstPlayerRtsCamera.Priority=1;
        secondPlayerRtsCamera.Priority=0;

        firstPlayerRtsCamera.Target.TrackingTarget.GetComponent<RTSCamera>().enabled=true;
        secondPlayerRtsCamera.Target.TrackingTarget.GetComponent<RTSCamera>().enabled=false;
    }
    public void ActivateSecondRTS(){
        cameras?.ForEach(cam=>cam.RemovePriority());
        firstPlayerRtsCamera.Priority=0;
        secondPlayerRtsCamera.Priority=1;
        
        firstPlayerRtsCamera.Target.TrackingTarget.GetComponent<RTSCamera>().enabled=false;
        secondPlayerRtsCamera.Target.TrackingTarget.GetComponent<RTSCamera>().enabled=true;
    }

    public void DesactivateRTS(){
        firstPlayerRtsCamera.Priority=0;
        secondPlayerRtsCamera.Priority=0;

        firstPlayerRtsCamera.Target.TrackingTarget.GetComponent<RTSCamera>().enabled=false;
        secondPlayerRtsCamera.Target.TrackingTarget.GetComponent<RTSCamera>().enabled=false;
    }

    public List<ICamera> GetPOVCameras(){
        return cameras;
    }

    

    public void RegisterCamera(ICamera camera){
        cameras.Add(camera);
    }

    public void UnregisterCamera(ICamera camera){
        cameras.Remove(camera);
    }
    
}

public interface ICamera{
    Tile GetPosition();
    void SetPriority();
    void RemovePriority();
    int GetOrderRadius();
}
