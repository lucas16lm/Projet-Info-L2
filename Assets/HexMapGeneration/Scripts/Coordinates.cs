using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Coordinates
{

    public static Vector3 OffsetToWorldCoordinates(int x, int y, float tileRadius)
    {
        float hexHeight = 2 * Mathf.Sqrt((3 * Mathf.Pow(tileRadius, 2)) / 4);

        float worldX = 1.5f*tileRadius * x; ;
        float worldZ = hexHeight*y;

        if(x % 2 != 0)
        {
            worldZ += hexHeight / 2;
        }


        return new Vector3(worldX, 0, worldZ);
    }
    public static Vector3 OffsetToWorldCoordinates(Vector2Int offsetCoordinates, float tileRadius)
    {
        return OffsetToWorldCoordinates(offsetCoordinates.x, offsetCoordinates.y, tileRadius);
    }

    public static Vector3Int OffsetToCubeCoordinates(int offsetX, int offsetY)
    {
        int cubeX = offsetX;
        int cubeY = offsetY - (offsetX - (offsetX & 1)) / 2;
        int cubeZ = -cubeX-cubeY;

        return new Vector3Int(cubeX, cubeY, cubeZ);
    }
    public static Vector3Int OffsetToCubeCoordinates(Vector2Int offsetCoordinates)
    {
        return OffsetToCubeCoordinates(offsetCoordinates.x, offsetCoordinates.y);
    }

    public static Vector2Int CubeToOffset(int cubeX, int cubeY, int cubeZ)
    {
        int offsetX = cubeX;
        int offsetY = cubeY + (cubeX - (cubeX & 1)) / 2;

        return new Vector2Int(offsetX, offsetY);
    }
    public static Vector2Int CubeToOffset(Vector3Int cubeCoordinates)
    {
        return CubeToOffset(cubeCoordinates.x, cubeCoordinates.y, cubeCoordinates.z);
    }
    

    
}