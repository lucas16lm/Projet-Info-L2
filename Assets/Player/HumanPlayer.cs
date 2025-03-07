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

        Coroutine hoverCoroutine = StartCoroutine(MouseListener(
            go=>go?.GetComponent<PlaceableObject>()!=null,
            go=>{
                PlaceableObject placeableObject = go.GetComponent<PlaceableObject>();
                if(GetPlaceableObjects().Contains(placeableObject)) placeableObject.SetOutline(true, GameManager.instance.AllyLayerId);
                else placeableObject.SetOutline(true, GameManager.instance.EnnemyLayerId);
            },
            go=>{
                go.GetComponent<PlaceableObject>().DisableOutlines();
            },
            go=>{})
        );

        while(!turnEnded){
            yield return HandlePlayerSelection();
        }
        
        
        yield return new WaitUntil(()=>turnEnded);

        GameManager.instance.playerManager.firstPlayer.GetPlaceableObjects().ForEach(o=>o.DisableOutlines());
        GameManager.instance.playerManager.secondPlayer.GetPlaceableObjects().ForEach(o=>o.DisableOutlines());

        StopCoroutine(hoverCoroutine);
        onComplete();
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

    public IEnumerator SelectGameObject(Predicate<GameObject> predicate, Action<GameObject> action){
        bool completed = false;
        InputSystem.actions.FindAction("EndTurn").performed += ctx=>completed=true;
        InputSystem.actions.FindAction("Cancel").performed += ctx=>completed=true;

        while(!completed){
            if(InputSystem.actions.FindAction("Select").WasPerformedThisFrame()){
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject() && predicate.Invoke(hit.collider.gameObject)){
                    
                    action?.Invoke(hit.collider.gameObject);
                    completed=true;
                }
            }
            yield return null;
        }
    }

    public IEnumerator HandlePlayerSelection(){
        
        GameObject firstSelection=null;
        yield return SelectGameObject(
            go=>IsTileRecruitable(go) || IsOwnUnit(go) || IsOwnGeneral(go) || IsOwnOfficer(go),
            go=>firstSelection=go
        );

        if(firstSelection==null) yield break;

        if(IsOwnGeneral(firstSelection) || IsOwnOfficer(firstSelection)){
            Debug.Log("TODO : click on general and officers");
            yield return null;
        }else if(IsTileRecruitable(firstSelection))
        {
            GameManager.instance.uIManager.OpenReinforcementPanel(this);
            Debug.Log("TODO : reinforcement");
            yield return null;
        }
        else if(IsOwnUnit(firstSelection)){
            GameObject secondObject = null;
            while(secondObject==null){
                yield return SelectGameObject(
                    go=>IsTile(go) || IsRivalElement(go),
                    go=>secondObject=go
                );
            }
            if(IsTile(secondObject)){
                yield return firstSelection.GetComponent<Unit>().Move(secondObject.GetComponent<Tile>());
            }
            else if(IsRivalElement(secondObject)){
                Debug.Log("TODO : attack");
            yield return null;
            }
            else{
                Debug.LogError("not supposed to be here");
            }
        }
        

        
    }

    bool IsTileRecruitable(GameObject go){
        Tile tile = go?.GetComponent<Tile>();
        if(tile==null) return false;
        return tile.IsAccessible() && Tile.GetTilesInRange(general.position, general.orderRange).Contains(tile);
    }

    bool IsOwnUnit(GameObject go){
        Unit unit = go?.GetComponent<Unit>();
        if(unit==null) return false;
        return units.Contains(unit);
    }

    bool IsOwnGeneral(GameObject go){
        General general = go?.GetComponent<General>();
        if(general==null) return false;
        return general==this.general;
    }

    bool IsOwnOfficer(GameObject go){
        Officer officer = go?.GetComponent<Officer>();
        if(officer==null) return false;
        return officers.Contains(officer);
    }

    bool IsTile(GameObject go){
        Tile tile = go?.GetComponent<Tile>();
        return tile!=null;
    }

    bool IsRivalElement(GameObject go){
        PlaceableObject placeableObject = go?.GetComponent<PlaceableObject>();
        if(placeableObject==null) return false;
        return !GetPlaceableObjects().Contains(placeableObject);
    }
}
