using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region static dictionary and getters
    
    private static Dictionary<Vector3Int,Tile> grid = new Dictionary<Vector3Int, Tile>();
    
    private static Dictionary<Tile,GameObject> gridObject = new Dictionary<Tile, GameObject>();

    public static void AddTile(Vector3Int coordinates, Tile tile){
        grid.Add(coordinates, tile);
    } 
    public static void AddObject(Tile tile, GameObject gameObject){
        gridObject.Add(tile, gameObject);
    }
     
    public static void AddObject(Vector3Int vector, GameObject gameObject){
       AddObject(GetTile(vector), gameObject);
    }
     
    public static void AddObject(int x,int y,int z, GameObject gameObject){
        AddObject(new Vector3Int(x,y,z), gameObject);
    }

    public static void AddTile(int x, int y, int z, Tile tile)
    {
        AddTile(new Vector3Int(x, y, z), tile);
    }
    public static void AddObject(int x, int y, int z, Tile tile)
    {
        AddTile(new Vector3Int(x, y, z), tile);
    }


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

    public static void Clear(){
        grid.Clear();
        gridObject.Clear();
    }

    public static List<Vector3Int> GetCoordinates(){
        return new List<Vector3Int>(grid.Keys);
    }

    public static List<Tile> GetTiles(){
        return new List<Tile>(grid.Values);
    }

    public static int GetSize(){
        return grid.Count;
    }

    

    #endregion

    #region static methods related to tiles

    public static float tileRadius;
    public static float DistanceBetween(Tile tile1, Tile tile2){
        return Vector3.Distance(tile1.transform.position, tile2.transform.position);
    }

    public static Tile FindNearestTile(Tile tile, List<Tile> tiles){
        Tile nearest = tiles[0];
        for (int i = 1; i < tiles.Count; i++)
        {
            if(DistanceBetween(tile, tiles[i])<DistanceBetween(tile, nearest)){
                nearest = tiles[i];
            }
        }
        return nearest;
    }
    #endregion

    #region tile related attributes and methods
    public Vector3Int cubicCoordinates;
    public TerrainType terrainType;

    public GameObject treePrefab;
    public Vector3 GetWorldPositionToMouvement()
    {
        return transform.position + new Vector3(0,1.5f,0);
    }

    

    public Boolean hasTroup() 
    {
        if (GetObject(this) != null)
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

    public void AssignBiome(TerrainType terrainType){
        this.terrainType=terrainType;
    }

    public void ApplyBiome(){
        switch(terrainType){
            case TerrainType.plain:
                transform.GetComponent<Renderer>().material.color = new Color(0, 1, 0);
                break;
            case TerrainType.forest:
                transform.GetComponent<Renderer>().material.color = new Color(0, 0.7f, 0);
                for (int i = 0; i < 6; i++){
                    Vector3 pos = new Vector3(
                        transform.position.x+0.7f*tileRadius*Mathf.Cos((2*Mathf.PI*i)/6),
                        0,
                        transform.position.z+0.7f*tileRadius*Mathf.Sin((2*Mathf.PI*i)/6));
                    Instantiate(treePrefab, pos+transform.localScale.y*Vector3.up, Quaternion.identity, transform.GetChild(0));
                }
                break;
            case TerrainType.hill:
                transform.GetComponent<Renderer>().materials[0].color = new Color(0.6f, 0.6f, 0);
                transform.GetComponent<Renderer>().materials[1].color = new Color(0.6f, 0.6f, 0);
                transform.localScale+=0.8f*Vector3.up;
                break;
            case TerrainType.mountain:
                transform.GetComponent<Renderer>().materials[0].color = new Color(0.25f, 0.25f, 0.25f);
                transform.GetComponent<Renderer>().materials[1].color = new Color(0.25f, 0.25f, 0.25f);
                transform.localScale+=1.5f*Vector3.up;
                break;
            case TerrainType.water:
                transform.GetComponent<Renderer>().materials[0].color = new Color(0, 0, 0.8f);
                transform.GetComponent<Renderer>().materials[1].color = new Color(0, 0, 0.8f);
                transform.localScale+=Vector3.down/2;
                break;
        }
    }
    #endregion
}

public enum TerrainType{
    plain,
    forest,
    hill,
    mountain,
    water
}
