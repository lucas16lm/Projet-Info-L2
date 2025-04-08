using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AnimationManager), typeof(AudioSource))]
public abstract class PlaceableObject : MonoBehaviour, IOutlinable
{
    public GameObject localCanvaGameObject;
    public PlaceableData data;
    public int healthPoints;
    public Tile position;

    public abstract void Initialize(PlaceableData placeableData, Tile position, Player player);
    public abstract void DammagedBy(Unit unit, int damagePoints);
    public abstract void Kill();

    public static void Instantiate(PlaceableData placeableData, Tile tile, Player player){
        if(!tile.IsAccessible() || (placeableData is UnitData && !player.ressourceBalance.RemoveRessources((placeableData as UnitData).cost))){
            GameManager.instance.soundManager.PlaySound("Cancel");
            return;
        }

        GameManager.instance.soundManager.PlaySound("UnitPlacement");
        GameManager.instance.uIManager.UpdateRessourcePanel(player);
        
        GameObject placeableGameObject = Instantiate(placeableData.gameObjectPrefab, tile.gameObject.transform.position+(tile.transform.localScale.y/2)*Vector3.up, Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)), player.transform);
        placeableGameObject.GetComponent<PlaceableObject>().Initialize(placeableData, tile, player);
        LocalCanvas.CreateLocalCanvas(placeableGameObject.GetComponent<PlaceableObject>().localCanvaGameObject, placeableGameObject.transform);

        AudioSource audioSource = placeableGameObject.GetComponent<AudioSource>();
        audioSource.spatialBlend = 0.9f;
    }

    public int GetCurrentHealth()
    {
        return healthPoints;
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
