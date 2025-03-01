using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HumanPlayer : Player
{
    public override IEnumerator Deployment(Action onComplete)
    {
        List<Tile> deploymentZone = GetDeploymentZone();
        deploymentZone.ForEach(tile=>tile.SetOutline(true));

        GameManager.instance.uIManager.SetUpUI(UiState.Deployment, this);
        bool turnEnded = false;

        yield return GeneralDeployment(deploymentZone);

        Coroutine unitDeployment = StartCoroutine(UnitDeployment(deploymentZone));
        InputSystem.actions.FindAction("EndTurn").performed+=ctx=>turnEnded=true;
        

        yield return new WaitUntil(()=>turnEnded);
        
        deploymentZone.ForEach(tile=>tile.SetOutline(false));
        GameManager.instance.uIManager.SetUpUI(UiState.Nothing, this);
        StopCoroutine(unitDeployment);
        onComplete();
    }

    public override IEnumerator PlayTurn(Action onComplete)
    {
        bool turnEnded = false;
        GameManager.instance.uIManager.SetUpUI(UiState.POV, this);

        InputSystem.actions.FindAction("EndTurn").performed+=ctx=>turnEnded=true;
        
        yield return new WaitUntil(()=>turnEnded);
        onComplete();
    }

    public override IEnumerator Wait(Action onComplete)
    {
        yield return null;
        onComplete();
    }

    IEnumerator GeneralDeployment(List<Tile> deploymentZone){
        Debug.Log("Placement du général de "+factionData.name);
        GameManager.instance.uIManager.PrintMessage(factionData.factionName+", place your general !");

        bool generalPlaced = false;
        Coroutine generalPlacement = StartCoroutine(MouseListener(
            go=>go?.GetComponent<Tile>()!=null,
            go=>{},
            go=>{},
            go=>{PlaceElement(factionData.generalData, go.GetComponent<Tile>()); generalPlaced=true;}
        ));

        yield return new WaitUntil(()=>generalPlaced);
        StopCoroutine(generalPlacement);
        
    }
    IEnumerator UnitDeployment(List<Tile> deploymentZone){
        Debug.Log("Placement des troupes de "+factionData.factionName);
        GameManager.instance.uIManager.PrintMessage(factionData.name+", compose your army !");
        bool turnEnded=false;
        InputSystem.actions.FindAction("EndTurn").performed+=ctx=>turnEnded=true;


        UnitData unitToPlace = null;
        
        for (int i = 0; i < factionData.factionUnitsData.Count; i++)
        {
            UnitData unitData = factionData.factionUnitsData[i];
            GameManager.instance.uIManager.deploymentPanel.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate{unitToPlace=unitData ; Debug.Log("selected : "+unitData.elementName);});
        }
        
        Coroutine unitPlacement = StartCoroutine(MouseListener(
            go=>go?.GetComponent<Tile>()!=null || go?.GetComponent<Unit>()!=null,
            go=>{if(go?.GetComponent<Unit>()!=null) go.GetComponent<Unit>().SetOutline(true);},
            go=>{if(go?.GetComponent<Unit>()!=null) go.GetComponent<Unit>().SetOutline(false);},
            go=>{
                Tile tile = go?.GetComponent<Tile>();
                Unit unit = go?.GetComponent<Unit>();

                if(tile!=null && unitToPlace != null && !tile.occupied && tile.occupable && deploymentZone.Contains(tile)){
                    PlaceElement(factionData.factionUnitsData[0], tile);
                    GameManager.instance.uIManager.SetUpUI(UiState.Deployment, this);
                }
                if(unit!=null){
                    ressourceBalance.AddRessources(unit.cost);
                    GameManager.instance.uIManager.SetUpUI(UiState.Deployment, this);
                    unit.position.occupied=false;
                    units.Remove(unit);
                    Destroy(go);
                }
            }
        ));

        yield return new WaitUntil(()=>turnEnded);
        StopCoroutine(unitPlacement);

    }

    public GameObject PlaceElement(PlaceableData placeableElement, Tile tile){
        if(!ressourceBalance.RemoveRessources(placeableElement.cost)){
            Debug.Log("Not enought ressources !");
            return null;
        }
        
        GameObject element = Instantiate(placeableElement.gameObjectPrefab, tile.gameObject.transform.position+(tile.transform.localScale.y/2)*Vector3.up, Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)), transform);
        
        if(placeableElement is GeneralData){
            general=element.GetComponent<General>();
            GameManager.instance.cameraManager.AddFirstPlayerCamera(element.GetComponentInChildren<CinemachineCamera>());
        }
        else if(placeableElement is OfficerData){
            officers.Add(element.GetComponent<Officer>());
        }
        else if(placeableElement is UnitData){
            UnitData unitData = (UnitData)placeableElement;
            Unit unit = element.GetComponent<Unit>();
            units.Add(unit);
            unit.Initialize(unitData, tile);
            GameManager.instance.uIManager.SetUpUI(UiState.Deployment, this);
        }
        else if(placeableElement is BuildingData){
            buildings.Add(element.GetComponent<Building>());
        }
        tile.occupied=true;
        return element;
    }


    public IEnumerator MouseListener(Predicate<GameObject> predicate, Action<GameObject> onEnter, Action<GameObject> onExit, Action<GameObject> onClick){
        GameObject previousObject = null;

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            GameObject currentObject = null;

            if (Physics.Raycast(ray, out hit))
            {
                currentObject = hit.collider.gameObject;
                if (currentObject != previousObject)
                {
                    if (previousObject != null && predicate(previousObject))
                    {
                        onExit(previousObject);
                    }

                    if(predicate(currentObject)) onEnter(currentObject);
                }
                if(InputSystem.actions.FindAction("Select").WasPerformedThisFrame()) onClick(currentObject);
            }
            else
            {
                if (previousObject != null && predicate(previousObject))
                {
                    onExit(previousObject);
                }
                currentObject = null;
            }

            previousObject = currentObject;

            yield return null;
        }
        }
}
