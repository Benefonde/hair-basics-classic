using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModesManager : MonoBehaviour
{
    void Start()
    {
        randomnessHorray = Random.Range(1, 50);
        if (PlayerPrefs.GetInt("speedyBeat", 0) == 1)
        {
            speedBoost.transform.Find("Lock").gameObject.SetActive(false);
            if (PlayerPrefs.GetInt("speedBoost", 0) == 1)
            {
                speedBoost.isOn = true;
            }
            else
            {
                speedBoost.isOn = false;
            }
        }
        if (PlayerPrefs.GetInt("tripleBeat", 0) == 1)
        {
            staminaBoost.transform.Find("Lock").gameObject.SetActive(false);
            if (PlayerPrefs.GetInt("extraStamina", 0) == 1)
            {
                staminaBoost.isOn = true;
            }
            else
            {
                staminaBoost.isOn = false;
            }
        }
        if (PlayerPrefs.GetInt("pSecretFound", 0) == 1)
        {
            krillerSlow.transform.Find("Lock").gameObject.SetActive(false);
            if (PlayerPrefs.GetInt("slowerKriller", 0) == 1)
            {
                krillerSlow.isOn = true;
            }
            else
            {
                krillerSlow.isOn = false;
            }
        }
        if (PlayerPrefs.GetInt("stealthyBeat", 0) == 1)
        {
            walkThrough.transform.Find("Lock").gameObject.SetActive(false);
            if (PlayerPrefs.GetInt("walkThrough", 0) == 1)
            {
                walkThrough.isOn = true;
            }
            else
            {
                walkThrough.isOn = false;
            }
        }
        if (PlayerPrefs.GetInt("mikoBeat", 0) == 1)
        {
            blockPath.transform.Find("Lock").gameObject.SetActive(false);
            if (PlayerPrefs.GetInt("blockPath", 0) == 1)
            {
                blockPath.isOn = true;
            }
            else
            {
                blockPath.isOn = false;
            }
        }
        if (PlayerPrefs.GetInt("jammerUnlocked", 0) == 1)
        {
            jammer.transform.Find("Lock").gameObject.SetActive(false);
            if (PlayerPrefs.GetInt("jammer", 0) == 1)
            {
                jammer.isOn = true;
            }
            else
            {
                jammer.isOn = false;
            }
        }
        if (PlayerPrefs.GetInt("algerBeat", 0) == 1)
        {
            infItem.transform.Find("Lock").gameObject.SetActive(false);
            if (PlayerPrefs.GetInt("infItem", 0) == 1)
            {
                infItem.isOn = true;
            }
            else
            {
                infItem.isOn = false;
            }
        }
    }

    void Update()
    {
        ModeCheck();
        ModifierCheck();

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(code[index]))
            {
                index++;
                if (index >= code.Length)
                {
                    TriggerEasterEgg();
                    index = 0;
                }
            }
            else
            {
                index = 0;
            }
        }
    }

    void TriggerEasterEgg()
    {
        TestMode = !TestMode;
        switch (TestMode)
        {
            case true: globalAudio.PlayOneShot(cheatOn); break;
            case false: globalAudio.PlayOneShot(cheatOff); break;
        }
    }

    void ModeCheck()
    {
        if (PlayerPrefs.GetInt("mikoUnlocked") == 1 || TestMode)
        {
            mikoSprite.color = Color.white;
            mikoMode.interactable = true;
            if (randomnessHorray != 5)
            {
                mikoText.text = "Miko Mode                                                     Survive one of the creator's friends, miko0087 in this NULL style-like gamemode!";
            }
            else
            {
                mikoText.text = "mikeroqjn Mode                                                        Survive one of the creator's friends, mikeroqjn0087Arc in this NULL style-like gamemode!";
            }
        }
        else
        {
            mikoSprite.color = Color.black;
            mikoMode.interactable = false;
            mikoText.text = "You need to find a secret to unlock this mode!";
        }
        if (PlayerPrefs.GetInt("speedyUnlocked") == 1 || TestMode)
        {
            storyStar.sprite = beat;
            speedySprite.color = Color.white;
            speedyMode.interactable = true;
            speedyText.text = "Speedy Mode                                                                       Oh noes! Panino has challenged you! Both of you are super fast and nobody else is here! Can you beat it?";
        }
        else
        {
            storyStar.sprite = notbeat;
            speedySprite.color = Color.black;
            speedyMode.interactable = false;
            speedyText.text = "You need to beat Story Mode to unlock this mode!";
        }

        if (PlayerPrefs.GetInt("speedyBeat") == 1)
        {
            speedyStar.sprite = beat;
        }
        else
        {
            speedyStar.sprite = notbeat;
        }
        if (PlayerPrefs.GetInt("mikoBeat") == 1)
        {
            mikoStar.sprite = beat;
        }
        else
        {
            mikoStar.sprite = notbeat;
        }
        if (PlayerPrefs.GetInt("mikoBeat") == 1 || TestMode)
        {
            tripleSprite.color = Color.white;
            tripleMode.interactable = true;
            tripleText.text = "Triple Mode                                                          Oh dear! Miko, Alger and Panino all teamed up to catch you! Can you get all 17 Dwaynes avoiding them  or will you get caught?";
        }
        else
        {
            tripleSprite.color = Color.black;
            tripleMode.interactable = false;
            if (randomnessHorray == 5)
            {
                tripleText.text = "You need to beat mi?e????n Mode to unlock this mode!";
            }
            else
            {
                tripleText.text = "You need to beat M??? Mode to unlock this mode!";
            }
        }

        if (PlayerPrefs.GetInt("tripleBeat") == 1)
        {
            tripleStar.sprite = beat;
        }
        else
        {
            tripleStar.sprite = notbeat;
        }
        if (PlayerPrefs.GetInt("pizzaBeat") == 1)
        {
            pizzaStar.sprite = beat;
        }
        else
        {
            pizzaStar.sprite = notbeat;
        }
        if (PlayerPrefs.GetInt("stealthyBeat") == 1 && PlayerPrefs.GetInt("classicBeat", 0) == 1 || TestMode)
        {
            algerSprite.color = Color.white;
            algerMode.interactable = true;
            algerText.text = "Alger Mode                                           Yes, it is literally just NULL Style. Sorry, but it's fun coding something like this!";
        }
        else
        {
            algerSprite.color = Color.black;
            algerMode.interactable = false;
            algerText.text = "You need to beat EVERY MODE [except Free and Endless mode] to unlock this mode!";
        }
        if (PlayerPrefs.GetInt("algerBeat") == 1)
        {
            algerStar.sprite = beat;
        }
        else
        {
            algerStar.sprite = notbeat;
        }
        if (PlayerPrefs.GetInt("speedyBeat") == 1 || TestMode)
        {
            stealthySprite.color = Color.white;
            stealthyMode.interactable = true;
            stealthyText.text = "Stealthy Mode                                                                      Oh no! Plenty of Algers have decided to catch anyone they find that snuck in the ball after hours! Can you hide from them with all the <color=blue>Dirty Chalk Erasers</color>?";
        }
        else
        {
            stealthySprite.color = Color.black;
            stealthyMode.interactable = false;
            stealthyText.text = "You need to beat Sp???? Mode to unlock this mode!";
        }
        if (PlayerPrefs.GetInt("stealthyBeat") == 1)
        {
            stealthyStar.sprite = beat;
        }
        else
        {
            stealthyStar.sprite = notbeat;
        }
        if (PlayerPrefs.GetInt("classicBeat") == 1)
        {
            classicStar.sprite = beat;
        }
        else
        {
            classicStar.sprite = notbeat;
        }
    }

    void ModifierCheck()
    {
        int speede = PlayerPrefs.GetInt("speedyBeat", 0);
        int stamin = PlayerPrefs.GetInt("tripleBeat", 0);
        int krille = PlayerPrefs.GetInt("zombieBeat", 0);
        int walk =  PlayerPrefs.GetInt("stealthyBeat", 0);
        int block = PlayerPrefs.GetInt("chaosBeat", 0);
        int jam = PlayerPrefs.GetInt("jammerUnlocked", 0);
        int item = PlayerPrefs.GetInt("algerBeat", 0);

        if (speede == 1)
        {
            speedBoost.interactable = true;
            if (speedBoost.isOn)
            {
                PlayerPrefs.SetInt("speedBoost", 1);
            }
            else
            {
                PlayerPrefs.SetInt("speedBoost", 0);
            }
        }
        if (stamin == 1)
        {
            staminaBoost.interactable = true;
            if (staminaBoost.isOn)
            {
                PlayerPrefs.SetInt("extraStamina", 1);
            }
            else
            {
                PlayerPrefs.SetInt("extraStamina", 0);
            }
        }
        if (krille == 1)
        {
            krillerSlow.interactable = true;
            if (krillerSlow.isOn)
            {
                PlayerPrefs.SetInt("slowerKriller", 1);
            }
            else
            {
                PlayerPrefs.SetInt("slowerKriller", 0);
            }
        }
        if (walk == 1)
        {
            walkThrough.interactable = true;
            if (walkThrough.isOn)
            {
                PlayerPrefs.SetInt("walkThrough", 1);
            }
            else
            {
                PlayerPrefs.SetInt("walkThrough", 0);
            }
        }
        if (block == 1)
        {
            blockPath.interactable = true;
            if (blockPath.isOn)
            {
                PlayerPrefs.SetInt("blockPath", 1);
            }
            else
            {
                PlayerPrefs.SetInt("blockPath", 0);
            }
        }
        if (jam == 1)
        {
            jammer.interactable = true;
            if (jammer.isOn)
            {
                PlayerPrefs.SetInt("jammer", 1);
            }
            else
            {
                PlayerPrefs.SetInt("jammer", 0);
            }
        }
        if (item == 1)
        {
            infItem.interactable = true;
            if (infItem.isOn)
            {
                PlayerPrefs.SetInt("infItem", 1);
            }
            else
            {
                PlayerPrefs.SetInt("infItem", 0);
            }
        }
    }

    public Button mikoMode;
    public TMP_Text mikoText;
    public Image mikoSprite;

    public Button speedyMode;
    public TMP_Text speedyText;
    public Image speedySprite;

    public Button tripleMode;
    public TMP_Text tripleText;
    public Image tripleSprite;

    public Button algerMode;
    public TMP_Text algerText;
    public Image algerSprite;

    public Button stealthyMode;
    public TMP_Text stealthyText;
    public Image stealthySprite;

    public Image storyStar;
    public Image speedyStar;
    public Image mikoStar;
    public Image tripleStar;
    public Image pizzaStar;
    public Image algerStar;
    public Image stealthyStar;
    public Image classicStar;

    public Sprite beat;
    public Sprite notbeat;

    public Toggle speedBoost;
    public Toggle staminaBoost;
    public Toggle krillerSlow;
    public Toggle walkThrough;
    public Toggle blockPath;
    public Toggle infItem;
    public Toggle jammer;

    public bool TestMode;
    private int randomnessHorray;

    private KeyCode[] code = { KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.B, KeyCode.A, KeyCode.Return };
    private int index;

    public AudioClip cheatOn;
    public AudioClip cheatOff;
    public AudioSource globalAudio;
}
