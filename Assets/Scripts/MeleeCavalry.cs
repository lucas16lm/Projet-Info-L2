using System.Collections;
using System.Collections.Generic;
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
            target.DammagedBy(this, (target is Unit) ? chargeDamage : 0);
        }
    }

    public override void DammagedBy(Unit unit, int bonusDamage)
    {
        transform.rotation=Quaternion.LookRotation(unit.transform.position-transform.position);
        healthPoints-=(unit.data.baseDamagePoints+bonusDamage);
        GetComponent<AudioSource>().PlayOneShot(data.damageSound);
        if(healthPoints<=0){
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
