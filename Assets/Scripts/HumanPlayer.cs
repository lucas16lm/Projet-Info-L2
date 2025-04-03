using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HumanPlayer : Player, ITurnObserver
{
    public ICamera currentCam;

    #region Deployment
    public override IEnumerator Deployment(Action onComplete)
    {
        GameManager.instance.turnManager.AddObserver(this);
        List<Tile> deploymentZone = GetDeploymentZone();
        deploymentZone.ForEach(tile => tile.SetOutline(true, GameManager.instance.TileZoneLayerID));

        bool turnEnded = false;

        yield return GeneralDeployment(deploymentZone);

        GameManager.instance.uIManager.deploymentPanel.OpenFor(this);
        Coroutine unitDeployment = StartCoroutine(UnitDeployment(deploymentZone));
        InputSystem.actions.FindAction("EndTurn").performed += ctx => turnEnded = true;


        yield return new WaitUntil(() => turnEnded);

        GameManager.instance.uIManager.deploymentPanel.Close();
        deploymentZone.ForEach(tile => tile.SetOutline(false, GameManager.instance.TileZoneLayerID));
        StopCoroutine(unitDeployment);
        onComplete();
    }
    IEnumerator GeneralDeployment(List<Tile> deploymentZone)
    {
        Debug.Log("Placement du général de " + factionData.name);
        GameManager.instance.uIManager.PrintMessage(factionData.factionName + ", place your general !");

        bool generalPlaced = false;
        Coroutine generalPlacement = StartCoroutine(MouseListener(
            go => go?.GetComponent<Tile>() != null && deploymentZone.Contains(go?.GetComponent<Tile>()),
            go => { },
            go => { },
            go => { PlaceableObject.Instantiate(factionData.generalData, go.GetComponent<Tile>(), this); if (general != null) generalPlaced = true; }
        ));

        yield return new WaitUntil(() => generalPlaced);
        StopCoroutine(generalPlacement);

    }
    IEnumerator UnitDeployment(List<Tile> deploymentZone)
    {
        Debug.Log("Placement des troupes de " + factionData.factionName);
        GameManager.instance.uIManager.PrintMessage(factionData.name + ", compose your army !");
        bool turnEnded = false;
        InputSystem.actions.FindAction("EndTurn").performed += ctx => turnEnded = true;

        DeploymentPanel deploymentPanel = GameManager.instance.uIManager.deploymentPanel;

        Coroutine unitPlacement = StartCoroutine(MouseListener(
            go => go?.GetComponent<Tile>() != null || (go?.GetComponent<Unit>() != null && units.Contains(go.GetComponent<Unit>())),
            go =>
            {
                if (go?.GetComponent<Unit>() != null)
                {
                    go.GetComponent<Unit>().SetOutline(true, GameManager.instance.AllyLayerId);
                    GameManager.instance.soundManager.PlaySound("Hover");
                }
            },
            go => { if (go?.GetComponent<Unit>() != null) go.GetComponent<Unit>().DisableOutlines(); },
            go =>
            {
                Tile tile = go?.GetComponent<Tile>();
                Unit unit = go?.GetComponent<Unit>();

                if (tile != null && deploymentPanel.GetSelectedUnit() != null && tile.IsAccessible() && deploymentZone.Contains(tile))
                {
                    PlaceableObject.Instantiate(deploymentPanel.GetSelectedUnit(), tile, this);
                }
                if (unit != null)
                {
                    GameManager.instance.soundManager.PlaySound("Gold");
                    ressourceBalance.AddRessources(unit.cost);
                    GameManager.instance.uIManager.UpdateRessourcePanel(this);
                    unit.position.Content = null;
                    units.Remove(unit);
                    Destroy(go);
                }
            }
        ));

        yield return new WaitUntil(() => turnEnded);
        StopCoroutine(unitPlacement);

    }
    #endregion

    #region Turn
    public override IEnumerator PlayTurn(Action onComplete)
    {
        currentCam = general.GetComponent<ICamera>();
        Tile.GetTilesInRange(currentCam.GetPosition(), currentCam.GetOrderRadius()).ForEach(tile => tile.SetOutline(true, GameManager.instance.TileZoneLayerID));
        bool turnEnded = false;

        InputSystem.actions.FindAction("EndTurn").performed += ctx => turnEnded = true;
        //TODO : améliorer la coroutine pour adpater et mettre en évidence si une action est possible seulement
        Coroutine hoverCoroutine = StartCoroutine(MouseListener(
            go => go?.GetComponent<PlaceableObject>() != null || IsTile(go),
            go =>
            {
                if (IsTile(go))
                {
                    go.GetComponent<Tile>().SetOutline(true, GameManager.instance.TileSelectLayerID);
                }
                else
                {
                    PlaceableObject placeableObject = go.GetComponent<PlaceableObject>();
                    GameManager.instance.soundManager.PlaySound("Hover");
                    if (GetPlaceableObjects().Contains(placeableObject)) placeableObject.SetOutline(true, GameManager.instance.AllyLayerId);
                    else placeableObject.SetOutline(true, GameManager.instance.EnnemyLayerId);
                    placeableObject.GetComponentInChildren<LocalCanvas>().ShowCanvas();
                }

            },
            go =>
            {
                if (IsTile(go))
                {
                    go.GetComponent<Tile>().SetOutline(false, GameManager.instance.TileSelectLayerID);
                }
                else
                {
                    go.GetComponent<IOutlinable>().DisableOutlines();
                    go.GetComponentInChildren<LocalCanvas>().HideCanvas();
                }

            },
            go => { })
        );


        while (!turnEnded)
        {
            yield return HandlePlayerSelection();
        }


        yield return new WaitUntil(() => turnEnded);

        GameManager.instance.playerManager.firstPlayer.GetPlaceableObjects().ForEach(o => o.DisableOutlines());
        GameManager.instance.playerManager.secondPlayer.GetPlaceableObjects().ForEach(o => o.DisableOutlines());
        Tile.GetTiles().ForEach(tile => tile.DisableOutlines());

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
    public IEnumerator MouseListener(Predicate<GameObject> predicate, Action<GameObject> onEnter, Action<GameObject> onExit, Action<GameObject> onClick)
    {
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

                    if (predicate(currentObject)) onEnter(currentObject);
                }
                if (InputSystem.actions.FindAction("Select").WasPerformedThisFrame() && predicate(currentObject)) onClick(currentObject);
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

    public IEnumerator SelectGameObject(Predicate<GameObject> predicate, Action<GameObject> action)
    {
        bool completed = false;
        InputSystem.actions.FindAction("EndTurn").performed += ctx => completed = true;
        InputSystem.actions.FindAction("Cancel").performed += ctx => completed = true;

        while (!completed)
        {
            if (InputSystem.actions.FindAction("Select").WasPerformedThisFrame())
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject() && predicate.Invoke(hit.collider.gameObject))
                {

                    action?.Invoke(hit.collider.gameObject);
                    completed = true;
                }
            }
            yield return null;
        }
    }

    public IEnumerator HandlePlayerSelection()
    {
        GameObject firstSelection = null;
        yield return SelectGameObject(
            go => IsInOrderRange(go) && (IsTileRecruitable(go) || IsOwnUnit(go) || IsOwnGeneral(go) || IsOwnOutpost(go)),
            go => firstSelection = go
        );

        if (firstSelection == null) yield break;

        if (IsOwnGeneral(firstSelection) || (IsOwnOutpost(firstSelection) && firstSelection.GetComponent<Outpost>().IsConstructed()))
        {
            Tile.GetTilesInRange(currentCam.GetPosition(), currentCam.GetOrderRadius()).ForEach(tile => tile.DisableOutlines());
            ICamera camera = firstSelection.GetComponent<ICamera>();
            camera.SetPriority();
            currentCam = camera;
            Tile.GetTilesInRange(currentCam.GetPosition(), currentCam.GetOrderRadius()).ForEach(tile => tile.SetOutline(true, GameManager.instance.TileZoneLayerID));
        }
        else if (IsTileRecruitable(firstSelection))
        {
            if (currentCam is General)
            {
                PlaceableData elementToPlace = null;
                yield return GameManager.instance.uIManager.reinforcementPanel.OpenFor(this, data => elementToPlace = data);
                if (elementToPlace != null){
                    PlaceableObject.Instantiate(elementToPlace, firstSelection.GetComponent<Tile>(), this);
                } 
            }
            else
            {
                PlaceableData elementToPlace = null;
                yield return GameManager.instance.uIManager.reinforcementPanel.OpenForBuildings(this, data => elementToPlace = data);
                if (elementToPlace != null) PlaceableObject.Instantiate(elementToPlace, firstSelection.GetComponent<Tile>(), this);
            }

        }
        else if (IsOwnUnit(firstSelection))
        {
            GameManager.instance.soundManager.PlaySound("UnitSelection");
            bool canceled = false;
            InputSystem.actions.FindAction("Cancel").performed += ctx => canceled = true;
            Unit unit = firstSelection.GetComponent<Unit>();
            List<Tile> accessibles = unit.GetAccessibleTiles();
            accessibles.ForEach(tile => tile.SetOutline(true, GameManager.instance.MoveRangeLayerID));
            if (unit is Ranged)
            {
                List<Tile> orderRangeTiles = Tile.GetTilesInRange(currentCam.GetPosition(), currentCam.GetOrderRadius());
                orderRangeTiles.ForEach(tile => tile.SetOutline(false, GameManager.instance.TileZoneLayerID));
                (unit as Ranged).GetAttackableTiles(orderRangeTiles).ForEach(tile => tile.SetOutline(true, GameManager.instance.RangedAttackLayerId));
            }


            GameObject secondObject = null;
            while (secondObject == null && !canceled)
            {
                yield return SelectGameObject(
                    go => IsInOrderRange(go) && (IsTile(go) || IsRivalElement(go)),
                    go => secondObject = go
                );
                accessibles.ForEach(tile => tile.SetOutline(false, GameManager.instance.MoveRangeLayerID));

                if (unit is Ranged)
                {
                    Tile.GetTilesInRange(currentCam.GetPosition(), currentCam.GetOrderRadius()).ForEach(tile => tile.SetOutline(true, GameManager.instance.TileZoneLayerID));
                    (unit as Ranged).GetAttackableTiles().ForEach(tile => tile.SetOutline(false, GameManager.instance.RangedAttackLayerId));
                }
            }

            if (secondObject == null) yield break;

            if (IsTile(secondObject))
            {
                //idée potentielle : permettre de ne pas attendre les déplacements
                GameManager.instance.soundManager.PlaySound("UnitMovement");
                StartCoroutine(firstSelection.GetComponent<Unit>().Move(secondObject.GetComponent<Tile>()));
            }
            else if (IsRivalElement(secondObject))
            {
                yield return firstSelection.GetComponent<Unit>().Attack(secondObject.GetComponent<PlaceableObject>());
            }

        }
    }

    bool IsInOrderRange(GameObject go)
    {
        Tile tile = go?.GetComponent<Tile>();
        if (tile != null) return Tile.GetTilesInRange(currentCam.GetPosition(), currentCam.GetOrderRadius()).Contains(tile);

        PlaceableObject placeableObject = go?.GetComponent<PlaceableObject>();
        if (placeableObject == null) return false;
        return Tile.GetTilesInRange(currentCam.GetPosition(), currentCam.GetOrderRadius()).Contains(placeableObject.position);
    }

    bool IsTileRecruitable(GameObject go)
    {
        Tile tile = go?.GetComponent<Tile>();
        if (tile == null) return false;
        return tile.IsAccessible() && Tile.GetTilesInRange(currentCam.GetPosition(), currentCam.GetOrderRadius()).Contains(tile);
    }

    bool IsOwnUnit(GameObject go)
    {
        Unit unit = go?.GetComponent<Unit>();
        if (unit == null) return false;
        return units.Contains(unit);
    }

    bool IsOwnGeneral(GameObject go)
    {
        General general = go?.GetComponent<General>();
        if (general == null) return false;
        return general == this.general;
    }

    bool IsOwnOutpost(GameObject go)
    {
        Outpost officer = go?.GetComponent<Outpost>();
        if (officer == null) return false;
        return outposts.Contains(officer);
    }

    bool IsTile(GameObject go)
    {
        Tile tile = go?.GetComponent<Tile>();
        return tile != null;
    }

    bool IsRivalElement(GameObject go)
    {
        PlaceableObject placeableObject = go?.GetComponent<PlaceableObject>();
        if (placeableObject == null) return false;
        return !GetPlaceableObjects().Contains(placeableObject);
    }

    public void OnTurnEnded()
    {
        ressourceBalance.AddRessources(GameManager.instance.goldGainPerTurn);
    }
}
