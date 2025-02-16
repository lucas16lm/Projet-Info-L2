using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    bool refreshCaseMouvement = true;



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
                        setTargets(FindPath(ThisTile,tile,MouvementRange));
                       
                        
                    }
                }
        }

        MoveToTarget();
        
       
        

        
    }



    public void setTargets(Tile tile)
    {
        targets = new List<Tile>() { tile };

        foreach (Tile tile2 in Tile.GetTiles())
        {
            tile2.GetComponent<OutlineManager>().enabled = false;
        }

    }

    public void setTargets(List<Tile> tiles)
    {
        targets = tiles;
        foreach (Tile tile in Tile.GetTiles())
        {
            tile.GetComponent<OutlineManager>().enabled = false;
        }



    }
    private void MoveToTarget()
    {
        if (targets.Count>0)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targets[0].GetWorldPositionToMouvement(), step);
            notArrived();
            refreshCaseMouvement = true;

        }
        else
        {
            if (refreshCaseMouvement)
            {
                ShowPossibleMove(MouvementRange);
                refreshCaseMouvement = false;
            }
        }
    }

    private void notArrived()
    {
        if (Vector3.Distance(transform.position, targets[0].GetWorldPositionToMouvement()) < 0.01f)
        {
            
            
            Tile.ChangeTile(ThisTile, targets[0] , this.gameObject);
            setTile(targets[0]);
            targets.RemoveAt(0);
            

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
            neighbors = new List<Tile>();
            
            foreach(Tile tile in Tile.GetWalkableTilesInRange(ThisTile, 1))
            {
                tile.GetComponent<OutlineManager>().enabled = true;
                neighbors.Add(tile);
            }
        
    }
    public void ShowPossibleMove(int range)
    {
        neighbors = new List<Tile>();


        //foreach (Tile tile in Tile.GetWalkableTilesInRange(ThisTile, range))
        //{
        //    tile.GetComponent<OutlineManager>().enabled = true;
        //  neighbors.Add(tile);
        //}
        Tile start = ThisTile;
        Dictionary<Tile, float> costMap = new Dictionary<Tile, float>(); // Distance depuis le départ
        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>(); // Pour reconstruire le chemin
        List<Tile> unvisited = new List<Tile>(); // Liste des tuiles non visitées

        // Initialisation
        costMap[ThisTile] = 0;
        unvisited.Add(start);

        while (unvisited.Count > 0)
        {
            // Trier pour prendre la tuile avec le coût le plus faible
            unvisited.Sort((a, b) => costMap[a].CompareTo(costMap[b]));
            Tile current = unvisited[0];
            unvisited.RemoveAt(0);

            // Si on a atteint la destination, on reconstruit le chemin
            

            // Explorer les voisins
            foreach (Tile neighbor in current.GetWalkableNeighbors())
            {

                if (neighbor == null) continue;
                Debug.Log(neighbor.name);
                float newCost = 1;
                if (costMap.ContainsKey(current))
                {
                    newCost = costMap[current] + 1;
                }


                if (!costMap.ContainsKey(neighbor) || (newCost < costMap[neighbor]))
                {
                    costMap[neighbor] = newCost;
                    
                    if (!unvisited.Contains(neighbor))
                        unvisited.Add(neighbor);
                }

            }
            
        }
        foreach (Tile tile in costMap.Keys)
        {
            if (costMap[tile] >0 && costMap[tile]<=range) 
            {
                neighbors.Add(tile);
                tile.GetComponent<OutlineManager>().enabled = true;

            }
        }
            Debug.Log("Pas de Chemin Trouvé");
        // Aucun chemin trouvé

    }

    public static List<Tile> FindPath(Tile start, Tile goal,int mouvementRange)
    {
        Dictionary<Tile, float> costMap = new Dictionary<Tile, float>(); // Distance depuis le départ
        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>(); // Pour reconstruire le chemin
        List<Tile> unvisited = new List<Tile>(); // Liste des tuiles non visitées

        // Initialisation
        costMap[start] = 0;
        unvisited.Add(start);

        while (unvisited.Count > 0)
        {
            // Trier pour prendre la tuile avec le coût le plus faible
            unvisited.Sort((a, b) => costMap[a].CompareTo(costMap[b]));
            Tile current = unvisited[0];
            unvisited.RemoveAt(0);

            // Si on a atteint la destination, on reconstruit le chemin
            if (current == goal)
                return ReconstructPath(cameFrom, goal);

            // Explorer les voisins
            foreach (Tile neighbor in current.GetWalkableNeighbors())
            {
             
                if( neighbor==null) continue;
                Debug.Log(neighbor.name);
                float newCost = 1;
                if (costMap.ContainsKey(current)) {
                    newCost = costMap[current] + 1; 
                }
                
                
                if (!costMap.ContainsKey(neighbor) || (newCost < costMap[neighbor]&&newCost<mouvementRange))
                {
                    costMap[neighbor] = newCost;
                    cameFrom[neighbor] = current;
                    if (!unvisited.Contains(neighbor))
                        unvisited.Add(neighbor);
                }
            }
        }
        Debug.Log("Pas de Chemin Trouvé");
        return null; // Aucun chemin trouvé
    }

    private static List<Tile> ReconstructPath(Dictionary<Tile, Tile> cameFrom, Tile goal)
    {
        List<Tile> path = new List<Tile>();
        Tile current = goal;

        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Reverse(); // On retourne la liste pour avoir le chemin du début à la fin
        return path;
    }



}
