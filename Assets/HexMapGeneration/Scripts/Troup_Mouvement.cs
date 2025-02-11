using UnityEngine;

public class Troup_Mouvement : MonoBehaviour
{
    public Vector3Int cubicCoordinate;


    public Tile ThisTile;

    
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
        }
    }
    

    public void Awake()
    {
       
        
    }

}
