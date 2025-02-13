using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Tile settings")]
    public GameObject tilePrefab;
    public float tileRadius=1f;
    [Header("Grid size settings")]
    public int width=15;
    public float heightRatio=1.5f;
    [Header("Procedural generation settings")]
    public int seed;
    public int nbBiomes = 1;
    [Header("Biome generation percentages")]
    public int plainPercentage;
    public int forestPercentage;
    public int hillPercentage;
    public int mountainPercentage;
    public int waterPercentage;
    
    private System.Random prng;

    public void PlaceTiles(int width){
        prng = new System.Random(seed);
        ClearTiles();
        Tile.tileRadius = tileRadius;

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
        nbBiomes=Mathf.Clamp(nbBiomes, 1, Tile.GetSize());
        List<Tile> regions = new List<Tile>();
        int nbRegion=0;
        while(nbRegion<nbBiomes){
            Tile region =  Tile.GetTile(Tile.GetCoordinates()[prng.Next(0, Tile.GetSize())]);
            regions.Add(region);
            region.transform.GetComponent<Renderer>().material.color=Color.red;
            AssignBiome(region);
            nbRegion++;
        }
        return regions;
    }

    private void AssignBiome(Tile tile){
        tile.AssignBiome(chooseBiome());
    }

    private TerrainType chooseBiome(){
        List<Tuple<int, TerrainType>> choices = new List<Tuple<int, TerrainType>>{
            new Tuple<int, TerrainType>(plainPercentage, TerrainType.plain),
            new Tuple<int, TerrainType>(forestPercentage, TerrainType.forest),
            new Tuple<int, TerrainType>(hillPercentage, TerrainType.hill),
            new Tuple<int, TerrainType>(mountainPercentage, TerrainType.mountain),
            new Tuple<int, TerrainType>(waterPercentage, TerrainType.water)
        };
        
        List<TerrainType> normalizedChoices = new List<TerrainType>();

        foreach(Tuple<int, TerrainType> choice in choices){
            for (int i = 0; i < choice.Item1; i++)
            {
                normalizedChoices.Add(choice.Item2);
            }
        }
        return normalizedChoices[prng.Next(0, normalizedChoices.Count-1)];
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
