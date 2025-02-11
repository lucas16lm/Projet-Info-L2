using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region static dictionary and getters
    private static Dictionary<Vector3Int,Tile> grid = new Dictionary<Vector3Int, Tile>();

    public static void AddTile(Vector3Int coordinates, Tile tile){
        grid.Add(coordinates, tile);
    }

    public static void AddTile(int x, int y, int z, Tile tile){
        AddTile(new Vector3Int(x,y,z), tile);
    }

    public static Tile GetTile(Vector3Int coordinates){
        Tile tile = null;
        grid.TryGetValue(coordinates, out tile);
        return tile;
    }

    public static Tile GetTile(int x, int y, int z){
        return GetTile(new Vector3Int(x,y,z));
    }

    public static void Clear(){
        grid.Clear();
    }

    public static List<Vector3Int> GetCoordinates(){
        return new List<Vector3Int>(grid.Keys);
    }

    public static List<Tile> GetTiles(){
        return new List<Tile>(grid.Values);
    }


    #endregion

    #region tile related attributes and methods
    public Vector3Int cubicCoordinates;
    public TerrainType terrainType;
    public float height;


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

    public void SetTerrainType(TerrainType terrainType){
        this.terrainType=terrainType;
        switch(terrainType){
            case TerrainType.plain:
                transform.GetComponent<Renderer>().material.color = Color.green;
                break;
            case TerrainType.forest:
                transform.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case TerrainType.hill:
                transform.GetComponent<Renderer>().material.color = Color.grey;
                break;
            case TerrainType.montain:
                transform.GetComponent<Renderer>().material.color = Color.black;
                break;
            case TerrainType.water:
                transform.GetComponent<Renderer>().material.color = Color.blue;
                break;
        }
    }

    public void SetTerrainType(float height){
        if(height<0.6f){
            SetTerrainType(TerrainType.plain);
        }
        else if(height<0.8f){
            SetTerrainType(TerrainType.forest);
        }
        else if(height<0.9f){
            SetTerrainType(TerrainType.hill);
        }
        else{
            SetTerrainType(TerrainType.montain);
        }
    }

    public List<Tile> GetNeighbors(){
        List<Tile> neighbors = new List<Tile>
        {
            GetUpNeighbor(),
            GetDownNeighbor(),
            GetUpRightNeighbor(),
            GetDownRightNeighbor(),
            GetUpLeftNeighbor(),
            GetDownLeftNeighbor()
        };
        return neighbors;
    }
    #endregion
}

public enum TerrainType{
    plain,
    forest,
    hill,
    montain,
    water
}
