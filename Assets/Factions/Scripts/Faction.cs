using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Faction : MonoBehaviour
{
    public FactionData data;
    public RessourceBalance ressourceBalance;

    public General general;
    public List<Officer> officers;
    public List<Unit> units;
    public List<Building> buildings;
    public List<PlaceableObject> placeableObjects{get{
        List<PlaceableObject> list = new List<PlaceableObject>(){general};
        list.AddRange(officers);
        list.AddRange(units);
        list.AddRange(buildings);
        return list;
    }}

    public void InitializeFaction(FactionData factionData)
    {
        transform.name=factionData.name;
        data=factionData;
        ressourceBalance = factionData.baseBalance;
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
            GameManager.instance.uIManager.UpdateRessourcePanel(this);
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
        GameManager.instance.uIManager.endTurnButton.GetComponent<Button>().onClick.AddListener(delegate{turnEnded=true; Debug.Log("turn ended");});
        
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
