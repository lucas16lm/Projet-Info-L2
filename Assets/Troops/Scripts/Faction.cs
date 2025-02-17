using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Faction", menuName = "Scriptable Objects/Faction")]
public class Faction : ScriptableObject
{
    public string factionName;
    public string description;
    public Sprite factionFlag;
    public List<Unit> factionUnits;
    public FactionBonus factionBonus;
}
