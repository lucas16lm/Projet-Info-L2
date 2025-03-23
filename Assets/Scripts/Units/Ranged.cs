using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using Unity.Mathematics;
using UnityEngine;

public abstract class Ranged : Unit
{
    public RangedData RangedData { get { return data as RangedData; } }

    public override IEnumerator Attack(PlaceableObject target)
    {
        if (!canAttack)
        {
            Debug.Log("Attaque déjà effectuée");
            yield break;
        }

        if (!GetAttackableTiles().Contains(target.position))
        {
            yield break;
        }

        
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
        for (int i = 1; i <= 4; i++)
        {
            GameObject projectile = Instantiate(RangedData.projectile, transform.GetChild(i).position, quaternion.identity);
            projectile.GetComponent<Projectile>().SetProjectile(target.transform, RangedData.projectileSpeed, RangedData.shootAngle, RangedData.projectilePrecision);
        }

        GameManager.instance.soundManager.PlaySound("UnitAttack");
        GetComponent<AudioSource>().PlayOneShot(UnitData.attackSound);
        canAttack = false;
        
        GetComponent<AnimationManager>().TriggerAnimation("Attack");
        target.DammagedBy(this, CalculateDamage(target));
    }

    public override void DammagedBy(Unit unit, int damagePoints)
    {
        transform.rotation=Quaternion.LookRotation(unit.transform.position-transform.position);
        healthPoints-=damagePoints;
        GetComponent<AudioSource>().PlayOneShot(UnitData.damageSound);
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
        GetComponent<AudioSource>().PlayOneShot(UnitData.deathSound);
        GetComponent<AnimationManager>().TriggerAnimation("Death");
        position.content = null;
        transform.parent.GetComponent<Player>().units.Remove(this);
        Tween.Delay(4, () => Destroy(gameObject));
    }

    public List<Tile> GetAttackableTiles()
    {
        List<Tile> attackables = new List<Tile>();
        foreach (Tile tile in Tile.GetTilesInRange(position, RangedData.attackRange))
        {
            if (!Tile.GetLineBetween(position, tile).Any(tile => tile.biome == Biome.mountain))
            {
                attackables.Add(tile);
            }
        }
        return attackables;
    }

    public List<Tile> GetAttackableTiles(List<Tile> orderRangeTiles)
    {
        return GetAttackableTiles().FindAll(tile => orderRangeTiles.Contains(tile));
    }

}
