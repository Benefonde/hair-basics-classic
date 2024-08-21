using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoScript : MonoBehaviour
{
    void Start()
    {
        string name = charName.text;
        string desc = description.text;

        for (int i = 0; i < unlockRequirement.Length; i++)
        {
            if (PlayerPrefs.GetInt(unlockRequirement[i]) == unlockReqIntNO[i])
            {
                image.SetActive(false);
                charName.text = "???";
                description.text = "You haven't found this character's unlock requirement yet!";
            }
            if (orInsteadOfAnd && PlayerPrefs.GetInt(unlockRequirement[i]) != unlockReqIntNO[i])
            {
                image.SetActive(true);
                charName.text = name;
                description.text = desc;
                break;
            }
        }
    }

    public GameObject image;
    public TMP_Text charName;
    public TMP_Text description;

    public string[] unlockRequirement;
    public int[] unlockReqIntNO;

    public bool orInsteadOfAnd;
}
