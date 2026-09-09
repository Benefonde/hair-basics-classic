using UnityEngine;
public class UnpauseAudio : MonoBehaviour
{
    void Update()
    {
        AudioListener.pause = false;
    }
}