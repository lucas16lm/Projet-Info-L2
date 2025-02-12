using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public static Dictionary<Vector3Int,Tile> grid = new Dictionary<Vector3Int, Tile>();
    public static Dictionary<Tile,GameObject> gridObject = new Dictionary<Tile, GameObject>();
    
    public static GameObject GetObject(Tile tile)
    {
        GameObject obj = null;
        gridObject.TryGetValue(tile, out obj );
        return obj;
    }
    public static GameObject GetObject(Vector3Int vector3Int)
    {
        return GetObject(GetTile(vector3Int));
    }
    public static GameObject GetObject(int x,int y , int z)
    {
        return GetObject(GetTile(x,y,z));
    }

    public static void SetObject(GameObject gameObject,Tile tile)
    {
        gridObject[tile]=gameObject;
    }

    public static void RemoveObject(Tile tile)
    {
        gridObject[tile] = null;
    }
    public static void ChangeTile(Tile previousTile,Tile newTile, GameObject gameObject)
    {
        RemoveObject(previousTile);
        SetObject(gameObject,newTile);
    }

    public static Tile GetTile(Vector3Int coordinates){
        Tile tile = null;
        grid.TryGetValue(coordinates, out tile);
        return tile;
    }

    public static Tile GetTile(int x, int y, int z){
        return GetTile(new Vector3Int(x,y,z));
    }

    

    public Vector3Int cubicCoordinates;
    public Vector3 GetWorldPositionToMouvement()
    {
        return transform.position + new Vector3(0,1.5f,0);
    }

    

    public Boolean hasTroup() 
    {
        if (gridObject[this] != null)
        {
            return true;
        }
        return false;
    }
    






    public Tile GetUpNeighbor(){
        return GetTile(cubicCoordinates.x, cubicCoordinates.y+1, cubicCoordinates.z-1);
    }

    public Tile GetDownNeighbor(){
        return GetTile(cubicCoordinates.x, cubicCoordinates.y-1, cubicCoordinates.z+1);
    }

    public Tile GetUpRightNeighbor(){
        return GetTile(cubicCoordinates.x+1, cubicCoordinates.y, cubicCoordinates.z-1);
    }

    public Tile GetDownRightNeighbor(){
        return GetTile(cubicCoordinates.x+1, cubicCoordinates.y-1, cubicCoordinates.z);
    }

    public Tile GetUpLeftNeighbor(){
        return GetTile(cubicCoordinates.x-1, cubicCoordinates.y+1, cubicCoordinates.z);
    }

    public Tile GetDownLeftNeighbor(){
        return GetTile(cubicCoordinates.x-1, cubicCoordinates.y, cubicCoordinates.z+1);
    }

    public List<Tile> GetNeighbors(){
        List<Tile> neighbors = new List<Tile>();
        neighbors.Add(GetUpNeighbor());
        neighbors.Add(GetDownNeighbor());
        neighbors.Add(GetUpRightNeighbor());
        neighbors.Add(GetDownRightNeighbor());
        neighbors.Add(GetUpLeftNeighbor());
        neighbors.Add(GetDownLeftNeighbor());
        return neighbors;
    }
}
