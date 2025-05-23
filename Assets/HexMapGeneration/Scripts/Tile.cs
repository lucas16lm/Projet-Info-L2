using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.Mathematics;

public class Tile : MonoBehaviour, IOutlinable
{
    #region static dictionary and getters
    private static Dictionary<Vector3Int, Tile> grid = new Dictionary<Vector3Int, Tile>();

    public static void AddTile(Vector3Int coordinates, Tile tile)
    {
        grid.Add(coordinates, tile);
    }

    public static void AddTile(int x, int y, int z, Tile tile)
    {
        AddTile(new Vector3Int(x, y, z), tile);
    }

    public static Tile GetTile(Vector3Int coordinates)
    {
        Tile tile = null;
        grid.TryGetValue(coordinates, out tile);
        return tile;
    }

    public static Tile GetTile(int x, int y, int z)
    {
        return GetTile(new Vector3Int(x, y, z));
    }

    public static void Clear()
    {
        grid.Clear();
    }

    public static List<Vector3Int> GetCoordinates()
    {
        return new List<Vector3Int>(grid.Keys);
    }

    public static List<Tile> GetTiles()
    {
        return new List<Tile>(grid.Values);
    }

    public static int GetSize()
    {
        return grid.Count;
    }


    #endregion

    #region static methods related to tiles

    public static float tileRadius;
    public static int DistanceBetween(Tile tile1, Tile tile2)
    {
        return (Mathf.Abs(tile1.cubicCoordinates.x - tile2.cubicCoordinates.x) + Mathf.Abs(tile1.cubicCoordinates.y - tile2.cubicCoordinates.y) + Mathf.Abs(tile1.cubicCoordinates.z - tile2.cubicCoordinates.z)) / 2;
    }

    public static Tile FindNearestTile(Tile tile, List<Tile> tiles)
    {
        Tile nearest = tiles[0];
        for (int i = 1; i < tiles.Count; i++)
        {
            if (DistanceBetween(tile, tiles[i]) < DistanceBetween(tile, nearest))
            {
                nearest = tiles[i];
            }
        }
        return nearest;
    }

    public static List<Tile> GetTilesBetween(int y1, int y2)
    {
        return Tile.GetTiles().Where(t => Coordinates.CubeToOffset(t.cubicCoordinates).y >= y1 && Coordinates.CubeToOffset(t.cubicCoordinates).y <= y2).ToList();
    }

    public static List<Tile> GetTilesInRange(Tile tile, int range)
    {
        List<Tile> tiles = new List<Tile>();
        for (int x = -range; x <= range; x++)
        {
            for (int y = Mathf.Max(-range, -x - range); y <= Mathf.Min(range, -x + range); y++)
            {
                int z = -x - y;
                Tile inRange = Tile.GetTile(x + tile.cubicCoordinates.x, y + tile.cubicCoordinates.y, z + tile.cubicCoordinates.z);
                if (inRange != null) tiles.Add(inRange);
            }
        }
        return tiles;
    }

    public static List<Tile> GetLineBetween(Tile start, Tile end)
    {
        List<Tile> line = new List<Tile>() { };
        int distance = DistanceBetween(start, end);
        for (int i = 0; i <= distance; i++)
        {
            Tile tile = GetTile(TileLerp(start, end, (float)i / distance));
            if (tile != null)
            {
                line.Add(tile);
            }

        }
        return line;
    }

