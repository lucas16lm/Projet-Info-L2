using UnityEngine;

[RequireComponent(typeof(OutlineManager), typeof(AnimationManager), typeof(AudioSource))]
public abstract class PlaceableObject : MonoBehaviour
{
    public int healthPoints;
    public Tile position;

    public abstract void Initialize(PlaceableData placeableData, Tile position);

    public void ApplyDamage(int amount)
    {
        healthPoints-=amount;
        if(healthPoints<=0) Kill();
    }

    public int GetCurrentHealth()
    {
        return healthPoints;
    }

    public void Kill()
    {
        position.occupied=false;
        //GameManager.instance.factionManager.firstFaction.units.Remove(this);
        //GameManager.instance.factionManager.secondFaction.units.Remove(this);
        Destroy(gameObject);
    }
}
