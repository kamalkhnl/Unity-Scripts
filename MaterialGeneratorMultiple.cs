using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MaterialGeneratorMultiple
{
    [MenuItem("Assets/Create Materials Multiple ")]
    private static void CreateMaterials()
    {
        Dictionary<string, List<Texture2D>> textureSets = new Dictionary<string, List<Texture2D>>();

        foreach (Object o in Selection.objects)
        {
            if (o.GetType() != typeof(Texture2D))
            {
                Debug.LogError("This isn't a texture: " + o);
                continue;
            }

            Texture2D selected = o as Texture2D;
            string textureName = selected.name;
            string prefix = textureName.Substring(0, textureName.LastIndexOf('_'));

            if (!textureSets.ContainsKey(prefix))
            {
                textureSets[prefix] = new List<Texture2D>();
            }

            textureSets[prefix].Add(selected);
        }

        foreach (var textureSet in textureSets)
        {
            string prefix = textureSet.Key;
            List<Texture2D> textures = textureSet.Value;

            Material material = new Material(Shader.Find("Standard"));

            foreach (Texture2D texture in textures)
            {
                string suffix = texture.name.Substring(texture.name.LastIndexOf('_') + 1);

                switch (suffix)
                {
                    case "D":
                        material.SetTexture("_MainTex", texture);
                        break;
                    case "M":
                        material.SetTexture("_MetallicGlossMap", texture);
                        break;
                    case "N":
                        material.SetTexture("_BumpMap", texture);
                        break;
                    case "AO":
                        material.SetTexture("_OcclusionMap", texture);
                        break;
                    default:
                        Debug.LogWarning("Unrecognized suffix: " + suffix);
                        break;
                }
            }

            // Create material asset
            string newAssetName = "Assets/Materials/" + prefix + "_Mat.mat"; // Modify this path if necessary
            AssetDatabase.CreateAsset(material, newAssetName);
            AssetDatabase.SaveAssets();

            Debug.Log("Done! Created material: " + newAssetName);
        }
    }
}
