using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OutpostData", menuName = "Scriptable Objects/OutpostData")]
public class OutpostData : PlaceableData
{
    public int turnToBuild;
    public int orderRange;
}
