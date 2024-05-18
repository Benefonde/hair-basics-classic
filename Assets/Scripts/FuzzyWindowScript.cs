using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyWindowScript : MonoBehaviour
{
    private void Start()
    {
        gameObject.tag = "Interactable";
        mat = gameObject.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (mat.material != windowClean && gc.item[gc.itemSelected] == 23)
        {
            gameObject.tag = "Interactable";
        }
        else
        {
            gameObject.tag = "Untagged";
        }
    }

    public void Clean()
    {
        gc.tc.windowsCleaned++;
        mat.material = windowClean;
        gameObject.layer = 2;
        gameObject.tag = "Untagged";
        if (wallOther.mat.material != wallOther.windowClean)
        {
            wallOther.gameObject.tag = "Untagged";
            wallOther.mat.material = wallOther.windowClean;
            wallOther.gameObject.layer = 2;
        }
        Instantiate(soapParticles, transform.position, soapParticles.transform.rotation);
    }

    public MeshRenderer mat;
    public FuzzyWindowScript wallOther;
    public GameControllerScript gc;

    public GameObject soapParticles;

    public Material windowClean;
}
