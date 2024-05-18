using UnityEngine;

public class SpriteColorManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        InitializeSpriteRenderer();
    }

    private void InitializeSpriteRenderer()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void OnDestroy()
    {
        CleanupSpriteRenderer();
    }

    private void CleanupSpriteRenderer()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        Color blendedColor = BlendLightingColors();
        ApplyColor(blendedColor);
    }

    private Color BlendLightingColors()
    {
        Color finalColor = Color.clear;
        float totalInfluence = 0f;

        for (int i = 0; i < Lighting.AllLightings.Count; i++)
        {
            float influence = Lighting.AllLightings[i].CalculateInfluence(transform.position);
            if (influence > 0)
            {
                Color lightColor = Lighting.AllLightings[i].GetColorAtPosition(transform.position);
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

        return finalColor;
    }

    private void ApplyColor(Color color)
    {
        if (spriteRenderer != null)
        {
            Color mixedColor = originalColor * color;
            spriteRenderer.color = mixedColor;
        }
    }
}