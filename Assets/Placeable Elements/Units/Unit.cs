using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using UnityEngine;

public abstract class Unit : PlaceableObject
{
    public UnitClass unitClass;
    public int damagePoints;
    public int movementPoints;
    public RessourceBalance cost;

    public override void Initialize(PlaceableData placeableData, Tile position, Player player)
    {
        UnitData unitData = (UnitData) placeableData;
        player.units.Add(this);
        unitClass=unitData.unitClass;
        healthPoints=unitData.baseHealthPoints;
        movementPoints=unitData.baseMovementPoints;
        cost=unitData.cost;
        gameObject.name=unitData.elementName;
        this.position=position;
        position.occupied=true;

        foreach(Renderer renderer in transform.GetChild(0).GetComponentsInChildren<Renderer>()) renderer.material=player.factionData.bannerMaterial;
        for (int i = 1; i < transform.childCount; i++)
        {
            foreach(Renderer renderer in transform.GetChild(i).GetComponentsInChildren<Renderer>()) renderer.material=player.factionData.unitsMaterial;
        }
    }

    public IEnumerator Move(Tile destination)
    {
        //AStar(position, destination).ForEach(t=>t.gameObject.GetComponent<Renderer>().material.color=Color.red);
        yield return Move(AStar(position, destination));
    }

    public IEnumerator Move(List<Tile> path)
    {
        if(path==null){
            yield break;
        }

        for (int i = 0; i < path.Count-1; i++)
        {
            gameObject.transform.rotation=Quaternion.LookRotation(path[i+1].transform.position-position.transform.position);
            GetComponent<AnimationManager>().SetMovementAnimation(true);
            float speed = unitClass==UnitClass.Infantry||unitClass==UnitClass.Ranged ? 2 : 1;
            yield return Tween.Position(transform,  path[i+1].transform.position+(path[i+1].transform.localScale.y/2)*Vector3.up, speed, Ease.Linear).ToYieldInstruction();
            position.occupied=false;
            path[i+1].occupied=true;
            position=path[i+1];
            GetComponent<AnimationManager>().SetMovementAnimation(false);
        }
    }

    public abstract void Attack(Unit target);

    public static List<Tile> AStar(Tile position, Tile destination){
        
        if (position == destination || destination.occupied || !destination.occupable) return null;

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
                if (closedList.Any(t => t.Item1 == neighbor) || neighbor.occupied || !neighbor.occupable)
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
        return path;
    }
}

