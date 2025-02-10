using UnityEngine;

public class Troup_Mouvement : MonoBehaviour
{
    public Vector3Int cubicCoordinates;

    public Tile OnTile;
    
    public Vector3Int getPos()
    {
        return cubicCoordinates;
    }

    public void movetoOnTile()
    {
        if (OnTile != null)
        {
            transform.position = OnTile.transform.position+ new Vector3(0,1.5f,0);
        }
    }
    
}
