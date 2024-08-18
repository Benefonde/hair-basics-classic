using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt(unlockRequirement) == 0)
        {
            image.SetActive(false);
            charName.text = "???";
            description.text = "You haven't found this character's unlock requirement yet!";
        }
    }

    public GameObject image;
    public TMP_Text charName;
    public TMP_Text description;

    public string unlockRequirement;
}
