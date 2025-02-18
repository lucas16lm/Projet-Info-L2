using UnityEngine;

public class FactionManager : MonoBehaviour
{
    public GameObject factionPrefab;
    public Faction firstFaction;
    public Faction secondFaction;

    public void InitFactions(FactionData firstFactionData, FactionData secondFactionData){
        firstFaction = Instantiate(factionPrefab, transform).GetComponent<Faction>();
        firstFaction.data = firstFactionData;

        secondFaction = Instantiate(factionPrefab, transform).GetComponent<Faction>();
        secondFaction.data = secondFactionData;
    }
}
