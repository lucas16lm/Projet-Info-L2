using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class Unit : MonoBehaviour, IDamageable
{
    public UnitData data;
    public int healthPoints;
    public Tile position;

    void Start()
    {
        healthPoints=data.baseHealthPoints;
    }


    public void Move(Tile destination)
    {
        //TODO : la c'est qu'une demo factice
        position.isFree=true;
        destination.isFree=false;
        Tween.Position(gameObject.transform, destination.transform.position+(destination.transform.localScale.y/2)*Vector3.up, 2.5f , Ease.Linear);
        
    }

    public void Move(List<Tile> path)
    {
        //TODO
    }

    public void Attack(Unit target)
    {
        //TODO
    }

    public void SpecialAction(){
        //TODO
    }

    public int GetCurrentHealth()
    {
        return healthPoints;
    }

    public void ApplyDamage(int amount)
    {
        healthPoints-=amount;
        if(healthPoints<=0) Kill();
    }

    public void Kill()
    {
        position.isFree=true;
        GameManager.instance.factionManager.firstFaction.units.Remove(this);
        GameManager.instance.factionManager.secondFaction.units.Remove(this);
        Destroy(gameObject);
    }
}

