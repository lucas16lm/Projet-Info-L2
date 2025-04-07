using System.Collections;
using PrimeTween;
using UnityEngine;

public abstract class Infantry : Unit
{
    public InfantryData InfantryData{get{return data as InfantryData;}}

    public override IEnumerator Attack(PlaceableObject target)
    {
        if(!canAttack){
            Debug.Log("Attaque déjà effectuée");
            yield break;
        }
        
        if(!IsAdjacentTo(target)){
            GameManager.instance.soundManager.PlaySound("UnitAttack");
            yield return Move(target);            
        }
        

        if(IsAdjacentTo(target)){
            canAttack=false;
            transform.rotation=Quaternion.LookRotation(target.transform.position-transform.position);
            GetComponent<AnimationManager>().TriggerAnimation("Attack");
            GetComponent<AudioSource>().PlayOneShot(UnitData.attackSound);
            target.DammagedBy(this, CalculateDamage(target));
        }
        
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
        }
    }

    public override void Kill()
    {
        GetComponent<AudioSource>().PlayOneShot(UnitData.deathSound);
        GetComponent<AnimationManager>().TriggerAnimation("Death");
        position.Content=null;
        transform.parent.GetComponent<Player>().units.Remove(this);
        Tween.Delay(4, ()=>Destroy(gameObject));
    }
    
}
