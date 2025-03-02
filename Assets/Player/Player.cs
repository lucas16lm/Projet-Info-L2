using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public FactionData factionData;
    public RessourceBalance ressourceBalance;
    public PlayerRole playerRole;

    public General general;
    public List<Officer> officers;
    public List<Unit> units;
    public List<Building> buildings;
    
    public abstract IEnumerator Deployment(Action onComplete);
    public abstract IEnumerator Wait(Action onComplete);
    public abstract IEnumerator PlayTurn(Action onComplete);

    protected List<Tile> GetDeploymentZone(){
        int maxY = (int)(GameManager.instance.mapGenerator.width*GameManager.instance.mapGenerator.heightRatio);
        return playerRole==PlayerRole.FirstPlayer ? Tile.GetTilesBetween(0, 3) : Tile.GetTilesBetween(maxY-4, maxY);
    }

    public List<PlaceableObject> GetPlaceableObject(){
        List<PlaceableObject> list = new List<PlaceableObject>(){general};
        list.AddRange(officers);
        list.AddRange(units);
        list.AddRange(buildings);
        return list;
    }
}

public enum PlayerRole{
    FirstPlayer,
    SecondPlayer
}
