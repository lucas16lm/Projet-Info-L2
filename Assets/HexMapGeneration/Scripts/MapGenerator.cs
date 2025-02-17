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

    public void CreateMap()
    {
        prng = new System.Random(seed);
        ClearTiles();
        CreateGrid();
        CreateBiomes();
        ApplyBiomeSettings();
    }

    private void CreateGrid()
    {
        Tile.tileRadius = tileRadius;
        if (width % 2 == 0) width++;
        int height = (int)(heightRatio * width);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x % 2 != 0 && y == height - 1) continue;

                GameObject tileGameObject = Instantiate(tilePrefab, Coordinates.OffsetToWorldCoordinates(x, y, tileRadius), Quaternion.identity, transform);

                Vector3Int cubicCoordinate = Coordinates.OffsetToCubeCoordinates(x, y);
                Tile tileComponent = tileGameObject.GetComponent<Tile>();
                tileComponent.cubicCoordinates = cubicCoordinate;
                Tile.AddTile(Coordinates.OffsetToCubeCoordinates(x, y), tileComponent);
            }
        }
    }

    private void CreateBiomes(){
        List<Tile> biomeCenters = PlaceBiomeCenters();
        
        foreach(Tile tile in Tile.GetTiles()){
            if(biomeCenters.Contains(tile)) continue;
            Tile nearest = Tile.FindNearestTile(tile, biomeCenters);
            tile.AssignBiome(nearest.biome);
        }
    }

    private List<Tile> PlaceBiomeCenters(){
        nbBiomes=Mathf.Clamp(nbBiomes, 1, Tile.GetSize());
        List<Tile> biomeCenter = new List<Tile>();
        int nbCenters=0;
        while(nbCenters<nbBiomes){
            Tile center =  Tile.GetTile(Tile.GetCoordinates()[prng.Next(0, Tile.GetSize())]);
            biomeCenter.Add(center);
            center.transform.GetComponent<Renderer>().material.color=Color.red;
            center.AssignBiome(chooseBiome());
            nbCenters++;
        }
        return biomeCenter;
    }

    private Biome chooseBiome(){
        List<Tuple<int, Biome>> choices = new List<Tuple<int, Biome>>{
            new Tuple<int, Biome>(plainPercentage, Biome.plain),
            new Tuple<int, Biome>(forestPercentage, Biome.forest),
            new Tuple<int, Biome>(hillPercentage, Biome.hill),
            new Tuple<int, Biome>(mountainPercentage, Biome.mountain),
            new Tuple<int, Biome>(waterPercentage, Biome.water)
        };
        
        List<Biome> normalizedChoices = new List<Biome>();

        foreach(Tuple<int, Biome> choice in choices){
            for (int i = 0; i < choice.Item1; i++)
            {
                normalizedChoices.Add(choice.Item2);
            }
        }
        return normalizedChoices[prng.Next(0, normalizedChoices.Count-1)];
    }

    private void ApplyBiomeSettings(){
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
