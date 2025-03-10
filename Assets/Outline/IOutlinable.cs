using UnityEngine;

public interface IOutlinable
{
    void SetOutline(bool value, int renderingLayerMaskId);
    public void DisableOutlines();
}
