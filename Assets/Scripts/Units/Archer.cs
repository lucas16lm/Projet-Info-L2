using System;
using UnityEngine;
public class Archer : Ranged
{
    public override int CalculateDamage(PlaceableObject target)
    {
        int damage = UnitData.baseDamagePoints;

        if(position.biome==Biome.hill){
            damage=Mathf.RoundToInt(damage*1.25f);
        }

        if(target.position.biome == Biome.forest){
            damage=Mathf.RoundToInt(damage*0.75f);
        }

        Debug.Log("Base dammage :"+UnitData.baseDamagePoints+", After bonus : "+ damage);
        return damage;
    }
}
