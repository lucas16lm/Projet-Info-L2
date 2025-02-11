using System;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int width=15;
    public float heightRatio=1.5f;
    public GameObject tilePrefab;
    public float tileRadius=1f;

    public int seed;
    private System.Random prng;

    public void PlaceTiles(int width){
        prng = new System.Random(seed);
        ClearTiles();

        if(width%2==0) width++;

        int height = (int)(heightRatio*width);
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(x%2!=0 && y==height-1) continue;

                GameObject tileGameObject = Instantiate(tilePrefab, Coordinates.OffsetToWorldCoordinates(x, y, tileRadius), Quaternion.identity, transform);

                Vector3Int cubicCoordinate = Coordinates.OffsetToCubeCoordinates(x, y);
                Tile tileComponent = tileGameObject.GetComponent<Tile>();
                tileComponent.cubicCoordinates = cubicCoordinate;
                Tile.AddTile(Coordinates.OffsetToCubeCoordinates(x, y), tileComponent);
            }
        }
        CreateBiomes();
        ApplyBiome();
    }

    public void CreateBiomes(){
        List<Tile> regions = PlaceRegions();
        
        foreach(Tile tile in Tile.GetTiles()){
            if(regions.Contains(tile)) continue;
            Tile nearest = Tile.FindNearestTile(tile, regions);
            tile.AssignBiome(nearest.terrainType);
        }
    }

    private List<Tile> PlaceRegions(){
        List<Tile> regions = new List<Tile>();
        int nbRegion=0;

        
        while(nbRegion<Tile.GetSize()/15){
            Tile region =  Tile.GetTile(Tile.GetCoordinates()[prng.Next(0, Tile.GetSize())]);
            regions.Add(region);
            region.transform.GetComponent<Renderer>().material.color=Color.red;
            AssignBiome(region);
            nbRegion++;
        }
        return regions;
    }

    private void AssignBiome(Tile tile){
        int rd = prng.Next(0, 100);
        if(rd < 5){
            tile.AssignBiome(TerrainType.water);
        }
        else if(rd < 75){
            tile.AssignBiome(TerrainType.plain);
        }
        else if(rd < 90){
            tile.AssignBiome(TerrainType.forest);
        }
        else if(rd < 95){
            tile.AssignBiome(TerrainType.hill);
        }
        else{
            tile.AssignBiome(TerrainType.montain);
        }
    }

    private void ApplyBiome(){
        foreach(Tile tile in Tile.GetTiles()){
            tile.ApplyBiome();
        }
    }

    

    public void ClearTiles(){
        foreach(Vector3Int coordinate in Tile.GetCoordinates()){
            Tile tile = Tile.GetTile(coordinate);
            if(tile != null) Destroy(tile.transform.gameObject);
        }
        Tile.Clear();
    }


}
