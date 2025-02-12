using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Troup_Mouvement : MonoBehaviour
{
    public Vector3Int cubicCoordinate;
    Ray ray;
    RaycastHit hit;

    public Tile ThisTile;

    public Material DefaultColor;
    public Material PossibleMoveColor;

    
    public Vector3Int getPos()
    {
        return cubicCoordinate;
    }

    public void setTile(Tile tile)
    {
        Tile.SetObject(this.gameObject, tile);
        ThisTile = tile;
       

    }


    public void movetoOnTile()
    {
        if (ThisTile != null)
        {
            transform.position = ThisTile.transform.position+ new Vector3(0,1.5f,0);
            ShowPossibleMove();
           
            
         }
    }

    public void ShowPossibleMove()
    {
        foreach (Tile tile in Tile.grid.Values)
        {
            tile.GetComponent<MeshRenderer>().material = DefaultColor;
        }
        List<Tile> tiles = ThisTile.GetNeighbors();
        foreach (Tile tile in tiles)
        {
            if (tile != null)
            {
                tile.GetComponent<MeshRenderer>().material = PossibleMoveColor;

            }
        }
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
                print(hit.collider.name);
        }
    }



}
