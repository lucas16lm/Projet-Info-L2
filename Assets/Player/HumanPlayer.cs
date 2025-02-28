using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HumanPlayer : Player
{
    public override IEnumerator Deployment(Action onComplete)
    {
        yield return GeneralDeployment();
        yield return UnitDeployment();
        onComplete();
    }

    public override IEnumerator PlayTurn(Action onComplete)
    {
        yield return null;
        onComplete();
    }

    public override IEnumerator Wait(Action onComplete)
    {
        yield return null;
        onComplete();
    }

    IEnumerator GeneralDeployment(){
        List<Tile> deploymentZone = GetDeploymentZone();
        Debug.Log("Placement du général de "+factionData.name);
        GameManager.instance.uIManager.PrintMessage(factionData.factionName+", place your general !");
        deploymentZone.ForEach(tile => tile.GetComponent<OutlineManager>().Outline());
        bool generalPlaced = false;
        
        while (!generalPlaced)
        {
            StartCoroutine(SelectMatchingGameObject(
                go =>{
                Tile tile = go?.GetComponent<Tile>();
                if(tile == null) return false;
                return deploymentZone.Contains(tile) && !tile.occupied && tile.occupable;
                },
                go=>{
                    PlaceElement(factionData.generalData, go.GetComponent<Tile>());
                    general.GetComponent<OutlineManager>().Outline();
                    generalPlaced=true;
                }));
            
            yield return new WaitUntil(()=>generalPlaced);
        }
    }
    IEnumerator UnitDeployment(){

        InputAction endTurnAction = InputSystem.actions.FindAction("EndTurn");
        if(endTurnAction.WasPerformedThisFrame()){
            Debug.Log("end");
            yield break;
        }

        List<Tile> deploymentZone = GetDeploymentZone();
        Debug.Log("Placement des troupes de "+factionData.factionName);
        GameManager.instance.uIManager.PrintMessage(factionData.name+", compose your army !");
        
        List<OutlineManager> outlineManagers = new List<OutlineManager>();
        deploymentZone.ForEach(tile => outlineManagers.Add(tile.GetComponent<OutlineManager>()));
        outlineManagers.Add(general.GetComponent<OutlineManager>());

        bool turnEnded=false;

        UnitData unitToPlace = null;
        
        for (int i = 0; i < factionData.factionUnitsData.Count; i++)
        {
            UnitData unitData = factionData.factionUnitsData[i];
            GameManager.instance.uIManager.recruitmentPanel.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate{unitToPlace=unitData ; Debug.Log("selected : "+unitData.elementName);});
        }

        while(!turnEnded){
            
            yield return SelectMatchingGameObject(
                go =>{
                    if(go?.GetComponent<Tile>()==null && go?.GetComponent<Unit>()==null) return false;
                    Tile tile = go?.GetComponent<Tile>();
                    Unit unit = go?.GetComponent<Unit>();
                    if(tile!=null) return unitToPlace!=null && deploymentZone.Contains(tile) && !tile.occupied && tile.occupable;
                    else return units.Contains(unit);
                    },
                go=>{
                    Tile tile = go?.GetComponent<Tile>();
                    Unit unit = go?.GetComponent<Unit>();
                    if(tile != null){
                        GameObject unitGO = PlaceElement(unitToPlace, go.GetComponent<Tile>());
                        if(unitGO!=null){
                            outlineManagers.Add(unitGO.GetComponent<OutlineManager>());
                            unitGO.GetComponent<OutlineManager>().Outline();
                            unitToPlace=null;
                        }
                    }
                    else{
                        ressourceBalance.AddRessources(unit.cost);
                        //GameManager.instance.uIManager.UpdateRessourcePanel(playerFaction);
                        unit.position.occupied=false;
                        units.Remove(unit);
                        outlineManagers.Remove(unit.GetComponent<OutlineManager>());
                        Destroy(go);
                    }
                });

            yield return null;
        }
        outlineManagers.ForEach(outline => outline.DisableOutline());
    }

    public GameObject PlaceElement(PlaceableData placeableElement, Tile tile){
        if(!ressourceBalance.RemoveRessources(placeableElement.cost)){
            Debug.Log("Not enought ressources !");
            return null;
        }
        
        GameObject element = Instantiate(placeableElement.gameObjectPrefab, tile.gameObject.transform.position+(tile.transform.localScale.y/2)*Vector3.up, Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)), transform);
        
        if(placeableElement is GeneralData){
            general=element.GetComponent<General>();
        }
        else if(placeableElement is OfficerData){
            officers.Add(element.GetComponent<Officer>());
        }
        else if(placeableElement is UnitData){
            UnitData unitData = (UnitData)placeableElement;
            Unit unit = element.GetComponent<Unit>();
            units.Add(unit);
            unit.Initialize(unitData, tile);
            //GameManager.instance.uIManager.UpdateRessourcePanel(this);
        }
        else if(placeableElement is BuildingData){
            buildings.Add(element.GetComponent<Building>());
        }
        tile.occupied=true;
        return element;
    }

    public IEnumerator SelectMatchingGameObject(Predicate<GameObject> predicate, Action<GameObject> action){
        bool done = false;
        
        InputAction selectAction = InputSystem.actions.FindAction("Select");
        InputAction cancelAction = InputSystem.actions.FindAction("Cancel");
        
        bool turnEnded=false;
        
        while(!done){
            
            if((cancelAction.WasPerformedThisFrame() && !EventSystem.current.IsPointerOverGameObject()) || turnEnded){
                yield break;
            }

            if (selectAction.WasPerformedThisFrame() && !EventSystem.current.IsPointerOverGameObject()){
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit)){
                    GameObject clicked = hit.collider.gameObject;
                    if(predicate(clicked)){
                        action(clicked);
                        done = true;
                    }
                }
            }
            yield return null;
        }
    }
}
