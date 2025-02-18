using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FactionData", menuName = "Scriptable Objects/FactionData")]
public class FactionData : ScriptableObject
{
    public string factionName;
    public string description;
    public Sprite factionFlag;
    public Color factionColor;
    public GameObject generalTower;
    public List<UnitData> factionUnits;
    public FactionBonus factionBonus;
}
