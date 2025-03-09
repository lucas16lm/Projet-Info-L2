using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class Tile : MonoBehaviour, IOutlinable
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

    public static int GetSize(){
        return grid.Count;
    }


    #endregion

    #region static methods related to tiles

    public static float tileRadius;
    public static int DistanceBetween(Tile tile1, Tile tile2){
        return (Mathf.Abs(tile1.cubicCoordinates.x-tile2.cubicCoordinates.x)+Mathf.Abs(tile1.cubicCoordinates.y-tile2.cubicCoordinates.y)+Mathf.Abs(tile1.cubicCoordinates.z-tile2.cubicCoordinates.z))/2;
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

    public static List<Tile> GetTilesBetween(int y1, int y2){
        return Tile.GetTiles().Where(t => Coordinates.CubeToOffset(t.cubicCoordinates).y >= y1 && Coordinates.CubeToOffset(t.cubicCoordinates).y <= y2).ToList();
    }

    public static List<Tile> GetTilesInRange(Tile tile, int range){
        List<Tile> tiles = new List<Tile>();
        for (int x = -range; x <= range; x++)
        {
            for (int y = Mathf.Max(-range, -x-range); y <= Mathf.Min(range, -x+range); y++)
            {
                int z = -x-y;
                Tile inRange = Tile.GetTile(x+tile.cubicCoordinates.x,y+tile.cubicCoordinates.y,z+tile.cubicCoordinates.z);
                if(inRange != null) tiles.Add(inRange);
            }
        }
        return tiles;
    }

    #endregion

    #region tile related attributes and methods
    public GameObject treePrefab;
    public Vector3Int cubicCoordinates;
    public Biome biome;
    public PlaceableObject content;
    public int moveCost {get{
        switch(biome){
            case Biome.plain:
                return 1;
            case Biome.forest:
                return 2;
            case Biome.hill:
                return 3;
            case Biome.mountain:
                return -1;
            case Biome.water:
                return -1;
            default:
                return -1;
        }
    }}

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
        
        Tile up = GetUpNeighbor();
        if(up!=null) neighbors.Add(up);
        Tile down = GetDownNeighbor();
        if(down!=null) neighbors.Add(down);
        Tile upRight = GetUpRightNeighbor();
        if(upRight!=null) neighbors.Add(upRight);
        Tile downRight = GetDownRightNeighbor();
        if(downRight!=null) neighbors.Add(downRight);
        Tile upLeft = GetUpLeftNeighbor();
        if(upLeft!=null) neighbors.Add(upLeft);
        Tile downLeft = GetDownLeftNeighbor();
        if(downLeft!=null) neighbors.Add(downLeft);
        
        return neighbors;
    }

    public bool IsAccessible(){
        if(biome==Biome.mountain || biome==Biome.water) return false;
        return content==null;
    }

    public void AssignBiome(Biome biome){
        this.biome=biome;
    }

    public void ApplyBiome(){
        switch(biome){
            case Biome.plain:
                transform.GetComponent<Renderer>().material.color = new Color(0, 1, 0);
                break;
            case Biome.forest:
                transform.GetComponent<Renderer>().material.color = new Color(0, 0.7f, 0);
                for (int i = 0; i < 6; i++){
                    Vector3 pos = new Vector3(
                        transform.position.x+0.7f*tileRadius*Mathf.Cos((2*Mathf.PI*i)/6),
                        0,
                        transform.position.z+0.7f*tileRadius*Mathf.Sin((2*Mathf.PI*i)/6));
                    Instantiate(treePrefab, pos+transform.localScale.y*Vector3.up, Quaternion.identity, transform.GetChild(0));
                }
                break;
            case Biome.hill:
                transform.GetComponent<Renderer>().materials[0].color = new Color(0.6f, 0.6f, 0);
                transform.GetComponent<Renderer>().materials[1].color = new Color(0.6f, 0.6f, 0);
                transform.localScale+=4*Vector3.up;
                break;
            case Biome.mountain:
                transform.GetComponent<Renderer>().materials[0].color = new Color(0.25f, 0.25f, 0.25f);
                transform.GetComponent<Renderer>().materials[1].color = new Color(0.25f, 0.25f, 0.25f);
                transform.localScale+=10f*Vector3.up;
                break;
            case Biome.water:
                transform.GetComponent<Renderer>().materials[0].color = new Color(0, 0, 0.8f);
                transform.GetComponent<Renderer>().materials[1].color = new Color(0, 0, 0.8f);
                transform.localScale+=2*Vector3.down;
                break;
        }
        
    }

    public void SetOutline(bool value, int renderingLayerMaskId)
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        RenderingLayerMask renderingLayerMask = renderer.renderingLayerMask;
        if(value){
            renderingLayerMask |= 0x1 << renderingLayerMaskId;
        }
        else{
            renderingLayerMask  &= ~(0x1 << renderingLayerMaskId);
        }
        renderer.renderingLayerMask = renderingLayerMask;
    }

    public void DisableOutlines()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        RenderingLayerMask renderingLayerMask = renderer.renderingLayerMask;
        renderingLayerMask  &= ~(0x1 << GameManager.instance.TileZoneLayerID);
        renderingLayerMask  &= ~(0x1 << GameManager.instance.TileSelectLayerID);
        renderer.renderingLayerMask = renderingLayerMask;
    }
    #endregion
}

public enum Biome{
    plain,
    forest,
    hill,
    mountain,
    water
}
