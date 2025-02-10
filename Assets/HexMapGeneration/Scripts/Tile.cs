using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public static Dictionary<Vector3Int,Tile> grid = new Dictionary<Vector3Int, Tile>();

    public static Tile GetTile(Vector3Int coordinates){
        Tile tile = null;
        grid.TryGetValue(coordinates, out tile);
        return tile;
    }

    public static Tile GetTile(int x, int y, int z){
        return GetTile(new Vector3Int(x,y,z));
    }



    public Vector3Int cubicCoordinates;

    public GameObject Troup; 

    public Boolean hasTroup=false;

    public void setTroup(GameObject troup)
    {
        Troup=troup;
        hasTroup = true;
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
