﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecretEyeScript : MonoBehaviour
{
    private void Start()
    {
        normalSky = RenderSettings.skybox;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player")
        {
            playerWlkSpeed = player.walkSpeed;
            playerRunSpeed = player.runSpeed;
            SecretEnter();
        }
    }

    void SecretEnter()
    {
        if (player.gc.finaleMode && secretEnterance)
        {
            player.gc.timer.isActivated = false;
        }
        else if (player.gc.finaleMode && !secretEnterance)
        {
            player.gc.timer.isActivated = true;
        }
        player.runSpeed = 0;
        player.walkSpeed = 0;
        if (secretEnterance)
        {
            player.inSecret = true;
            player.gc.secretsFound++;
        }
        else
        {
            player.inSecret = false;
        }
        secretEye.PlayOneShot(secretEnter);
        Invoke("SecretExit", secretEnter.length);
    }

    void SecretExit()
    {
        if (secretEnterance)
        {
            if (player.gc.finaleMode)
            {
                player.gc.pizzaTimeMusic.Pause();
                player.gc.lap2Music.Pause();
                player.gc.wwnMusic.Pause();
                player.gc.pizzaTimeTimer.SetBool("up", false);
            }
            secretMusic.SetActive(true);
            RenderSettings.skybox = secretSky;
            player.gc.baldiScrpt.enabled = false;
            player.gc.principalScript.enabled = false;
            player.gc.firstPrizeScript.enabled = false;
            player.gc.bigball.GetComponent<AgentTest>().enabled = false;
            player.gc.crafters.GetComponent<CraftersScript>().enabled = false;
            player.gc.baba.GetComponent<BabaScript>().enabled = false;
            if (PlayerPrefs.GetInt("yellow", 0) == 1)
            {
                player.gc.yellowFace.SetActive(false);
            }
        }
        else
        {
            secretMusic.SetActive(false);
            if (player.gc.finaleMode)
            {
                player.gc.pizzaTimeMusic.UnPause();
                player.gc.lap2Music.UnPause();
                player.gc.wwnMusic.UnPause();
                player.gc.pizzaTimeTimer.SetBool("up", true);
            }
            RenderSettings.skybox = normalSky;
            player.gc.baldiScrpt.enabled = true;
            player.gc.principalScript.enabled = true;
            player.gc.firstPrizeScript.enabled = true;
            player.gc.bigball.GetComponent<AgentTest>().enabled = true;
            player.gc.crafters.GetComponent<CraftersScript>().enabled = true;
            player.gc.baba.GetComponent<BabaScript>().enabled = true;
            if (PlayerPrefs.GetInt("yellow", 0) == 1)
            {
                player.gc.yellowFace.SetActive(true);
            }
        }
        player.runSpeed = playerRunSpeed;
        player.walkSpeed = playerWlkSpeed;
        player.transform.position = pos;
        gameObject.SetActive(false);
        StartCoroutine(player.PlayerInv());
    }

    public Vector3 pos;
    public bool secretEnterance;
    public PlayerScript player;

    public AudioSource secretEye;
    public AudioClip secretEnter;

    public GameObject secretMusic;

    public Material secretSky;
    private Material normalSky;

    private float playerRunSpeed;
    private float playerWlkSpeed;
}