    private static Vector3Int TileLerp(Tile start, Tile end, float t)
    {
        float x = Mathf.Lerp(start.cubicCoordinates.x, end.cubicCoordinates.x, t);
        float y = Mathf.Lerp(start.cubicCoordinates.y, end.cubicCoordinates.y, t);
        float z = Mathf.Lerp(start.cubicCoordinates.z, end.cubicCoordinates.z, t);
        return new Vector3Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y), Mathf.RoundToInt(z));
    }



    #endregion

    #region tile related attributes and methods
    [Header("Props")]
    public GameObject forestPrefab;
    public List<GameObject> mountainTops;
    public GameObject plainTop;
    public GameObject hillTop;
    public GameObject border;
    [Header("Materials")]
    public Material waterMaterial;
    public Material plainMaterial;
    public Material hillMaterial;
    [Header("Particles")]
    public GameObject targetParticle;
    public GameObject smallBloodParticle;
    public GameObject bigBloodParticle;

    public Vector3Int cubicCoordinates;
    public Biome biome;
    private PlaceableObject _content;
    public PlaceableObject Content{
        get { return _content; }
        set{
            if (_content != value)
        {
            _content = value;
            if(value != null){
                SetForestTransparent(true);
            }else{
                SetForestTransparent(false);
            }
        }
        }
    }
    public int moveCost
    {
        get
        {
            switch (biome)
            {
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
        }
    }

    public Tile GetUpNeighbor()
    {
        return GetTile(cubicCoordinates.x, cubicCoordinates.y + 1, cubicCoordinates.z - 1);
    }

    public Tile GetDownNeighbor()
    {
        return GetTile(cubicCoordinates.x, cubicCoordinates.y - 1, cubicCoordinates.z + 1);
    }

    public Tile GetUpRightNeighbor()
    {
        return GetTile(cubicCoordinates.x + 1, cubicCoordinates.y, cubicCoordinates.z - 1);
    }

    public Tile GetDownRightNeighbor()
    {
        return GetTile(cubicCoordinates.x + 1, cubicCoordinates.y - 1, cubicCoordinates.z);
    }

    public Tile GetUpLeftNeighbor()
    {
        return GetTile(cubicCoordinates.x - 1, cubicCoordinates.y + 1, cubicCoordinates.z);
    }

    public Tile GetDownLeftNeighbor()
    {
        return GetTile(cubicCoordinates.x - 1, cubicCoordinates.y, cubicCoordinates.z + 1);
    }

    public List<Tile> GetNeighbors()
    {
        List<Tile> neighbors = new List<Tile>();

        Tile up = GetUpNeighbor();
        if (up != null) neighbors.Add(up);
        Tile down = GetDownNeighbor();
        if (down != null) neighbors.Add(down);
        Tile upRight = GetUpRightNeighbor();
        if (upRight != null) neighbors.Add(upRight);
        Tile downRight = GetDownRightNeighbor();
        if (downRight != null) neighbors.Add(downRight);
        Tile upLeft = GetUpLeftNeighbor();
        if (upLeft != null) neighbors.Add(upLeft);
        Tile downLeft = GetDownLeftNeighbor();
        if (downLeft != null) neighbors.Add(downLeft);

        return neighbors;
    }

    public bool IsAccessible()
    {
        if (biome == Biome.mountain || biome == Biome.water || biome == Biome.border) return false;
        return _content == null;
    }

    public void AssignBiome(Biome biome)
    {
        this.biome = biome;
    }

    public void ApplyBiome()
    {
        switch (biome)
        {
            case Biome.plain:
                transform.GetComponent<Renderer>().material = plainMaterial;
                Instantiate(plainTop, transform.GetChild(0).position, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0), transform.GetChild(0));
                break;
            case Biome.forest:
                transform.GetComponent<Renderer>().material = plainMaterial;
                Instantiate(forestPrefab, transform.GetChild(0).position, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0), transform.GetChild(0));
                break;
            case Biome.hill:
                transform.GetComponent<Renderer>().material = hillMaterial;
                transform.localScale += 2 * Vector3.up;
                Instantiate(hillTop, transform.GetChild(0).position, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0), transform.GetChild(0));
                break;
            case Biome.mountain:
                transform.GetComponent<Renderer>().materials[0].color = new Color(0.25f, 0.25f, 0.25f);
                transform.GetComponent<Renderer>().materials[1].color = new Color(0.25f, 0.25f, 0.25f);
                Instantiate(mountainTops[new System.Random().Next(0, mountainTops.Count - 1)], transform.GetChild(0).position, Quaternion.identity, transform.GetChild(0));
                break;
            case Biome.water:
                transform.GetComponent<Renderer>().material = waterMaterial;
                transform.localScale += 2 * Vector3.down;
                break;
            case Biome.border:
                transform.localScale = Tile.tileRadius*Vector3.one;
                transform.GetComponent<Renderer>().material = plainMaterial;
                Instantiate(border, transform.GetChild(0).position, Quaternion.identity, transform.GetChild(0));
                break;
        }



    }

    public void SetForestTransparent(bool state)
    {
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            foreach(Material material in renderer.materials){
                material.SetFloat("_UseTransparency", state ? 1 : 0);
            }
        }

    }

    public void SetOutline(bool value, int renderingLayerMaskId)
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        RenderingLayerMask renderingLayerMask = renderer.renderingLayerMask;
        if (value)
        {
            renderingLayerMask |= 0x1 << renderingLayerMaskId;
        }
        else
        {
            renderingLayerMask &= ~(0x1 << renderingLayerMaskId);
        }
        renderer.renderingLayerMask = renderingLayerMask;
    }

    public void DisableOutlines()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        RenderingLayerMask renderingLayerMask = renderer.renderingLayerMask;
        renderingLayerMask &= ~(0x1 << GameManager.instance.TileZoneLayerID);
        renderingLayerMask &= ~(0x1 << GameManager.instance.TileSelectLayerID);
        renderingLayerMask &= ~(0x1 << GameManager.instance.MoveRangeLayerID);
        renderingLayerMask &= ~(0x1 << GameManager.instance.RangedAttackLayerId);
        renderer.renderingLayerMask = renderingLayerMask;
    }
    #endregion
}

public enum Biome
{
    plain,
    forest,
    hill,
    mountain,
    water,
    border
}
