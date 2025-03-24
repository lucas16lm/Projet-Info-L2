using UnityEngine;

public class PikeInfantry : Infantry
{
    public override int CalculateDamage(PlaceableObject target)
    {
        int damage = UnitData.baseDamagePoints;
        Biome biome = target.position.biome;
        
        switch(biome){
            case Biome.hill:
                damage=Mathf.RoundToInt(damage*0.3f);
                break;
            case Biome.forest:
                damage=Mathf.RoundToInt(damage*0.8f);
                break;
        }

        switch(target){
            case MeleeCavalry:
                damage=Mathf.RoundToInt(damage*2f);
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
