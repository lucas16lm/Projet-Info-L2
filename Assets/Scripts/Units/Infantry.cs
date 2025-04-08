using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        List<Infantry> allies = position.GetNeighbors().Where(tile=>tile.Content!=null && tile.Content is Infantry && transform.parent.GetComponent<Player>().units.Contains(tile.Content)).Select(tile=>tile.Content as Infantry).ToList();
        if(allies.Count>=2){
            Debug.Log(allies.Count+" adjacent allies : -25% damage");
            damagePoints=Mathf.RoundToInt(damagePoints*0.75f);
        }

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
