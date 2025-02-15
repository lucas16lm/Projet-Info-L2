using UnityEngine;
using PrimeTween;
using Linework.SurfaceFill;

public class OutlineManager : MonoBehaviour
{
    public RenderingLayerMask outlineLayerMask;
    public SurfaceFillSettings outlineSettings;


    void OnEnable()
    {
        GetComponent<Renderer>().renderingLayerMask=outlineLayerMask;
    }
    void OnDisable()
    {
        GetComponent<Renderer>().renderingLayerMask=RenderingLayerMask.defaultRenderingLayerMask;
    }


}
