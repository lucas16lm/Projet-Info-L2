using UnityEngine;
using UnityEditor;
using System.IO;

public class ImageExporter : MonoBehaviour
{
    public RenderTexture renderTexture;

    [ContextMenu("Exporter png")]
    public void ExportPhoto()
    {
        if(renderTexture==null) return;
        // Convertit la RenderTexture en Texture2D
        byte[] bytes = ToTexture2D(renderTexture).EncodeToPNG();

        // Définir le chemin de sauvegarde
        var dirPath = Application.dataPath + "/Photos";

        // Vérifier si le dossier existe, sinon le créer
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        // Sauvegarder l'image sous un nom unique
        string filePath = dirPath + "/Photo_" + Random.Range(0, 100000) + ".png";
        File.WriteAllBytes(filePath, bytes);

        // Afficher le chemin de sauvegarde dans la console
        Debug.Log("Photo enregistrée : " + filePath);
    }

    private Texture2D ToTexture2D(RenderTexture rTex)
    {
        // Créer une Texture2D avec les dimensions de la RenderTexture
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);

        // Copier les pixels de la RenderTexture à la Texture2D
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null; // Nettoyer l'état actif

        return tex;
    }



}
