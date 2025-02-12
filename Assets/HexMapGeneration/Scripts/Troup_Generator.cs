using UnityEngine;

public class Troup_Generator : MonoBehaviour
{
   public GameObject TroupPrefab;
    
   public void CreateTroupe(int x,int y,int tileRadius)
    {

        Tile tile = Tile.GetTile(Coordinates.OffsetToCubeCoordinates(x, y)); 
        if(!tile.hasTroup())
        {
            GameObject troup = Instantiate(TroupPrefab, Coordinates.OffsetToWorldCoordinates(x, y, tileRadius), Quaternion.identity, transform);
            Troup_Mouvement troup_Mouvement = troup.GetComponent<Troup_Mouvement>();

            troup_Mouvement.setTile(tile);
            troup_Mouvement.setTargets(tile);

            
        }
       
        
       
       

    }
}
