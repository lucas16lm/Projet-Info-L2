using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FactionData", menuName = "Scriptable Objects/FactionData")]
public class FactionData : ScriptableObject
{
    public string factionName;
    public string description;
    public Material bannerMaterial;
    public Material unitsMaterial;
    public Color factionColor;
    public RessourceBalance baseBalance;
    public GeneralData generalData;
    public OutpostData outpostData;
    public List<UnitData> factionUnitsData;
}
