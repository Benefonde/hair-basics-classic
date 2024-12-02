using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BossControllerScript : MonoBehaviour
{
    void Start()
    {
        gc.camScript.disable3Dcam = true;
        gc.debugMode = true;
        alger.agent.Warp(new Vector3(-95, alger.transform.position.y, 5));
        alger.speed = 0;
        gc.playerScript.walkSpeed = 0;
        gc.playerScript.runSpeed = 0;
        Begin();
    }

    public void Begin()
    {
        gc.finaleMode = false;
        gc.camScript.ShakeNow(new Vector3(1, 1, 1), 10);
        string[] thing = { "Nerd! You can't beat this mode, cause I said so!", "So you will quit the game!" };
        float[] thingie = { 3.920f, 2.37825f };
        Color[] thingest = { Color.blue, Color.blue };
        gc.entrance_4.Lower();
        alger.baldiAudio.PlayOneShot(alger.preBoss[0]);
        musicAud.loop = true;
        musicAud.clip = bossMusic[0]; 
        musicAud.Play();
        FindObjectOfType<SubtitleManager>().AddChained3DSubtitle(thing, thingie, thingest, alger.transform);
        gc.audioDevice.PlayOneShot(gc.aud_Switch);
        gc.playerScript.walkSpeed = 30;
        gc.playerScript.runSpeed = 30;
        gc.hud.SetActive(false);
        bossHud.SetActive(true);
        gc.map.SetActive(false);
        gc.world.SetActive(false);
        for (int i = 0; i < gc.tutorals.Length; i++)
        {
            gc.tutorals[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < 3; i++)
        {
            gc.item[i] = 0;
        }
        for (int i = 0; i < FindObjectsOfType<PickupScript>().Length; i++)
        {
            FindObjectsOfType<PickupScript>()[i].gameObject.SetActive(false);
        }
        bossObjectManager.SetActive(true);
    }

    void Update()
    {
        if (alger.health == 0 && !gc.ModifierOn())
        {
            PlayerPrefs.SetInt("algerBeat", 1);
            SceneManager.LoadScene("AlgerBeat");
        }
        else if (gc.ModifierOn() && alger.health <= 1)
        {
            alger.bossMode = true;
            alger.disableWanderOrTarget = false;
            gc.playerTransform.position = alger.transform.position;
        }
        bar.value = alger.health;
        hp.text = $"{alger.health}/20";
    }

    public IEnumerator ChangeMusic(int i)
    {
        musicAud.loop = false;
        yield return new WaitUntil(() => !musicAud.isPlaying);
        musicAud.clip = bossMusic[i];
        if (i != 2)
        {
            musicAud.Play();
        }
        if (i != 1)
        {
            musicAud.loop = true;
        }
        else
        {
            StartCoroutine(ChangeMusic(2));
        }
        if (i == 2)
        {
            yield return new WaitUntil(() => !musicAud.isPlaying);
            musicAud.Play();
        }
    }

    public AlgerNullScript alger;
    public GameControllerScript gc;

    public GameObject bossObjectManager;
    public GameObject bossHud;

    public Slider bar;
    public TMP_Text hp;

    public AudioSource musicAud;
    public AudioClip[] bossMusic;
    // 0 - intro loop
    // 1 - intro
    // 2 - part/phase 1
    // 3 - part/phase 2
    // 4 - part/phase 3
    // 5 - part/phase 3 fast
    // 6 - outro, unused
}
