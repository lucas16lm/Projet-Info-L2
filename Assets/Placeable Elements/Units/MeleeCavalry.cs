using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class MeleeCavalry : Unit
{
    public override IEnumerator Attack(PlaceableObject target)
    {
        int chargeDamage=0;
        if(!canAttack){
            Debug.Log("Attaque déjà effectuée");
            yield break;
        }
        
        if(!IsAdjacentTo(target)){
            chargeDamage = (int)(unitData.baseDamagePoints*0.25*GetPathToElement(target).Count);
            yield return Move(target);            
        }

        if(IsAdjacentTo(target)){
            canAttack=false;
            transform.rotation=Quaternion.LookRotation(target.transform.position-transform.position);
            GetComponent<AnimationManager>().TriggerAnimation("Attack");
            target.DammagedBy(this, (target is Unit) ? chargeDamage : 0);
        }
    }

    public override void DammagedBy(Unit unit, int bonusDamage)
    {
        transform.rotation=Quaternion.LookRotation(unit.transform.position-transform.position);
        GetComponent<AnimationManager>().TriggerAnimation("Damage");
        healthPoints-=(unit.unitData.baseDamagePoints+bonusDamage);
        if(healthPoints<=0) Kill();
    }

    public override void Kill()
    {
        GetComponent<AnimationManager>().TriggerAnimation("Death");
        position.content=null;
        transform.parent.GetComponent<Player>().units.Remove(this);
        Tween.Delay(4, ()=>Destroy(gameObject));
    }
}
