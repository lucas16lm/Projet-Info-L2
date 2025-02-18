using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera firstPlayerRtsCamera;
    public CinemachineCamera secondPlayerRtsCamera;
    public List<CinemachineCamera> firstPlayerCameras;
    public List<CinemachineCamera> secondPlayerCameras;

    public void SetCameraState(CameraState cameraState){
        switch(cameraState){
            case CameraState.FirstPlayerRTS:
                firstPlayerRtsCamera.Priority=1;
                secondPlayerRtsCamera.Priority=0;
                firstPlayerCameras.ForEach(cam => cam.Priority=0);
                secondPlayerCameras.ForEach(cam => cam.Priority=0);
                firstPlayerRtsCamera.Target.TrackingTarget.GetComponent<RTSCamera>().enabled=true;
                secondPlayerRtsCamera.Target.TrackingTarget.GetComponent<RTSCamera>().enabled=false;
                break;

            case CameraState.SecondPlayerRTS:
                firstPlayerRtsCamera.Priority=0;
                secondPlayerRtsCamera.Priority=1;
                firstPlayerCameras.ForEach(cam => cam.Priority=0);
                secondPlayerCameras.ForEach(cam => cam.Priority=0);
                firstPlayerRtsCamera.Target.TrackingTarget.GetComponent<RTSCamera>().enabled=false;
                secondPlayerRtsCamera.Target.TrackingTarget.GetComponent<RTSCamera>().enabled=true;
                break;
        }
    }

    public void AddFirstPlayerCamera(CinemachineCamera camera){
        firstPlayerCameras.Add(camera);
    }

    
}

public enum CameraState{
    FirstPlayerRTS,
    SecondPlayerRTS,
    FirstPlayerPOV,
    SecondPlayerPOV
}
