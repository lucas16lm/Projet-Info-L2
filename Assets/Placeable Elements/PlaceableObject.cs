using UnityEngine;

[RequireComponent(typeof(AnimationManager), typeof(AudioSource))]
public abstract class PlaceableObject : MonoBehaviour, IOutlinable
{
    public int healthPoints;
    public Tile position;

    public abstract void Initialize(PlaceableData placeableData, Tile position, Player player);

    public static void Instantiate(PlaceableData placeableData, Tile tile, Player player){
        if(!tile.IsAccessible()) return;
        if(!player.ressourceBalance.RemoveRessources(placeableData.cost)) return;
        GameManager.instance.uIManager.UpdateRessourcePanel(player);
        
        GameObject placeableGameObject = Instantiate(placeableData.gameObjectPrefab, tile.gameObject.transform.position+(tile.transform.localScale.y/2)*Vector3.up, Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)), player.transform);
        placeableGameObject.GetComponent<PlaceableObject>().Initialize(placeableData, tile, player);
    }

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
        position.content=null;
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
