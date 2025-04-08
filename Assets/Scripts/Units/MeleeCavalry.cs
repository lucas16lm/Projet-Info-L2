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
            chargeDamage = (int)(UnitData.baseDamagePoints*MeleeCavalryData.chargeBonus*GetPathToElement(target).Count);
            yield return Move(target);            
        }

        if(IsAdjacentTo(target)){
            canAttack=false;
            transform.rotation=Quaternion.LookRotation(target.transform.position-transform.position);
            GetComponent<AnimationManager>().TriggerAnimation("Attack");
            GetComponent<AudioSource>().PlayOneShot(UnitData.attackSound);
            target.DammagedBy(this, CalculateDamage(target, chargeDamage));
            Tween.Delay(0.75f, ()=>UpdateMaterial());
        }
    }

    public override int CalculateDamage(PlaceableObject target)
    {
        int damage = UnitData.baseDamagePoints;
        Biome biome = target.position.biome;
        
        switch(biome){
            case Biome.hill:
                damage=Mathf.RoundToInt(damage*0.50f);
                break;
        }

        switch(target){
            case PikeInfantry:
                damage=Mathf.RoundToInt(damage*0.5f);
                break;
        }

        return damage;
    }
    public int CalculateDamage(PlaceableObject target, int chargeDamage)
    {
        int damage = CalculateDamage(target);
        chargeDamage = damage/UnitData.baseDamagePoints*chargeDamage;

        Debug.Log("Base dammage :"+UnitData.baseDamagePoints+", After bonus : "+ (damage+chargeDamage));
        return damage+chargeDamage;
    }

    public override void DammagedBy(Unit unit, int damagePoints)
    {
        transform.rotation=Quaternion.LookRotation(unit.transform.position-transform.position);
        healthPoints-=damagePoints;
        GetComponentInChildren<LocalCanvas>().UpdateCanvas();
        GetComponent<AudioSource>().PlayOneShot(UnitData.damageSound);
        if(healthPoints<=0){
            Kill();
        }else{
            GetComponent<AnimationManager>().TriggerAnimation("Damage");
            GameObject blood = Instantiate(position.smallBloodParticle, transform.position, Quaternion.identity, transform);
            Tween.Delay(1, ()=>Destroy(blood));
        }
    }


    public override void Kill()
    {
        Instantiate(position.bigBloodParticle, transform.position, Quaternion.identity, transform);
        GetComponent<AudioSource>().PlayOneShot(UnitData.deathSound);
        GetComponent<AnimationManager>().TriggerAnimation("Death");
        position.Content=null;
        transform.parent.GetComponent<Player>().units.Remove(this);
        Tween.Delay(4, ()=>Destroy(gameObject));
    }
}
