using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeverScript : MonoBehaviour
{
    private void Start()
    {
        pull = GetComponent<Animator>();
        rolls = 50 + PlayerPrefs.GetInt("rollBonus", 0);
        rollText.text = $"{rolls} Roll(s)";
        PlayerPrefs.SetInt("rollBonus", 0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
        {
            print(rolls);
            if (rolls > 0 && luckScreen.offset != 4)
            {
                if (luckScreen.isRolling)
                {
                    return;
                }
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;
                if (Physics.Raycast(ray, out raycastHit))
                {
                    if (raycastHit.transform.gameObject == transform.gameObject & Vector3.Distance(this.player.position, base.transform.position) < 10f & Cursor.lockState == CursorLockMode.Locked)
                    {
                        Pull();
                        rolls--;
                        rollText.text = $"{rolls} Roll(s)";
                    }
                }
            }
            else
            {
                rolls = 0;
                rollText.text = $"{rolls} Roll(s)";
            }
        }
    }

    public void Pull()
    {
        if (!luckScreen.isRolling)
        {
            pull.SetTrigger("Pull the lever or whatever RIGHT NOW RIGHT NOW");
            luckScreen.Roll();
        }
    }

    public LuckScreenScript luckScreen;

    Animator pull;

    public Transform player;

    public int rolls;
    public TMP_Text rollText;
}