using System.Collections;
using PrimeTween;
using UnityEngine;

public class MeleeCavalry : Unit
{
    public MeleeCavalryData MeleeCavalryData{get{return data as MeleeCavalryData;}}

    public override IEnumerator Attack(PlaceableObject target)
    {
        int chargeDamage=0;
        if(!canAttack){
            Debug.Log("Attaque déjà effectuée");
            yield break;
        }
        
        if(!IsAdjacentTo(target)){
            GameManager.instance.soundManager.PlaySound("UnitAttack");
            chargeDamage = (int)(data.baseDamagePoints*MeleeCavalryData.chargeBonus*GetPathToElement(target).Count);
            yield return Move(target);            
        }

        if(IsAdjacentTo(target)){
            canAttack=false;
            transform.rotation=Quaternion.LookRotation(target.transform.position-transform.position);
            GetComponent<AnimationManager>().TriggerAnimation("Attack");
            GetComponent<AudioSource>().PlayOneShot(data.attackSound);
            target.DammagedBy(this, CalculateDamage(target, chargeDamage));
        }
    }

    public override int CalculateDamage(PlaceableObject target)
    {
        int damage = data.baseDamagePoints;
        Biome biome = target.position.biome;
        
        switch(biome){
            case Biome.forest:
                damage=Mathf.RoundToInt(damage*0.5f);
                break;
            case Biome.hill:
                damage=Mathf.RoundToInt(damage*0.1f);
                break;
        }

        switch(target){
            case PikeInfantry:
                damage=Mathf.RoundToInt(damage*0.2f);
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

        return damage;
    }
    public int CalculateDamage(PlaceableObject target, int chargeDamage)
    {
        int damage = CalculateDamage(target);
        chargeDamage = damage/data.baseDamagePoints*chargeDamage;

        Debug.Log("Base dammage :"+data.baseDamagePoints+", After bonus : "+ (damage+chargeDamage));
        return damage+chargeDamage;
    }

    public override void DammagedBy(Unit unit, int damagePoints)
    {
        transform.rotation=Quaternion.LookRotation(unit.transform.position-transform.position);
        healthPoints-=damagePoints;
        GetComponent<AudioSource>().PlayOneShot(data.damageSound);
        if(healthPoints<=0){
            unit.transform.parent.GetComponent<Player>().ressourceBalance.AddRessources(cost);
            GameManager.instance.uIManager.UpdateRessourcePanel(unit.transform.parent.GetComponent<Player>());
            GameManager.instance.soundManager.PlaySound("Gold");
            Kill();
        }else{
            GetComponent<AnimationManager>().TriggerAnimation("Damage");
        }
    }


    public override void Kill()
    {
        GetComponent<AudioSource>().PlayOneShot(data.deathSound);
        GetComponent<AnimationManager>().TriggerAnimation("Death");
        position.content=null;
        transform.parent.GetComponent<Player>().units.Remove(this);
        Tween.Delay(4, ()=>Destroy(gameObject));
    }
}
