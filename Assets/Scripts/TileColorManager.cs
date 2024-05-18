using UnityEngine;
using System.Collections.Generic;
using TMPro;

#if UNITY_EDITOR
[ExecuteAlways]
#else  
[ExecuteInEditMode]
#endif

public class TileColorManager : MonoBehaviour
{
    public bool alwaysUpdate;

    private void Awake()
    {
        InitializeRenderers();
        InitializeTextMeshPros();
    }

    private void Update()
    {
        foreach (Lighting light in Lighting.AllLightings)
        {
            if (light.alwaysUpdateObjects.Contains(gameObject))
            {
                alwaysUpdate = true;
                break;
            }
        }

        if (Lighting.AllLightings.Count > 0)
        {
            BlendLightingColors();
        }
    }
    private struct RendererMaterials
    {
        public Renderer renderer;
        public Material[] originalMaterials;
        public Color[] originalColors;
        public MaterialPropertyBlock[] blocks;
    }

    private struct TextMeshProColors
    {
        public TMP_Text textMeshPro;
        public Color originalColor;
    }

    private List<RendererMaterials> rendererMaterialsList = new List<RendererMaterials>();
    private List<TextMeshProColors> textMeshProColorsList = new List<TextMeshProColors>();



    private void InitializeTextMeshPros()
    {
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text text in texts)
        {
            TextMeshProColors textMeshProColors;
            textMeshProColors.textMeshPro = text;
            textMeshProColors.originalColor = text.color;
            textMeshProColorsList.Add(textMeshProColors);
        }
    }

    private void InitializeRenderers()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            RendererMaterials rendererMaterials;
            rendererMaterials.renderer = renderer;
            rendererMaterials.originalMaterials = renderer.sharedMaterials;
            rendererMaterials.originalColors = new Color[rendererMaterials.originalMaterials.Length];
            rendererMaterials.blocks = new MaterialPropertyBlock[rendererMaterials.originalMaterials.Length];
            for (int i = 0; i < rendererMaterials.originalMaterials.Length; i++)
            {
                rendererMaterials.originalColors[i] = rendererMaterials.originalMaterials[i].color;
                rendererMaterials.blocks[i] = new MaterialPropertyBlock();
                rendererMaterials.renderer.GetPropertyBlock(rendererMaterials.blocks[i], i);
            }
            rendererMaterialsList.Add(rendererMaterials);
        }
    }

    private void OnDisable()
    {
        CleanupRenderers();
    }

    private void CleanupRenderers()
    {
        foreach (RendererMaterials rendererMaterials in rendererMaterialsList)
        {
            for (int i = 0; i < rendererMaterials.blocks.Length; i++)
            {
                rendererMaterials.renderer.SetPropertyBlock(null, i);
            }
        }
        rendererMaterialsList.Clear();
    }



    private void BlendLightingColors()
    {
        Color finalColor = Color.clear;
        float totalInfluence = 0f;
        for (int i = 0; i < Lighting.AllLightings.Count; i++)
        {
            Lighting currentLighting = Lighting.AllLightings[i];
            if (currentLighting.IsExcluded(gameObject))
                continue;

            float influence = currentLighting.CalculateInfluence(transform.position);
            if (influence > 0)
            {
                Color lightColor = currentLighting.GetColorAtPosition(transform.position);
                finalColor += lightColor * influence;
                totalInfluence += influence;
            }
        }


        if (totalInfluence > 0)
        {
            finalColor /= totalInfluence;
        }
        else
        {
            finalColor = Color.white;
        }

        ApplyColor(finalColor);
    }

    private void ApplyColor(Color color)
    {
        for (int i = 0; i < rendererMaterialsList.Count; i++)
        {
            ApplyColorToMaterials(color, rendererMaterialsList[i]);
        }
        for (int i = 0; i < textMeshProColorsList.Count; i++)
        {
            ApplyColorToTextMeshPro(color, textMeshProColorsList[i]);
        }
    }

    private static void ApplyColorToMaterials(Color color, RendererMaterials rendererMaterials)
    {
        for (int i = 0; i < rendererMaterials.blocks.Length; i++)
        {
            rendererMaterials.blocks[i].SetColor("_Color", rendererMaterials.originalColors[i] * color);
            rendererMaterials.blocks[i].SetFloat("_Cutoff", color.a);

            rendererMaterials.renderer.SetPropertyBlock(rendererMaterials.blocks[i], i);
        }
    }


    private static void ApplyColorToTextMeshPro(Color color, TextMeshProColors textMeshProColors)
    {
        Color mixedColor = textMeshProColors.originalColor * color;
        textMeshProColors.textMeshPro.color = mixedColor;
    }
}
