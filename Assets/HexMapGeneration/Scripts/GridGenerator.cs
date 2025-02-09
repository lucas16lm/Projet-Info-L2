using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int width=15;
    public float heightRatio=1.5f;
    public GameObject tilePrefab;
    public float tileRadius=1f;


    
    void Start(){    
        PlaceTiles(width);
    }

    void PlaceTiles(int width){
        if(width%2==0) width++;

        int height = (int)(1.5f*width);
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(x%2!=0 && y==height-1) continue;

                GameObject tileGameObject = Instantiate(tilePrefab, Coordinates.OffsetToWorldCoordinates(x, y, tileRadius), Quaternion.identity, transform);
                tileGameObject.GetComponent<Tile>().cubicCoordinates = Coordinates.OffsetToCubeCoordinates(x, y);

                tileGameObject.GetComponent<Renderer>().material.color=Random.ColorHSV();
            }
        }
    }
}
