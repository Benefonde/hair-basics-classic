using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameControllerScriptSimple : MonoBehaviour
{
    private void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 0.5f);
        audioDevice = GetComponent<AudioSource>();
        LockMouse();
        if (algerBeat)
        {
            ps.walkSpeed = 0;
            ps.runSpeed = 0;
            StartCoroutine(WaitTillTeleport());
        }
    }

    public void LockMouse()
    {
        cursorController.LockCursor();
        mouseLocked = true;
        reticle.SetActive(value: true);
    }

    public void UnlockMouse()
    {
        cursorController.UnlockCursor();
        mouseLocked = false;
        reticle.SetActive(value: false);
    }

    public IEnumerator WaitTillTeleport()
    {
        yield return new WaitForSecondsRealtime(10);
        player.position = new Vector3(15, 4, 95);
        ps.playerRotation = Quaternion.Euler(new Vector3(0, -90, 0));
    }

    public IEnumerator SwitchPressed()
    {
        ps.walkSpeed = 10;
        ps.runSpeed = 20;
        PlayerPrefs.SetInt("infItemUnlocked", 1);
        for (int i = 0; i < 50; i++)
        {
            Instantiate(balloon, new Vector3(Random.Range(15f, 55f), 5, Random.Range(75f, 115)), Quaternion.identity);
        }
        yield return new WaitForSecondsRealtime(15);
        SceneManager.LoadScene("BenefondCrates");
    }

    public CursorControllerScript cursorController;

    public bool mouseLocked;

    public GameObject reticle;

    public AudioSource audioDevice;

    public Transform player;
    public PlayerScriptSimple ps;

    public GameObject balloon;

    public bool algerBeat;
}
