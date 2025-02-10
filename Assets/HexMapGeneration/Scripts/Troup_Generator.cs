using UnityEngine;

public class Troup_Generator : MonoBehaviour
{
   public GameObject TroupPrefab;
    
   public void CreateTroupe(int x,int y,int tileRadius)
    {
        GameObject troup = Instantiate(TroupPrefab, Coordinates.OffsetToWorldCoordinates(x, y, tileRadius)+ new Vector3(0,0,10), Quaternion.identity, transform);
        troup.transform.position = new Vector3(transform.position.x, 2, transform.position.z);

    }
}
