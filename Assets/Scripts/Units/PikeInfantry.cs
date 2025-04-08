using UnityEngine;

public class PikeInfantry : Infantry
{
    public override int CalculateDamage(PlaceableObject target)
    {
        int damage = UnitData.baseDamagePoints;
        Biome biome = target.position.biome;
        
        switch(biome){
            case Biome.hill:
                damage=Mathf.RoundToInt(damage*0.75f);
                break;
        }

        switch(target){
            case MeleeCavalry:
                damage=Mathf.RoundToInt(damage*3f);
                break;
        }

        Debug.Log("Base dammage :"+UnitData.baseDamagePoints+", After bonus : "+ damage);
        return damage;
    }
}
