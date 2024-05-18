using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player")
        {
            Time.timeScale = 2f;
            FindObjectOfType<GameControllerScript>().originalTimeScale = 2;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Player")
        {
            Time.timeScale = 1f;
            FindObjectOfType<GameControllerScript>().originalTimeScale = 1;
        }
    }
}
