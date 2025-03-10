using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ShaderLibE : MonoBehaviour 
{
    public static ShaderLibE instance;
    
    public Dictionary<string, Material> Materials;
    public List<string> MaterialNames;
    public List<Material> MaterialLibs;
    

    public Material GetMaterial(int index)
    {
        return MaterialLibs[index];
    }
    public Material GetMaterial(string name)
    {
        if (MaterialNames.Contains(name))
        {
            return GetMaterial(MaterialNames.IndexOf(name));
        }
        else {
            Debug.Log("Nom de Materiel pas dans la liste");
            return null;
        }

    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


}
