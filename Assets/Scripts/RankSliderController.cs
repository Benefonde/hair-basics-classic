using UnityEngine;
using UnityEngine.UI;

public class RankSliderController : MonoBehaviour
{
    public Image baseImage; // Reference to the base icon image
    public Image fillImage; // Reference to the fill-up texture image
    public Sprite[] baseTextures; // Array holding different base icon textures (D, C, B, A, S)
    public Sprite[] fillTextures; // Array holding different fill-up textures (D, C, B, A, S)
    public Slider slider; // Reference to the Slider component
    public int sRankScore;
    private float[] rankThresholds;

    public PizzaScoreScript pss;

    private int currentRankIndex = 0; // Current rank index

    private void Start()
    {
        sRankScore = pss.sRankScore;
    }

    public void UpdateRank(float score)
    {
        int maxRankIndex = baseTextures.Length - 1;

        for (int i = 1; i <= maxRankIndex; i++)
        {
            float threshold = sRankScore / i;

            if (score >= threshold - 1)
            {
                currentRankIndex = maxRankIndex - i;
                break;
            }
        }

        baseImage.sprite = baseTextures[currentRankIndex];
        fillImage.sprite = fillTextures[currentRankIndex];

        if (currentRankIndex == maxRankIndex)
        {
            fillImage.fillAmount = 0f;
        }
        else
        {
            float normalizedScore = CalculateNormalizedScore(score, currentRankIndex);
            fillImage.fillAmount = normalizedScore;
        }
    }



    private float CalculateNormalizedScore(float score, int rankIndex)
    {
        if (rankIndex == baseTextures.Length - 1)
        {
            return 1f;
        }

        float threshold = sRankScore / (baseTextures.Length - 1 - rankIndex);
        float nextThreshold = sRankScore / (baseTextures.Length - rankIndex);

        return Mathf.Clamp01((score - threshold) / (nextThreshold - threshold));
    }
}
