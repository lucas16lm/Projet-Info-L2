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
        Destroy(gameObject);
    }

    public void SetOutline(bool value, int renderingLayerMaskId)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        RenderingLayerMask renderingLayerMask = renderers[0].renderingLayerMask;
        if(value){
            renderingLayerMask |= 0x1 << renderingLayerMaskId;
        }
        else{
            renderingLayerMask  &= ~(0x1 << renderingLayerMaskId);
        }
        
        foreach(Renderer renderer in renderers) renderer.renderingLayerMask = renderingLayerMask;
    }
    
    public void DisableOutlines(){
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        RenderingLayerMask renderingLayerMask = renderers[0].renderingLayerMask;
        renderingLayerMask  &= ~(0x1 << GameManager.instance.AllyLayerId);
        renderingLayerMask  &= ~(0x1 << GameManager.instance.EnnemyLayerId);
        
        foreach(Renderer renderer in renderers) renderer.renderingLayerMask = renderingLayerMask;
    }
}
