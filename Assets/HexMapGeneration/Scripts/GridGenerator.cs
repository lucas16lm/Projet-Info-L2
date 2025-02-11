using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int width=15;
    public float heightRatio=1.5f;
    public GameObject tilePrefab;
    public float tileRadius=1f;


    
    void Start(){    
        PlaceTiles(width);
        ClearTiles();
    }

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
                Tile.grid.Add(Coordinates.OffsetToCubeCoordinates(x, y), tileComponent);
                Tile.gridObject.Add(tileComponent, null);
            }
        }
    }

    public void ClearTiles(){
        foreach(Vector3Int coordinate in Tile.grid.Keys){
            Tile tile=null;
            Tile.grid.TryGetValue(coordinate, out tile);
            if(tile != null) Destroy(tile.transform.gameObject);
        }
        Tile.grid.Clear();
    }


}
