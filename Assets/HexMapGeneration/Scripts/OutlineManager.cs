using UnityEngine;
using PrimeTween;
using Linework.SurfaceFill;
using Linework.SoftOutline;
using System.Linq;

public class OutlineManager : MonoBehaviour
{
    public int outlineLayerMaskId;
    public SoftOutlineSettings outlineSettings;


    public void Outline()
    {
        RenderingLayerMask renderingLayerMask = RenderingLayerMask.defaultRenderingLayerMask;
        renderingLayerMask |= 0x1 << outlineLayerMaskId;
        GetComponentInChildren<Renderer>().renderingLayerMask = renderingLayerMask;
        GetComponentsInChildren<Renderer>().ToList().ForEach(renderer => renderer.renderingLayerMask = renderingLayerMask);
    }
    public void DisableOutline()
    {
        GetComponentInChildren<Renderer>().renderingLayerMask=RenderingLayerMask.defaultRenderingLayerMask;
        GetComponentsInChildren<Renderer>().ToList().ForEach(renderer => renderer.renderingLayerMask = RenderingLayerMask.defaultRenderingLayerMask);
    }


}
