using UnityEngine;

[RequireComponent(typeof(AnimationManager), typeof(AudioSource))]
public abstract class PlaceableObject : MonoBehaviour, IOutlinable
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

    public void SetOutline(bool value)
    {
        //TODO adapter couleur
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        RenderingLayerMask renderingLayerMask = renderers[0].renderingLayerMask;
        if(value){
            renderingLayerMask |= 0x1 << 10;
        }
        else{
            renderingLayerMask  &= ~(0x1 << 10);
        }
        
        foreach(Renderer renderer in renderers) renderer.renderingLayerMask = renderingLayerMask;
    }
}
