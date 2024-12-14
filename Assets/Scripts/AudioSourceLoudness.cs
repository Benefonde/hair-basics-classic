using UnityEngine;

public class AudioSourceLoudness : MonoBehaviour
{
    public AudioSource audioSource;
    public int sampleSize = 1024;
    public float rmsValue;
    public float dbValue;

    private float[] samples;

    void Start()
    {
        samples = new float[sampleSize];
    }

    void Update()
    {
        // Gets the audio sample data of the currently played audio clip
        audioSource.GetOutputData(samples, 0);

        // Calculate the RMS value of audio sample data
        float sum = 0;
        for (int i = 0; i < sampleSize; i++)
        {
            sum += samples[i] * samples[i];
        }
        rmsValue = Mathf.Sqrt(sum / sampleSize);

        // Convert RMS to db Value
        dbValue = 20 * Mathf.Log10(rmsValue / 0.1f);
    }
}