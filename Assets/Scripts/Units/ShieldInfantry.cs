using UnityEngine;

public class ShieldInfantry : Infantry
{
    public override int CalculateDamage(PlaceableObject target)
    {
        int damage = UnitData.baseDamagePoints;
        Biome biome = target.position.biome;
        
        switch(biome){
            case Biome.hill:
                damage=Mathf.RoundToInt(damage*0.5f);
                break;
            case Biome.forest:
                damage=Mathf.RoundToInt(damage*0.8f);
                break;
        }

        switch(target){
            case PikeInfantry:
                damage=Mathf.RoundToInt(damage*1.25f);
                break;
            case Ranged:
                damage=Mathf.RoundToInt(damage*1.5f);
                break;
        }

        if(target is Infantry){
            if(target.position.GetNeighbors().FindAll(neighbor=>neighbor.content is Infantry && target.transform.parent.GetComponent<Player>().units.Contains(neighbor.content as Infantry)).Count>=2){
                damage=Mathf.RoundToInt(damage*(target as Infantry).InfantryData.adjacenceBonus);
            }
        }
        Debug.Log("Base dammage :"+UnitData.baseDamagePoints+", After bonus : "+ damage);
        return damage;
    }
}
