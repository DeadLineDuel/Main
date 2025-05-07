using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class TransparencyController : NetworkBehaviour
{
    private enum TransparencyState
    {
        Transparent,
        Opaque
    }
    
    [field: SerializeField] private Material[] OpaqueMaterial;
    [field: SerializeField] private Material[] TransparentMaterial;
    
    public void SetToTransparent()
    {
        SetMaterial(TransparencyState.Transparent);
    }

    public void SetToOpaque()
    {
        SetMaterial(TransparencyState.Opaque);
    }
    
    private void SetMaterial(TransparencyState transparencyState)
    {
        Material[] mats;
        switch (transparencyState)
        {
           case TransparencyState.Transparent:
               mats = TransparentMaterial;
               break;
           case TransparencyState.Opaque:
               mats = OpaqueMaterial;
               break;
           default:
               mats = OpaqueMaterial;
               break;
        }
        
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            if (transparencyState == TransparencyState.Transparent)
            {
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }
            Material[] originalMats = renderer.materials;
            for (int i = 0; i < originalMats.Length; i++)
            {
                originalMats[i] = mats[i];
            }
            renderer.materials = originalMats;
        }
    }
}
