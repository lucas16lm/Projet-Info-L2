using UnityEngine;
using PrimeTween;
using Linework.SurfaceFill;
using Linework.SoftOutline;

public class OutlineManager : MonoBehaviour
{
    public int outlineLayerMaskId;
    public SoftOutlineSettings outlineSettings;


    void OnEnable()
    {
        RenderingLayerMask renderingLayerMask = RenderingLayerMask.defaultRenderingLayerMask;
        renderingLayerMask |= 0x1 << outlineLayerMaskId;
        GetComponent<Renderer>().renderingLayerMask = renderingLayerMask;
    }
    void OnDisable()
    {
        GetComponent<Renderer>().renderingLayerMask=RenderingLayerMask.defaultRenderingLayerMask;
    }


}
