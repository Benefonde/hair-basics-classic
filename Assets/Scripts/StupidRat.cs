using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidRat : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "BSODA_Spray(Clone)" || other.transform.name == "Objection(Clone)" || other.transform.name == "Donut(Clone)")
        {
            ps.AddPoints(100, 1);
            gc.audioDevice.PlayOneShot(ratDed);
            FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Rat destroyed*", ratDed.length, Color.cyan, transform);
            Object.Destroy(gameObject);
            Object.Destroy(other.gameObject);
        }
        if (other.transform.name == "Yellow Face")
        {
            gc.SomeoneTied(gameObject);
            gameObject.SetActive(false);
        }
    }

    public PizzaScoreScript ps;
    public GameControllerScript gc;
    public AudioClip ratDed;
}
