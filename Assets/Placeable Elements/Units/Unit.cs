using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using UnityEngine;

public abstract class Unit : PlaceableObject, ITurnObserver
{
    public UnitData unitData;
    public UnitClass unitClass;
    
    public int damagePoints;
    public int movementPoints;
    public RessourceBalance cost;
    

    public override void Initialize(PlaceableData placeableData, Tile position, Player player)
    {
        GameManager.instance.turnManager.AddObserver(this);
        unitData = (UnitData) placeableData;
        player.units.Add(this);
        
        unitClass=unitData.unitClass;
        healthPoints=unitData.baseHealthPoints;
        movementPoints=unitData.baseMovementPoints;
        cost=unitData.cost;
        gameObject.name=unitData.elementName;
        this.position=position;
        position.content=this;
        Debug.Log("Initialize");
        InitializeHealthBar();
        foreach(Renderer renderer in transform.GetChild(0).GetComponentsInChildren<Renderer>()) renderer.material=player.factionData.bannerMaterial;
        for (int i = 1; i < transform.childCount; i++)
        {
           
            
            foreach(Renderer renderer in transform.GetChild(i).GetComponentsInChildren<Renderer>()) renderer.material=player.factionData.unitsMaterial;
        }
        


    }

    
    
    

    public IEnumerator Move(Tile destination)
    {
        //AStar(position, destination).ForEach(t=>t.gameObject.GetComponent<Renderer>().material.color=Color.red);
        List<Tile> path = AStar(position, destination);
        int moveCost=GetMoveCost(path);
        if(moveCost<=movementPoints){
            yield return Move(path);
            movementPoints-=moveCost;
        }
        
    }

    public IEnumerator Move(List<Tile> path)
    {
        if(path==null){
            yield break;
        }

        for (int i = 0; i < path.Count; i++)
        {
            gameObject.transform.rotation=Quaternion.LookRotation(path[i].transform.position-position.transform.position);
            GetComponent<AnimationManager>().SetMovementAnimation(true);
            float speed = unitClass==UnitClass.Infantry||unitClass==UnitClass.Ranged ? 2 : 1;
            yield return Tween.Position(transform,  path[i].transform.position+(path[i].transform.localScale.y/2)*Vector3.up, speed, Ease.Linear).ToYieldInstruction();
            position.content=null;
            path[i].content=this;
            position=path[i];
            GetComponent<AnimationManager>().SetMovementAnimation(false);
        }
    }

    public abstract void Attack(PlaceableObject target);

    public static List<Tile> AStar(Tile position, Tile destination){
        
        if (position == destination || !destination.IsAccessible()) return null;

        List<Tuple<Tile, int>> openList = new List<Tuple<Tile, int>>() { new Tuple<Tile, int>(position, 0) };
        List<Tuple<Tile, int>> closedList = new List<Tuple<Tile, int>>();
        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
        Dictionary<Tile, int> gScore = new Dictionary<Tile, int>();

        gScore[position] = 0;

        while (openList.Count > 0)
        {
            // Trouver la tuile avec le plus petit F = G + H
            Tuple<Tile, int> currentTuple = openList[0];
            int fCurrent = currentTuple.Item2 + Tile.DistanceBetween(currentTuple.Item1, destination);
            
            foreach (var tuple in openList)
            {
                int f = tuple.Item2 + Tile.DistanceBetween(tuple.Item1, destination);
                if (f < fCurrent)
                {
                    currentTuple = tuple;
                    fCurrent = f;
                }
            }

            Tile current = currentTuple.Item1;

            // Si on atteint la destination, reconstruire le chemin
            if (current == destination)
            {
                return ReconstructPath(cameFrom, current);
            }

            openList.Remove(currentTuple);
            closedList.Add(currentTuple);

            foreach (Tile neighbor in current.GetNeighbors())
            {
                if (closedList.Any(t => t.Item1 == neighbor) || !neighbor.IsAccessible())
                    continue;

                int tentativeGScore = gScore[current] + neighbor.moveCost;

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    gScore[neighbor] = tentativeGScore;
                    cameFrom[neighbor] = current;

                    openList.Add(new Tuple<Tile, int>(neighbor, tentativeGScore));
                }
            }
        }

        return null; // Aucun chemin trouvé
    }   

    // Méthode pour reconstruire le chemin après avoir atteint la destination
    private static List<Tile> ReconstructPath(Dictionary<Tile, Tile> cameFrom, Tile current)
    {
        List<Tile> path = new List<Tile> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        path.Remove(path[0]);
        return path;
    }

    private int GetMoveCost(List<Tile> path){
        return path != null ? path.Sum(tile=>tile.moveCost) : 0;
    }

    public bool CanGoToTile(Tile tile){
        return GetMoveCost(AStar(position, tile))<=movementPoints;
    }

    public List<Tile> GetAccessibleTiles(){
        List<Tile> accessibles = Tile.GetTilesInRange(position, movementPoints).FindAll(tile=>{
            List<Tile> path = AStar(position, tile);
            return path!=null && GetMoveCost(path)<=movementPoints && tile.IsAccessible();
        });
        return accessibles;
    }

    public void OnTurnEnded()
    {
        movementPoints=unitData.baseMovementPoints;
    }
}

public interface ISpecialAction{
    public IEnumerator SpecialAction(GameObject target);
}


