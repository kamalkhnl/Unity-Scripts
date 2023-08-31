using UnityEngine;
using UnityEditor;

public class MaterialGenerator
{
    [MenuItem("Assets/Create Material")]
    private static void CreateMaterial()
    {
        Texture2D diffuseTexture = null;
        Texture2D metallicTexture = null;
        Texture2D normalTexture = null;
        Texture2D aoTexture = null;

        foreach (Object o in Selection.objects)
        {
            if (o.GetType() != typeof(Texture2D))
            {
                Debug.LogError("This isn't a texture: " + o);
                continue;
            }

            Texture2D selected = o as Texture2D;
            string textureName = selected.name;
            string suffix = textureName.Substring(textureName.LastIndexOf('_') + 1);

            switch (suffix)
            {
                case "D":
                    diffuseTexture = selected;
                    break;
                case "M":
                    metallicTexture = selected;
                    break;
                case "N":
                    normalTexture = selected;
                    break;
                case "AO":
                    aoTexture = selected;
                    break;
                default:
                    Debug.LogWarning("Unrecognized suffix: " + suffix);
                    break;
            }
        }

        Material material = new Material(Shader.Find("Standard"));

        // Set textures to corresponding slots
        material.SetTexture("_MainTex", diffuseTexture);
        material.SetTexture("_MetallicGlossMap", metallicTexture);
        material.SetTexture("_BumpMap", normalTexture);
        material.SetTexture("_OcclusionMap", aoTexture);

        // Create material asset
        string materialName = Selection.objects[0].name.Split('_')[0]; // Assumes all textures share the same prefix
        string newAssetName = "Assets/Materials/" + materialName + "_Mat.mat"; // Modify this path if necessary
        AssetDatabase.CreateAsset(material, newAssetName);
        AssetDatabase.SaveAssets();

        Debug.Log("Done! Created material: " + newAssetName);
    }
}
