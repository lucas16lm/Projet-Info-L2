using UnityEngine;

public class TwoHandedInfantry : Infantry
{
    public override int CalculateDamage(PlaceableObject target)
    {
        int damage = UnitData.baseDamagePoints;
        Biome biome = target.position.biome;
        
        switch(biome){
            case Biome.hill:
                damage=Mathf.RoundToInt(damage*0.8f);
                break;
        }

        switch(target){
            case PikeInfantry:
                damage=Mathf.RoundToInt(damage*1.5f);
                break;
            case ShieldInfantry:
                damage=Mathf.RoundToInt(damage*1.5f);
                break;
            case Ranged:
                damage=Mathf.RoundToInt(damage*2f);
                break;
        }

        if(target is Infantry){
            if(target.position.GetNeighbors().FindAll(neighbor=>neighbor.Content is Infantry && target.transform.parent.GetComponent<Player>().units.Contains(neighbor.Content as Infantry)).Count>=2){
                damage=Mathf.RoundToInt(damage*(target as Infantry).InfantryData.adjacenceBonus);
            }
        }
        Debug.Log("Base dammage :"+UnitData.baseDamagePoints+", After bonus : "+ damage);
        return damage;
    }
}
