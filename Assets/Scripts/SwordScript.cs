using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwordScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        durability = swordType.durability;
        attack = swordType.attack;
        fill.color = swordType.color;
        attackText.text = swordType.attack.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        fill.color = swordType.color;
        durSlider.value = durability;
        durSlider.maxValue = swordType.durability;
        attack = Mathf.RoundToInt(swordType.attack * Mathf.Clamp(durSlider.value, 0.15f, 1));
        if (durability <= 0)
        {
            ChangeSword(none);
        }
        attackText.text = Mathf.RoundToInt(swordType.attack * Mathf.Clamp(durSlider.value, 0.15f, 1)).ToString();
        swordTypeText.text = swordType.name;

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) && Time.timeScale != 0)
        {
            Physics.Raycast(player.position, player.forward, out RaycastHit h, 10);
            if (h.transform == null)
            {
                return;
            }
            if (h.transform.name == "Zombie")
            {
                ZombieScript zombo = h.transform.gameObject.GetComponent<ZombieScript>();
                zombo.TakeDamage(attack);
            }
        }
    }

    public void ChangeSword(Sword s)
    {
        swordType = s;
        if (s.name != "None" || s.name != "Wooden")
        {
            FindObjectOfType<TrophyCollectingScript>().onlyWooden = false;
        }
        if (s.name != "None")
        {
            FindObjectOfType<TrophyCollectingScript>().usedItem = true;
        }
        fill.color = swordType.color;
    }

    public Sword swordType;
    public Sword none;
    public int durability;
    int attack;

    public Image fill;
    public Slider durSlider;

    public TMP_Text attackText;
    public TMP_Text swordTypeText;

    public Transform player;
}
