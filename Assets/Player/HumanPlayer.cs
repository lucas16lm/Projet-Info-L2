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
    
    #region Deployment
    public override IEnumerator Deployment(Action onComplete)
    {
        List<Tile> deploymentZone = GetDeploymentZone();
        deploymentZone.ForEach(tile=>tile.SetOutline(true, GameManager.instance.TileLayerId));
        
        bool turnEnded = false;

        yield return GeneralDeployment(deploymentZone);

        GameManager.instance.uIManager.deploymentPanel.SetActive(true);
        Coroutine unitDeployment = StartCoroutine(UnitDeployment(deploymentZone));
        InputSystem.actions.FindAction("EndTurn").performed+=ctx=>turnEnded=true;
        

        yield return new WaitUntil(()=>turnEnded);
        
        deploymentZone.ForEach(tile=>tile.SetOutline(false, GameManager.instance.TileLayerId));
        StopCoroutine(unitDeployment);
        onComplete();
    }
    IEnumerator GeneralDeployment(List<Tile> deploymentZone){
        Debug.Log("Placement du général de "+factionData.name);
        GameManager.instance.uIManager.PrintMessage(factionData.factionName+", place your general !");

        bool generalPlaced = false;
        Coroutine generalPlacement = StartCoroutine(MouseListener(
            go=>go?.GetComponent<Tile>()!=null && deploymentZone.Contains(go?.GetComponent<Tile>()),
            go=>{},
            go=>{},
            go=>{PlaceableObject.Instantiate(factionData.generalData, go.GetComponent<Tile>(), this); if(general!=null)generalPlaced=true;}
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
            GameManager.instance.uIManager.deploymentPanel.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate{unitToPlace=unitData;});
        }

        
        
        Coroutine unitPlacement = StartCoroutine(MouseListener(
            go=>go?.GetComponent<Tile>()!=null || (go?.GetComponent<Unit>()!=null && units.Contains(go.GetComponent<Unit>())),
            go=>{if(go?.GetComponent<Unit>()!=null) go.GetComponent<Unit>().SetOutline(true, GameManager.instance.AllyLayerId);},
            go=>{if(go?.GetComponent<Unit>()!=null) go.GetComponent<Unit>().DisableOutlines();},
            go=>{
                Tile tile = go?.GetComponent<Tile>();
                Unit unit = go?.GetComponent<Unit>();

                if(tile!=null && unitToPlace != null && !tile.occupied && tile.occupable && deploymentZone.Contains(tile)){
                    PlaceableObject.Instantiate(unitToPlace, tile, this);
                }
                if(unit!=null){
                    ressourceBalance.AddRessources(unit.cost);
                    GameManager.instance.uIManager.UpdateRessourcePanel(this);
                    unit.position.occupied=false;
                    units.Remove(unit);
                    Destroy(go);
                }
            }
        ));

        yield return new WaitUntil(()=>turnEnded);
        StopCoroutine(unitPlacement);

    }
    #endregion

    #region Turn
    public override IEnumerator PlayTurn(Action onComplete)
    {
        bool turnEnded = false;

        InputSystem.actions.FindAction("EndTurn").performed+=ctx=>turnEnded=true;

        PlaceableObject selected = null;
        PlaceableObject target = null;
        Tile destination = null;

        Coroutine selectCoroutine = StartCoroutine(MouseListener(   //idée éventuelle : surcharge méthode MouseListener prenant des IENumerator au lieu des actions en paramètre
            go=>go?.GetComponent<PlaceableObject>()!=null,      //TODO : créer méthode retournant bool pour gérer chaque cas de figure
            go=>{
                PlaceableObject placeableObject = go.GetComponent<PlaceableObject>();   //TODO : idem pour les actions
                if(GetPlaceableObjects().Contains(placeableObject)) placeableObject.SetOutline(true, GameManager.instance.AllyLayerId);
                else placeableObject.SetOutline(true, GameManager.instance.EnnemyLayerId);
            },
            go=>{
                go.GetComponent<PlaceableObject>().DisableOutlines();
            },
            go=>{
                selected=go.GetComponent<PlaceableObject>();
            })
        );
        
        yield return new WaitUntil(()=>turnEnded);

        GameManager.instance.playerManager.firstPlayer.GetPlaceableObjects().ForEach(o=>o.DisableOutlines());
        GameManager.instance.playerManager.secondPlayer.GetPlaceableObjects().ForEach(o=>o.DisableOutlines());

        StopCoroutine(selectCoroutine);
        onComplete();
    }

    IEnumerator HandleSelectedObject(PlaceableObject placeableObject){
        switch(placeableObject){
            case Unit:
                Unit unit = placeableObject as Unit;
                break;
            case General or Officer:
                placeableObject.GetComponentInChildren<CinemachineCamera>().Prioritize();
                break;
            default:
                Debug.Log("pas unit");
                break;
        }
        yield return null;
    }

    IEnumerator ChooseTarget(Unit unit, Action<PlaceableObject> onComplete){
        bool completed=false;
        Coroutine chooseCoroutine = StartCoroutine(MouseListener(
            go=>{
                PlaceableObject placeableObject = go?.GetComponent<PlaceableObject>();
                if(placeableObject==null) return false;
                return true;
            },
            go=>{},
            go=>{},
            go=>{onComplete(go.GetComponent<PlaceableObject>()); completed=true;}
        ));

        yield return new WaitUntil(()=>completed);
        
        StopCoroutine(chooseCoroutine);
    }



    #endregion

    #region Wait
    public override IEnumerator Wait(Action onComplete)
    {
        yield return null;
        onComplete();
    }
    #endregion
    public IEnumerator MouseListener(Predicate<GameObject> predicate, Action<GameObject> onEnter, Action<GameObject> onExit, Action<GameObject> onClick){
        GameObject previousObject = null;

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            GameObject currentObject = null;

            if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
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
                if(InputSystem.actions.FindAction("Select").WasPerformedThisFrame() && predicate(currentObject)) onClick(currentObject);
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
