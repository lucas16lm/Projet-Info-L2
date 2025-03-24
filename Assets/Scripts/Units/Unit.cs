using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using UnityEngine;

public abstract class Unit : PlaceableObject, ITurnObserver
{
    public UnitData UnitData{get{return data as UnitData;}}

    public int movementPoints;
    public bool canAttack = false;
    public RessourceBalance cost;
    public float timeToMove;

    public abstract IEnumerator Attack(PlaceableObject target);
    public abstract int CalculateDamage(PlaceableObject target);

    public override void Initialize(PlaceableData placeableData, Tile position, Player player)
    {
        GameManager.instance.turnManager.AddObserver(this);
        data = placeableData;
        player.units.Add(this);
        timeToMove=UnitData.timeToMove;
        healthPoints=data.baseHealthPoints;
        movementPoints=UnitData.baseMovementPoints;
        canAttack=false;
        cost=data.cost;
        gameObject.name=data.elementName;
        this.position=position;
        position.content=this;

        foreach(Renderer renderer in transform.GetChild(0).GetComponentsInChildren<Renderer>()) renderer.material=player.factionData.bannerMaterial;
        for (int i = 1; i < transform.childCount; i++)
        {
            foreach(Renderer renderer in transform.GetChild(i).GetComponentsInChildren<Renderer>()) renderer.material=player.factionData.unitsMaterial;
        }
        InitializeAudioSource();
    }

    public void InitializeAudioSource(){
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend=0.9f;
    }

    public IEnumerator Move(Tile destination)
    {
        yield return Move(AStar(position, destination));
    }

    private IEnumerator Move(List<Tile> path)
    {
        if(path==null || path.Count==0){
            yield break;
        }

        int moveCost=GetMoveCost(path);
        if(moveCost>movementPoints){
            yield break;
        }
        position.content=null;
        path[path.Count-1].content=this;
        movementPoints-=moveCost;

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.loop=true;
        audioSource.clip=UnitData.movementSound;
        audioSource.Play();
        for (int i = 0; i < path.Count; i++)
        {
            gameObject.transform.rotation=Quaternion.LookRotation(path[i].transform.position-position.transform.position);
            GetComponent<AnimationManager>().SetMovementAnimation(true);
            yield return Tween.Position(transform,  path[i].transform.position+(path[i].transform.localScale.y/2)*Vector3.up, timeToMove, Ease.Linear).ToYieldInstruction();
            position=path[i];
            GetComponent<AnimationManager>().SetMovementAnimation(false);
        }
        audioSource.loop=false;
        audioSource.Stop();
    }

    public IEnumerator Move(PlaceableObject target)
    {
        yield return Move(GetPathToElement(target));
    }

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

    protected int GetMoveCost(List<Tile> path){
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

    protected bool IsAdjacentTo(PlaceableObject target){
        return target.position.GetNeighbors().Contains(position);
    }

    protected List<Tile> GetPathToElement(PlaceableObject target){
        //Should be used if the unit is not adjacent to the target !

        List<Tile> possibleDestinations = target.position.GetNeighbors().FindAll(tile=>GetAccessibleTiles().Contains(tile));
        if(possibleDestinations.Count==0){
            //There is no possible path
            return possibleDestinations;
        }
        
        Tile destination = possibleDestinations[0];

        for (int i = 1; i < possibleDestinations.Count; i++)
        {
            if (GetMoveCost(AStar(position, possibleDestinations[i])) < GetMoveCost(AStar(position, destination)))
            {
                destination = possibleDestinations[i];
            }
        }
        return AStar(position, destination);
    }

    public void OnTurnEnded()
    {
        movementPoints=UnitData.baseMovementPoints;
        canAttack=true;
    }
}

