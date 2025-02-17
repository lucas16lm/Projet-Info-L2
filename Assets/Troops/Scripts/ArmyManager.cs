using UnityEngine;
using UnityEngine.InputSystem;

public class ArmyManager : MonoBehaviour
{
    private InputAction inputAction;
    public bool hasGeneral = false;

    void Start()
    {
        inputAction = InputSystem.actions.FindAction("Select");
    }

    public void PlaceUnit(Tile target){
        //TODO
    }

    public void PlaceBuilding(){
        //TODO
    }

    public void SelectUnit(){
        //TODO
    }
}
