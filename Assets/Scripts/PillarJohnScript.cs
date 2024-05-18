using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarJohnScript : MonoBehaviour
{
    void Update()
    {
        if (Vector3.Distance(gc.playerTransform.position, transform.position) <= 3)
        {
            if (!meatophobia.isPlaying && !played)
            {
                meatophobia.Play();
                played = true;
            }
            meatophobia.UnPause();
        }
        else
        {
            meatophobia.Pause();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && gc.playerScript.guiltType == "running" && gc.playerScript.guilt > 0)
        {
            meatophobia.Stop();
            gc.ItsPizzaTime();
            Destroy(gameObject);
        }
    }

    public GameControllerScript gc;
    public AudioSource meatophobia;

    private bool played;
}
