using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region static dictionary and getters
    public static Dictionary<Vector3Int,Tile> grid = new Dictionary<Vector3Int, Tile>();

    public static Tile GetTile(Vector3Int coordinates){
        Tile tile = null;
        grid.TryGetValue(coordinates, out tile);
        return tile;
    }

    public static Tile GetTile(int x, int y, int z){
        return GetTile(new Vector3Int(x,y,z));
    }
    #endregion

    #region tile related attributes and methods
    public Vector3Int cubicCoordinates;
    public TerrainType terrainType;


    void Start(){
        SetTerrainType((TerrainType) Random.Range(0, 5));
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

    public void SetTerrainType(TerrainType terrainType){
        this.terrainType=terrainType;
        switch(terrainType){
            case TerrainType.plain:
                transform.GetComponent<Renderer>().material.color = Color.green;
                break;
            case TerrainType.forest:
                transform.GetComponent<Renderer>().material.color = Color.green;
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
    #endregion
}

public enum TerrainType{
    plain,
    forest,
    hill,
    montain,
    water
}
