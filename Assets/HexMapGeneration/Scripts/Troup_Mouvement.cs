using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class Troup_Mouvement : MonoBehaviour
{
    public Vector3Int cubicCoordinate;
    Ray ray;
    RaycastHit hit;

    public Tile ThisTile;

    public Material DefaultColor;
    public Material PossibleMoveColor;

    public float speed;
    [SerializeField]
    private List<Tile> targets;

    private List<Tile> neighbors;
    public int MouvementRange;



    void Awake()
    {
        targets = new List<Tile>();
        neighbors = new List<Tile>();
        
    }


    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
                if (hit.collider.tag == "Tile" )
                {
                    
                    Tile tile = hit.collider.GetComponent<Tile>();
                    if (InTileList(neighbors, tile))
                    {
                        setTargets(new List<Tile>() { tile});
                       
                        
                    }
                }
        }

        MoveToTarget();
        
       
        

        
    }



    public void setTargets(Tile tile)
    {
        targets = new List<Tile>() { tile };



    }

    public void setTargets(List<Tile> tiles)
    {
        targets = tiles;
        
        
        
    }
    private void MoveToTarget()
    {
        if (targets.Count>0)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targets[0].GetWorldPositionToMouvement(), step);
            notArrived();
            

        }
    }

    private void notArrived()
    {
        if (Vector3.Distance(transform.position, targets[0].GetWorldPositionToMouvement()) < 0.01f)
        {
            
            
            Tile.ChangeTile(ThisTile, targets[0] , this.gameObject);
            setTile(targets[0]);
            targets.RemoveAt(0);
            ShowPossibleMove(3);

        }
       
    }

    private bool InTileList(List<Tile> tiles,Tile tile)
    {
        foreach(Tile checktile in tiles)
        {
            if (checktile == tile) 
            {
                return true;
            }
        }
        return false;
    }
    public void setTile(Tile tile)
    {
        Tile.SetObject(this.gameObject, tile);
        ThisTile = tile;


    }


   

    public void ShowPossibleMove()
    {
        foreach (Tile tile in Tile.grid.Values)
        {
            tile.GetComponent<MeshRenderer>().material = DefaultColor;
        }
        neighbors = ThisTile.GetNeighbors();
        foreach (Tile tile in neighbors)
        {
            if (tile != null)
            {
                tile.GetComponent<MeshRenderer>().material = PossibleMoveColor;

            }
        }
    }

    public void ShowPossibleMove(int range)
    {
        //TODO: A Optimiser
        foreach (Tile tile in Tile.grid.Values)
        {
            tile.GetComponent<MeshRenderer>().material = DefaultColor;
        }
        neighbors = new List<Tile>();
        List<Tile> tileToExpende = new List<Tile>();
        tileToExpende.Add(ThisTile);
        for (int i = 0; i < range; i++)
        {
            List<Tile> tempTileToExpend = new List<Tile>();
            while (tileToExpende.Count > 0)
            {
                Debug.Log(tileToExpende.Count);
                List<Tile> temp = tileToExpende[0].GetNeighbors();
                foreach (Tile tile in temp)
                {
                    if (tile != null)
                    {
                        if (!InTileList(neighbors, tile) && tile != ThisTile)
                        {
                            neighbors.Add(tile);
                            tempTileToExpend.Add(tile);

                        }
                    }
                }
                tileToExpende.RemoveAt(0);
            }
            tileToExpende = tempTileToExpend;


        }
        foreach (Tile tile in neighbors)
        {
            if (tile != null)
            {
                tile.GetComponent<MeshRenderer>().material = PossibleMoveColor;

            }
        }
    }



}
