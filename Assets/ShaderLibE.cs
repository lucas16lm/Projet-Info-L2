using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ShaderLibE :MonoBehaviour
{
    
    public static Dictionary<string, Material> MaterialLib = new Dictionary<string, Material>();
    public Dictionary<string, Material> Materials;
    public List<string> MaterialNames;
    public List<Material> MaterialLibs;
    public void Awake()
    {
        if (MaterialLibs.Count != MaterialNames.Count)
        {
            Debug.Log("Shader Lib Erreur nombre , material et nom materiel");

        }
        for (int i = 0; i < Math.Min(MaterialLibs.Count, MaterialNames.Count); i++)
        {
            MaterialLib.Add(MaterialNames[i], MaterialLibs[i]);
        }
    }


}
