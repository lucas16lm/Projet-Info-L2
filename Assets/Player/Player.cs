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
    public List<Outpost> outposts;
    public List<Unit> units;

    public abstract IEnumerator Deployment(Action onComplete);
    public abstract IEnumerator Wait(Action onComplete);
    public abstract IEnumerator PlayTurn(Action onComplete);

    protected List<Tile> GetDeploymentZone(){
        int maxY = (int)(GameManager.instance.mapGenerator.width*GameManager.instance.mapGenerator.heightRatio);
        return playerRole==PlayerRole.FirstPlayer ? Tile.GetTilesBetween(0, 3) : Tile.GetTilesBetween(maxY-4, maxY);
    }

    public List<PlaceableObject> GetPlaceableObjects(){
        List<PlaceableObject> list = new List<PlaceableObject>(){general};
        list.AddRange(outposts);
        list.AddRange(units);
        return list;
    }
}

public enum PlayerRole{
    FirstPlayer,
    SecondPlayer
}
