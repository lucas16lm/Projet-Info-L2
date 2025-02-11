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


    public void PlaceTiles(int width){
        ClearTiles();

        if(width%2==0) width++;

        int height = (int)(1.5f*width);
        
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
    }

    public void CreateBiomes(){
        System.Random prng = new System.Random(seed);
        List<Tile> centers = new List<Tile>();
        for (int i = 0; i < 10; i++)
        {
            
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
