using UnityEngine;

public class Troup_Mouvement : MonoBehaviour
{
    public Vector3Int cubicCoordinates;
    public static Tile GetPos(int x, int y, int z)
    {
        return GetPos(new Vector3Int(x, y, z));
    }
}
