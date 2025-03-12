using System.Collections;
using PrimeTween;
using UnityEngine;

public class Infantry : Unit
{
    public override IEnumerator Attack(PlaceableObject target)
    {
        if(!canAttack){
            Debug.Log("Attaque déjà effectuée");
            yield break;
        }
        
        if(!IsAdjacentTo(target)){
            yield return Move(target);            
        }
        

        if(IsAdjacentTo(target)){
            canAttack=false;
            transform.rotation=Quaternion.LookRotation(target.transform.position-transform.position);
            GetComponent<AnimationManager>().TriggerAnimation("Attack");
            target.DammagedBy(this, 0);
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
