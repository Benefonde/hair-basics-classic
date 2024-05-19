using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class ObjectionItem
{
    public int objecUses;

    private int initialUsesSaved;

    public void Collect(int initialUses)
    {
        objecUses = initialUses;
        initialUsesSaved = initialUses;
    }

    public void Objection(Transform cameraTransform, AudioSource audioDevice, AudioClip objectionSound, CameraScript camScript, GameObject objection, Transform playerTransform, GameControllerScript gameController)
    {
        objecUses--;
        float num = (float)(Mathf.RoundToInt((cameraTransform.rotation.eulerAngles.y - 90f) / 4f) * 4);
        audioDevice.PlayOneShot(objectionSound);
        if (PlayerPrefs.GetInt("shake", 1) == 1)
        {
            camScript.ShakeNow(new Vector3(0.2f, 0.05f, 0.2f), 20);
        }
        for (int i = 0; i < 3; i++)
        {
            Object.Instantiate<GameObject>(objection, playerTransform.position, Quaternion.Euler(0f, num, 0f));
            num += 90f;
        }
        if (objecUses == 0)
        {
            gameController.ResetItem();
            objecUses = initialUsesSaved;
        }
        gameController.UpdateItemName();
    }
}

public class GameControllerScript : MonoBehaviour
{
    public TrophyCollectingScript tc;
    public bool SchoolScene;
    public bool ClassicSchoolScene;

    public Material night;
    public Material day;

    public GameObject dwayneDebt;

    public int yellowFaceOn;

    public GameObject map;
    public GameObject world;
    public GameObject border;

    public bool playAgain;

    private int pCounter;

    public GameControllerScript()
    {
        itemSelectOffset = new int[4] { -134, -94, -50, -6 };
    }

    public bool HasItemInInventory(int itemID)
    {
        if (item[0] == itemID || item[1] == itemID || item[2] == itemID || item[3] == itemID)
        {
            return true;
        }
        return false;
    }

    private void Start()
    {
        tc = GetComponent<TrophyCollectingScript>();
        if (PlayerPrefs.GetInt("heldItemShow", 0) == 0)
        {
            heldItem.SetActive(false);
        }
        maxItems = 4;
        if (PlayerPrefs.GetInt("minimap", 1) == 0)
        {
            map.SetActive(false);
            world.SetActive(false);
            border.SetActive(false);
        }
        math = PlayerPrefs.GetInt("math", 1);
        speedBoost = PlayerPrefs.GetInt("speedBoost", 0);
        extraStamina = PlayerPrefs.GetInt("extraStamina", 0);
        slowerKriller = PlayerPrefs.GetInt("slowerKriller", 0);
        walkThrough = PlayerPrefs.GetInt("walkThrough", 0);
        blockPath = PlayerPrefs.GetInt("blockPath", 0);
        infItem = PlayerPrefs.GetInt("infItem", 0);
        jammers = PlayerPrefs.GetInt("jammer", 0);
        yellowFaceOn = PlayerPrefs.GetInt("yellow", 0);
        algerKrilledByPlayer = false;
        originalTimeScale = 1;
        Time.timeScale = originalTimeScale;
        amountOfExit = 4;
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 0.5f);
        hour = System.DateTime.Now.Hour;
        minute = System.DateTime.Now.Minute;
        cullingMask = cameraNormal.cullingMask;
        audioDevice = GetComponent<AudioSource>();
        mode = PlayerPrefs.GetString("CurrentMode");
        if (SchoolScene)
        {
            if (PlayerPrefs.GetInt("timer") == 1)
            {
                modeTimer.SetActive(true);
            }
            if (mode == "story" || mode == "pizza" || mode == "free")
            {
                objecUsesinit = 6;
            }
            if (mode == "endless" || mode == "triple" || mode == "alger")
            {
                objecUsesinit = 12;
            }
            if (mode == "miko" || mode == "chaos")
            {
                objecUsesinit = 4;
            }
            if (mode == "speedy" || mode == "dark")
            {
                objecUsesinit = 8;
            }
            if (mode == "dark" || mode == "pizza" || mode == "stealthy" || mode == "alger" || mode == "free")
            {
                math = 0;
            }
            if (walkThrough == 1)
            {
                playerScript.walkThroughAbility = true;
            }
            if (blockPath == 1)
            {
                playerScript.blockPathAbility = true;
            }
            if (infItem == 1)
            {
                TestingItemsMode = true;
            }
            if (jammers == 1)
            {
                player.jammerBar.gameObject.SetActive(true);
                jammerMeter.SetActive(true);
            }
            if (!ClassicSchoolScene)
            {
                entrance_4.Lower();
            }
            if (mode == "speedy")
            {
                this.SpeedyStart();
            }
            else if (mode == "story" || mode == "endless" || mode == "pizza" || mode == "free" || mode == "dark" || mode == "chaos")
            {
                this.NormalStart();

                if (mode == "endless")
                {
                    baldiScrpt.endless = true;
                }
                if (mode == "pizza")
                {
                    psgo.SetActive(true);
                    toppins.SetActive(true);
                    secrets.SetActive(true);
                    lap2Portal.SetActive(true);
                    for (int i = 0; i < secretWalls.Length; i++)
                    {
                        secretWalls[i].material = secretWall2;
                    }
                    asdaa.SetActive(false);
                }
                if (mode == "dark")
                {
                    notebookCount.color = Color.white;
                    RenderSettings.ambientLight = Color.black;
                    RenderSettings.skybox = night;
                    hour = 1;
                    player.GetComponent<Light>().enabled = true;
                    principal.GetComponent<Light>().enabled = true;
                }
            }
            else if (mode == "miko")
            {
                this.MikoStart();
            }
            else if (mode == "triple")
            {
                this.TripleStart();
            }
            else if (mode == "alger")
            {
                this.AlgerStart();
            }
            else if (mode == "stealthy")
            {
                StealthyStart();
            }

            if (extraStamina == 1)
            {
                player.maxStamina = 200;
            }
            if (!ClassicSchoolScene)
            {
                for (int i = 0; i < secretWalls.Length; i++)
                {
                    Physics.IgnoreCollision(playerCharacter, secretWalls[i].GetComponent<MeshCollider>());
                }
            }
            if (speedBoost == 1)
            {
                player.walkSpeed *= 1.7f;
                player.runSpeed *= 1.7f;
            }
            objectItem = new ObjectionItem[item.Length];

            for (int i = 0; i < objectItem.Length; i++)
            {
                objectItem[i] = new ObjectionItem();
                objectItem[i].Collect(objecUsesinit);
            }
        }
        if (ClassicSchoolScene)
        {
            if (walkThrough == 1)
            {
                playerScript.walkThroughAbility = true;
            }
            if (blockPath == 1)
            {
                playerScript.blockPathAbility = true;
            }
            if (jammers == 1)
            {
                player.jammerBar.gameObject.SetActive(true);
                jammerMeter.SetActive(true);
            }
            if (infItem == 1)
            {
                TestingItemsMode = true;
            }
            this.ClassicStart();
            if (extraStamina == 1)
            {
                player.maxStamina = 200;
            }
            for (int i = 0; i < secretWalls.Length; i++)
            {
                Physics.IgnoreCollision(playerCharacter, secretWalls[i].GetComponent<MeshCollider>());
            }
            if (speedBoost == 1)
            {
                player.walkSpeed *= 1.7f;
                player.runSpeed *= 1.7f;
            }
        }
        LockMouse();
        UpdateNotebookCount();
        itemSelected = 0;
        gameOverDelay = 0.5f;
    }

    public void SpeedyStart()
    {
        CollectItem(11);
        player.walkSpeed = 46;
        player.runSpeed = 64;
        baldiScript.baldiAnger = 18f;
        baldiScript.baldiWait = 0.4f;
        if (slowerKriller == 1)
        {
            baldiScript.baldiAnger = 13f;
            baldiScript.baldiWait = 1.2f;
        }
        spoopMode = true;
        entrance_0.Lower();
        entrance_1.Lower();
        entrance_2.Lower();
        entrance_3.Lower();
        player.transform.position = new Vector3(5, 4, 125);
        player.transform.rotation = Quaternion.Euler(0, -45, 0);
        cameraNormal.transform.position = new Vector3(5, 5, 125);
        cameraNormal.transform.rotation = Quaternion.Euler(0, -45, 0);
        baldiTutor.SetActive(false);
        if (yellowFaceOn == 0)
        {
            baldi.SetActive(true);
        }
        else
        {
            windowedWall.material = broken;
            yellowFace.SetActive(true);
            MikoScript yellowey = yellowFace.GetComponent<MikoScript>();
            yellowey.baldiAudio.PlayOneShot(brokenWindow);
            camScript.ShakeNow(new Vector3(0.2f, 0.2f, 0.2f), 10);
        }
        locationText.text = "Panino's Ball, sometime after Story Mode";
        locationText.color = Color.blue;
        speedyItemLayout.SetActive(true);
        craftersTime = false;
        RenderSettings.skybox = day;
    }

    public void NormalStart()
    {
        locationText.text = "Panino's Ball, " + hour + ":" + minute;
        locationText.color = Color.blue;
        normalItemLayout.SetActive(true);
        craftersTime = true;
        if (hour < 7 || hour > 18)
        {
            RenderSettings.skybox = night;
            RenderSettings.ambientLight = new Color(0.8f, 0.8f, 0.8f);
            schoolMusic.clip = darkSchool;
        }
        else
        {
            RenderSettings.skybox = day;
        }
        if (slowerKriller == 1)
        {
            baldiScript.baldiSpeedScale = 0.5875f;
        }
        schoolMusic.Play();
    }

    public void MikoStart()
    {
        amountOfExit = 3;
        baldiTutor.SetActive(false);
        entrance_0.Lower();
        entrance_1.Lower();
        entrance_2.Lower();
        entrance_3.Lower();
        player.transform.position = new Vector3(5, 4, 5);
        cameraTransform.position = new Vector3(5, 5, 5);
        spoopMode = true;
        locationText.text = "Panino's Ball, 34 days later";
        locationText.color = Color.red;
        mikoItemLayout.SetActive(true);
        int yellow = yellowFaceOn;
        miko.SetActive(true);
        if (yellow == 1)
        {
            windowedWall.material = broken;
            FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Break!*", brokenWindow.length, Color.white, windowedWall.transform);
            yellowFace.SetActive(true);
            MikoScript yellowey = yellowFace.GetComponent<MikoScript>();
            yellowey.baldiAudio.PlayOneShot(brokenWindow);
            camScript.ShakeNow(new Vector3(0.2f, 0.2f, 0.2f), 10);
        }
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.black;
        RenderSettings.skybox = night;
        craftersTime = false;
    }

    public void SetTime(float timeScale)
    {
        if (timeScale == 0)
        {
            PauseGame();
            pauseMenu.SetActive(false);
            AudioListener.pause = false;
            FindObjectOfType<SubtitleManager>().localTimeScale = 1;
        }
        else
        {
            Time.timeScale = timeScale;
            originalTimeScale = timeScale;
        }
    }

    public void TripleStart()
    {
        amountOfExit = 4;
        baldiTutor.SetActive(false);
        entrance_0.Lower();
        entrance_1.Lower();
        entrance_2.Lower();
        entrance_3.Lower();
        player.transform.position = new Vector3(5, 4, 5);
        cameraTransform.position = new Vector3(5, 5, 5);
        spoopMode = true;
        playerScript.runSpeed += 8;
        playerScript.walkSpeed += 6;
        locationText.text = "Panino's Ball, alternative universe";
        locationText.color = Color.white;
        mikoItemLayout.SetActive(true);
        miko.SetActive(true);
        baldi.SetActive(true);
        alger.SetActive(true);
        if (slowerKriller == 1)
        {
            baldiScript.baldiSpeedScale = 0.5875f;
            algerScript.baldiSpeedScale = 0.55f;
        }
        if (yellowFaceOn == 1)
        {
            windowedWall.material = broken;
            yellowFace.SetActive(true);
            MikoScript yellowey = yellowFace.GetComponent<MikoScript>();
            yellowey.baldiAudio.PlayOneShot(brokenWindow);
            camScript.ShakeNow(new Vector3(0.2f, 0.2f, 0.2f), 10);
        }
        RenderSettings.ambientLight = new Color(0.8f, 0.5f, 0.5f, 1);
        baldiAgent.Warp(new Vector3(5, 1.64f, 135));
        RenderSettings.skybox = night;
        craftersTime = false;
    }

    public void AlgerStart()
    {
        locationText.text = "Panino's Ball, ";
        locationText.color = Color.white;
        algerItemLayout.SetActive(true);
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fogDensity = 0.01f;
        RenderSettings.skybox = night;
        craftersTime = false;
        spoopMode = true;
        baldiTutor.SetActive(false);
        entrance_0.Lower();
        entrance_1.Lower();
        entrance_2.Lower();
        entrance_3.Lower();
        player.transform.position = new Vector3(5, 4, 5);
        cameraTransform.position = new Vector3(5, 5, 5);
        algerNull.SetActive(true);
        if (yellowFaceOn == 1)
        {
            windowedWall.material = broken;
            yellowFace.SetActive(true);
            MikoScript yellowey = yellowFace.GetComponent<MikoScript>();
            yellowey.baldiAudio.PlayOneShot(brokenWindow);
            camScript.ShakeNow(new Vector3(0.2f, 0.2f, 0.2f), 10);
        }
    }
    public void StealthyStart()
    {
        amountOfExit = 0;
        for (int i = 0; i < 16; i++)
        {
            dwaynes[Random.Range(0, 16)].gameObject.SetActive(false);
        }
        maxNoteboos = FindObjectsOfType<NotebookScript>().Length;
        CollectItem(20);
        CollectItem(20);
        CollectItem(20);
        CollectItem(1);
        playerScript.ResetGuilt("afterHours", 2147483647);
        locationText.text = $"Panino's Ball, 3:{minute}AM";
        playerScript.walkSpeed += 8;
        playerScript.runSpeed += 15;
        locationText.color = Color.cyan;
        stealthyItemLayout.SetActive(true);
        RenderSettings.skybox = night;
        RenderSettings.ambientLight = Color.gray;
        craftersTime = false;
        spoopMode = true;
        baldiTutor.SetActive(false);
        entrance_0.Lower();
        entrance_1.Lower();
        entrance_2.Lower();
        entrance_3.Lower();
        player.transform.position = new Vector3(5, 4, 5);
        cameraTransform.position = new Vector3(5, 5, 5);
        Invoke("SpawnAlgers", 2);
    }

    void ClassicStart()
    {
        playerScript.walkSpeed = 10;
        playerScript.runSpeed = 15;
        amountOfExit = 3;
        maxNoteboos = 7;
        locationText.text = string.Empty;
        locationText.color = Color.clear;
        craftersTime = true;
        if (slowerKriller == 1)
        {
            baldiScript.baldiSpeedScale = 0.5875f;
        }
        schoolMusic.Play();
    }

    public bool ModifierOn()
    {
        if (speedBoost == 1)
        {
            return true;
        }
        if (extraStamina == 1)
        {
            return true;
        }
        if (slowerKriller == 1)
        {
            return true;
        }
        if (walkThrough == 1)
        {
            return true;
        }
        if (blockPath == 1)
        {
            return true;
        }
        if (infItem == 1)
        {
            return true;
        }
        if (jammers == 1)
        {
            return true;
        }
        return false;
    }

    void SpawnAlgers()
    {
        principal.SetActive(true);
        if (yellowFaceOn == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject bro = Instantiate(principal);
                bro.transform.name = "Alger (Hair Basics)";
            }
        }
        else
        {
            for (int i = 0; i < 30; i++)
            {
                GameObject bro = Instantiate(principal);
                bro.transform.name = "Alger (Hair Basics)";
            }
            windowedWall.material = broken;
            yellowFace.SetActive(true);
            for (int i = 0; i < 9; i++)
            {
                GameObject bro = Instantiate(yellowFace);
                bro.transform.name = "Yellow Face";
            }
            MikoScript yellowey = yellowFace.GetComponent<MikoScript>();
            yellowey.baldiAudio.PlayOneShot(brokenWindow);
            camScript.ShakeNow(new Vector3(0.2f, 0.2f, 0.2f), 10);
        }
    }

    public void LoseNotebooks(int amount, float multiply)
    {
        notebooks -= amount;
        UpdateNotebookCount();
        if (notebooks < 0)
        {
            notebooks *= Mathf.RoundToInt(multiply);
            UpdateNotebookCount();
            NotebookDebt();
        }
    }

    public void NotebookDebt()
    {
        ESCAPEmusic.Play();
        dwayneDebt.SetActive(true);
        dwayneDebtTimer = ESCAPEmusic.clip.length;
        player.stamina += player.maxStamina * 2.50f;
        for (int i = 0; i < dwaynes.Length; i++)
        {
            dwaynes[i].transform.position = new Vector3(dwaynes[i].transform.position.x, 4, dwaynes[i].transform.position.z);
            dwaynes[i].audioDevice.Play();
        }
    }

    public void ItsPizzaTime()
    {
        audioDevice.PlayOneShot(pillarHit);
        fadeToWhite.SetTrigger("pisstime");
        pizzaTimee.SetActive(true);
        ActivateFinaleMode();
        principal.SetActive(true);
        principalScript.angry = false;
        pizzaTimeMusic.Play();
        if (slowerKriller == 1)
        {
            baldiScript.baldiWait = 1f;
        }
        else
        {
            baldiScript.baldiWait = 0.6f;
        }
        baldiScript.baldiTempAnger = 0f;
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            time += Time.unscaledDeltaTime;
            System.TimeSpan timee = System.TimeSpan.FromSeconds(time);
            modeTimer.GetComponent<TMP_Text>().text = string.Format("{0:00}:{1:00}:{2:000}", timee.Minutes, timee.Seconds, timee.Milliseconds);
        }
        if (SchoolScene)
        {
            dwayneDebtTimerText.text = Mathf.CeilToInt(dwayneDebtTimer).ToString();
        }
        if (mode == "pizza" && finaleMode)
        {
            if (scoreDecayTimer <= 0)
            {
                pss.AddPoints(-5, 0.5f);
                scoreDecayTimer = 1;
                player.stamina += player.maxStamina / 20;
            }
            else if (!player.inSecret)
            {
                scoreDecayTimer -= 1 * Time.deltaTime;
            }

            if (timer.timeLeft < 57 && pizzaTimeMusic.time < 171)
            {
                pizzaTimeMusic.time = 171;
            }
        }
        if (pss.score < 1 & finaleMode & pizzaface.isActiveAndEnabled)
        {
            player.gameOver = true;
        }
        if (at.clickCount > 0)
        {
            getout.text = "YOU HAVE BEEN BALLED! CLICK " + at.clickCount + " TIMES TO GET OUT OF BEING BALLED!";
            sliderClickOut.value = at.clickCount;
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) && !gamePaused)
            {
                at.clickCount -= 1;
                sliderClickOut.value = at.clickCount;
            }
        }
        else
        {
            getoutObject.SetActive(false);
        }
        if (dwayneDebtTimer > 0 && notebooks < 0)
        {
            if (Time.deltaTime > 1)
            {
                dwayneDebtTimer -= Time.unscaledDeltaTime;
            }
            else
            {
                dwayneDebtTimer -= Time.deltaTime;
            }
        }
        else if (notebooks < 0)
        {
            if (baldi.activeSelf)
            {
                player.transform.position = baldi.transform.position;
            }
            if (yellowFace.activeSelf)
            {
                player.transform.position = yellowFace.transform.position;
            }
            notebooks = 0;
        }
        else
        {
            dwayneDebtTimer = 0;
        }
        if (craftersWaitTime > 0 && !crafters.activeSelf && craftersTime)
        {
            craftersWaitTime -= Time.deltaTime;
        }
        if (craftersWaitTime < 0 || craftersWaitTime == 0 && !crafters.activeSelf && craftersTime)
        {
            craftersWaitTime = 0;
            crafters.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.F8) && mode == "alger")
        {
            player.transform.position = new Vector3(-105, 4, 25);
            algerNull.GetComponent<AlgerNullScript>().bc.SetActive(true);
            algerNull.GetComponent<AlgerNullScript>().disableWanderOrTarget = true;

            notebooks = 17;
            exitsReached = 4;
            finaleMode = true;
            entrance_4.Raise();
        }
        if (!learningActive)
        {
            if (Input.GetButtonDown("Pause") && !disablePausing)
            {
                if (!gamePaused)
                {
                    PauseGame();
                }
                else
                {
                    UnpauseGame();
                }
            }
            if (Input.GetKeyDown(KeyCode.E) & gamePaused)
            {
                ExitGame();
            }
            else if (Input.GetKeyDown(KeyCode.S) & gamePaused)
            {
                UnpauseGame();
            }
            else if (Input.GetKeyDown(KeyCode.R) & gamePaused)
            {
                RestartScene();
            }
            if (!gamePaused & (Time.timeScale < originalTimeScale))
            {
                Time.timeScale = originalTimeScale;
            }
            if (Input.GetMouseButtonDown(1) && Time.timeScale != 0f)
            {
                UseItem();
            }
            if (Input.GetKeyDown(KeyCode.Q) && Time.timeScale != 0f)
            {
                UseItem();
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && Time.timeScale != 0f)
            {
                DecreaseItemSelection();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Time.timeScale != 0f)
            {
                IncreaseItemSelection();
            }
            if (Time.timeScale != 0f)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    itemSelected = 0;
                    UpdateItemSelection();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    itemSelected = 1;
                    UpdateItemSelection();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    itemSelected = 2;
                    UpdateItemSelection();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    itemSelected = 3;
                    UpdateItemSelection();
                }
            }
        }
        else if (Time.timeScale != 0f)
        {
            Time.timeScale = 0f;
        }
        if ((player.stamina < 0f) & !warning.activeSelf)
        {
            warning.SetActive(value: true);
        }
        else if ((player.stamina > 0f) & warning.activeSelf)
        {
            warning.SetActive(value: false);
        }
        if (player.gameOver)
        {
            GameOverStart();
            gameOverDelay -= Time.unscaledDeltaTime * 0.5f;
            Time.timeScale = 0;
            if (mode == "pizza")
            {
                pss.AddPoints(Mathf.RoundToInt(-50000 * Time.unscaledDeltaTime), 1);
            }
            if (gameOverDelay < 0)
            {
                if (camScript.character.name == "Alger")
                {
                    Application.Quit();
                    return;
                }
                Time.timeScale = 1f;
                SceneManager.LoadScene("GameOver");
            }
        }
        if (lap2Music.time > 21.25f)
        {
            playAgain = true;
        }
        if (lap2Music.time < 21.25f && playAgain)
        {
            lap2Music.time = 21.25f;
        }
        if (mode == "chaos")
        {
            if (CHAOStimer < 0)
            {
                if (Random.Range(1, 15) == 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        item[i] = Random.Range(0, itemHudTextures.Length - 1);
                    }
                }
                playerScript.walkSpeed = Random.Range(15f, 35f);
                playerScript.runSpeed = playerScript.walkSpeed + Random.Range(3f, 9f);
                CHAOStimer = Random.Range(0.5f, 3f);
                if (Random.Range(0, 2) == 1)
                {
                    RenderSettings.ambientLight = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), 1);
                }
                UpdateAllItem();
                baldiScrpt.baldiWait = Random.Range(0.5f, 2f);
                if (Random.Range(1, 30) == 10)
                {
                    Teleport();
                }
            }
            else
            {
                CHAOStimer -= Time.deltaTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            notebooks = maxNoteboos;
            UpdateNotebookCount();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            pCounter++;
            if (pCounter >= 50)
            {
                SceneManager.LoadScene("p");
            }
        }
        if (jammersTimer < 35 && spoopMode && jammers == 1)
        {
            jammersTimer += Time.deltaTime;
        }
        else if (!HasItemInInventory(24) && HasItemInInventory(0) && spoopMode && jammers == 1)
        {
            jammersTimer = 0;
            jammers = 0;
            player.jammerBar.gameObject.SetActive(false);
            jammerMeter.SetActive(false);
            CollectItem(24);
        }
        if (ClassicSchoolScene && (!audioDevice.isPlaying && audioDevice.time == 0) && exitsReached == 3)
        {
            audioDevice.clip = aud_MachineLoop;
            audioDevice.loop = true;
            audioDevice.Play();
        }
        if (ClassicSchoolScene && exitsReached == 3 && Time.deltaTime != 0)
        {
            baldiScrpt.baldiWait -= 0.0075f * Time.deltaTime;
            if (baldiScrpt.baldiWait < 0.05f)
            {
                baldiAgent.Warp(player.transform.position);
            }
        }
    }

    public void SomeoneTied(GameObject gObject)
    {
        print(gObject.transform.name);
        audioDevice.PlayOneShot(aud_chrisAAAAA);
        FindObjectOfType<SubtitleManager>().Add2DSubtitle("EAAAAAGH!!", aud_chrisAAAAA.length, Color.white);
        camScript.ShakeNow(new Vector3(0.2f, 0.2f, 0.2f), 25);
        if (gObject.transform.name == "Player")
        {
            playerTransform.position = new Vector3(playerTransform.position.x, 0.5f, playerTransform.position.z);
        }
        if (mode == "classic")
        {
            return;
        }
        ded.PickRandomText(gObject.transform.name);
    }

    public void UpdateAllItem()
    {
        for (int i = 0; i < 4; i++)
        {
            itemSlot[i].texture = itemHudTextures[item[i]];
        }
        UpdateItemName();
    }

    public void GameOverStart()
    {
        disablePausing = true;
        if (!gameOverPlayed)
        {
            var charac = camScript.character.transform.name;
            Time.timeScale = 0f;
            if (charac == "Panino")
            {
                audioDevice.PlayOneShot(aud_buzz);
                FindObjectOfType<SubtitleManager>().Add2DSubtitle("b a l l s", 0.85f, Color.white);
                gameOverDelay = 0.5f;
            }
            else if (charac == "Alger")
            {
                audioDevice.PlayOneShot(algerKrilledYouHaha);
                gameOverDelay = 5f;
            }
            else
            {
                audioDevice.PlayOneShot(aud_chrisAAAAA);
                FindObjectOfType<SubtitleManager>().Add2DSubtitle("EAAAAAGH!!", aud_chrisAAAAA.length, Color.white);
                gameOverDelay = 1f;
            }
            if (mode == "endless" && notebooks > PlayerPrefs.GetInt("HighBooks") && !highScoreText.activeSelf)
            {
                highScoreText.SetActive(value: true);
            }
            if (mode == "endless")
            {
                if (notebooks > PlayerPrefs.GetInt("HighBooks"))
                {
                    PlayerPrefs.SetInt("HighBooks", notebooks);
                }
                PlayerPrefs.SetInt("CurrentBooks", notebooks);
            }
        }

        gameOverPlayed = true;
    }

    public void UpdateNotebookCount()
    {
        int highScoreBotenook = PlayerPrefs.GetInt("HighBooks");
        if (mode != "endless")
        {
            notebookCount.text = $"{notebooks}/{maxNoteboos} Dwaynes";
        }
        else
        {
            notebookCount.text = $"{notebooks}/{highScoreBotenook} H.S. Dwaynes";
        }
        if ((notebooks == maxNoteboos) & (mode == "story" || mode == "free" || mode == "dark" || mode == "chaos"))
        {
            ActivateFinaleMode();
            principal.SetActive(true);
            principalScript.angry = false;
        }
        if ((notebooks == maxNoteboos) & (mode == "speedy" || mode == "miko" || mode == "triple" || mode == "alger" || mode == "stealthy" || mode == "classic"))
        {
            ActivateFinaleMode();
        }
        if (notebooks == maxNoteboos && mode == "pizza")
        {
            StartCoroutine(EventRing());
        }
        if (notebooks > -1 && dwayneDebt.activeSelf)
        {
            ESCAPEmusic.Stop();
            dwayneDebt.SetActive(false);
            dwayneDebtTimer = 2763;
        }
    }

    IEnumerator EventRing()
    {
        yield return new WaitForSeconds(1.5f);
        audioDevice.PlayOneShot(riiiing);
        FindObjectOfType<SubtitleManager>().Add2DSubtitle("*RIIIIING!*", 1, Color.white);
        yield return new WaitForSeconds(1);
        eventText.SetActive(true);
        yield return new WaitForSeconds(5);
        eventText.SetActive(false);
    }

    public void CollectNotebook()
    {
        notebooks++;
        UpdateNotebookCount();
    }

    public void LockMouse()
    {
        if (!learningActive)
        {
            cursorController.LockCursor();
            mouseLocked = true;
            reticle.SetActive(value: true);
        }
    }

    public void UnlockMouse()
    {
        cursorController.UnlockCursor();
        mouseLocked = false;
        reticle.SetActive(value: false);
    }

    public void PauseGame()
    {
        if (!learningActive)
        {
            AudioListener.pause = true;
            UnlockMouse();
            originalTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            FindObjectOfType<SubtitleManager>().localTimeScale = 0;
            gamePaused = true;
            pauseMenu.SetActive(value: true);
            if (!ClassicSchoolScene)
            {
                for (int i = 0; i < tutorals.Length; i++)
                {
                    tutorals[i].Pause();
                }
            }
        }
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void UnpauseGame()
    {
        AudioListener.pause = false;
        Time.timeScale = originalTimeScale;
        gamePaused = false;
        pauseMenu.SetActive(value: false);
        FindObjectOfType<SubtitleManager>().localTimeScale = 1;
        LockMouse();
        if (!ClassicSchoolScene)
        {
            for (int i = 0; i < tutorals.Length; i++)
            {
                tutorals[i].Play();
            }
        }
    }

    public void FadeToWhite()
    {
        SetTime(0);
        fadeToWhite.updateMode = AnimatorUpdateMode.UnscaledTime;
        fadeToWhite.SetTrigger("fade");
    }

    public void SpawnWithChance(GameObject character, float minRange, float maxRange, float TargetNum, bool integer = true)
    {
        float rng = Random.Range(minRange, maxRange); 
        

        if (Mathf.FloorToInt(rng) == TargetNum)
        {
            character.SetActive(true);
            if (character == crafters)
            {
                craftersTime = true;
            }
        }
        if (character == crafters && Mathf.FloorToInt(rng) != TargetNum)
        {
            craftersTime = false;
        }

        print($"{character} got a {minRange} in {maxRange} chance, and got {Mathf.FloorToInt(rng)}. Target is {TargetNum}");
    }

    public void ActivateSpoopMode()
    {
        spoopMode = true;
        entrance_0.Lower();
        entrance_1.Lower();
        entrance_2.Lower();
        entrance_3.Lower();
        if (mode == "story" || mode == "pizza" || mode == "dark" || mode == "chaos")
        {
            baldiTutor.SetActive(value: false);
            baldi.SetActive(value: true);
            principal.SetActive(value: true);
            SpawnWithChance(crafters, 1, 4, 2, true);
            SpawnWithChance(gottaSweep, 1, 2, 1, true);
            SpawnWithChance(bully, 1, 2, 1, true);
            SpawnWithChance(firstPrize, 1, 3, 2, true);
            SpawnWithChance(guardianAngel, 1, 6, 4, true);
            SpawnWithChance(baba, 1, 3, 2, true);
            SpawnWithChance(devin, 1, 4, 2, true);

            int rng = yellowFaceOn;
            print(rng);
            if (rng == 1)
            {
                windowedWall.material = broken;
                craftersTime = false;
                yellowFace.SetActive(true);
                MikoScript yellowey = yellowFace.GetComponent<MikoScript>();
                yellowey.baldiAudio.PlayOneShot(brokenWindow);
            }
        }
        else if (mode != "free" && mode != "classic")
        {
            int rng = yellowFaceOn;
            if (rng == 1)
            {
                windowedWall.material = broken;
                yellowFace.SetActive(true);
                MikoScript yellowey = yellowFace.GetComponent<MikoScript>();
                yellowey.baldiAudio.PlayOneShot(brokenWindow);
                camScript.ShakeNow(new Vector3(0.2f, 0.2f, 0.2f), 10);
            }
            baldiTutor.SetActive(value: false);
            baldi.SetActive(value: true);
            principal.SetActive(value: true);
            SpawnWithChance(crafters, 1, 3, 2, true);
            SpawnWithChance(gottaSweep, 1, 5, 2, true);
            SpawnWithChance(bully, 1, 4, 2, true);
            SpawnWithChance(firstPrize, 1, 7, 2, true);
            SpawnWithChance(guardianAngel, 1, 10, 4, true);
            SpawnWithChance(baba, 1, 4, 2, true);
            SpawnWithChance(devin, 1, 3, 2, true);
        }
        else if (mode == "free")
        {
            int rng = yellowFaceOn;
            if (rng == 1)
            {
                windowedWall.material = broken;
                yellowFace.SetActive(true);
                MikoScript yellowey = yellowFace.GetComponent<MikoScript>();
                yellowey.baldiAudio.PlayOneShot(brokenWindow);
                camScript.ShakeNow(new Vector3(0.2f, 0.2f, 0.2f), 10);
            }
            principal.SetActive(value: true);
            SpawnWithChance(crafters, 1, 1, 1, true);
            SpawnWithChance(gottaSweep, 1, 1, 1, true);
            SpawnWithChance(bully, 1, 1, 1, true);
            SpawnWithChance(firstPrize, 1, 1, 1, true);
            SpawnWithChance(guardianAngel, 1, 1, 1, true);
            SpawnWithChance(baba, 1, 1, 1, true);
            SpawnWithChance(devin, 1, 1, 1, true);
        }
        else
        {
            baldiTutor.SetActive(value: false);
            baldi.SetActive(value: true);
            principal.SetActive(value: true);
            crafters.SetActive(true);
            gottaSweep.SetActive(true);
            bully.SetActive(true);
            firstPrize.SetActive(true);
            guardianAngel.SetActive(true);
            baba.SetActive(true);
            devin.SetActive(true);
        }
        audioDevice.PlayOneShot(aud_Hang);
        FindObjectOfType<SubtitleManager>().Add2DSubtitle("Ayo", aud_Hang.length, Color.cyan);
        learnMusic.Stop();
        schoolMusic.Stop();
        if (mode == "pizza")
        {
            pss.AddPoints(50, 2);
        }
    }

    private void ActivateFinaleMode()
    {
        laps++;
        if (mode == "pizza")
        {
            lapCount.text = "Lap 1";
            lapCount.gameObject.SetActive(true);
            escapeCollect.SetActive(true);
        }
        finaleMode = true;
        entrance_0.Raise();
        if (mode != "stealthy")
        {
            entrance_1.Raise();
            entrance_2.Raise();
            entrance_3.Raise();
        }
        if (mode == "classic")
        {
            baldiScript.baldiWait = 0.7f;
        }
        if (mode != "classic")
        {
            player.walkSpeed += 4f;
            player.runSpeed += 6f;
        }
        if (mode != "miko" & mode != "triple" & mode != "alger" & mode != "stealthy" & mode != "classic")
        {
            timer.isActivated = true;
            if (this.mode == "speedy")
            {
                this.timer.timeLeft = 40f;
            }
            else if (this.mode == "chaos")
            {
                this.timer.timeLeft = 120f;
            }
            else if (mode != "pizza")
            {
                this.timer.timeLeft = 80f;
                schooldarkenthingy.SetTrigger("RUN");
            }
            else if (mode == "pizza")
            {
                this.timer.timeLeft = 155f;
                pizzaTimeTimer.gameObject.SetActive(true);
                pizzaTimeTimer.SetBool("up", true);
                pss.AddPoints(700, 0);
            }
        }
        notebookCount.text = "Get to the starting area!"; 
        if (mode != "stealthy")
        {
            notebookCount.text = "0/4 Exits";
        }
        foreach (VideoPlayer player in tutorals)
        {
            player.clip = panic;
        }
    }

    public void GetAngry(float value)
    {
        if (!spoopMode)
        {
            ActivateSpoopMode();
        }
        if (baldiScrpt.isActiveAndEnabled)
        {
            baldiScrpt.GetAngry(value);
        }
        if (algerScript.isActiveAndEnabled)
        {
            algerScript.GetAngry(value);
        }
    }

    public void ActivateLearningGame()
    {
        learningActive = true;
        UnlockMouse();
        tutorBaldi.Stop();
        if (!spoopMode)
        {
            schoolMusic.Stop();
            learnMusic.Play();
        }
        ESCAPEmusic.Pause();
    }

    public void Lap2Enter()
    {
        laps++;
        lapCount.text = $"Lap {laps}";
        playerCharacter.enabled = true;
        playerCollider.enabled = true;
        exitsReached = 0;
        notebookCount.text = "0/4 Exits";
        RenderSettings.ambientLight = Color.red;
        RenderSettings.fog = false;
        if (laps == 2)
        {
            pizzaTimeMusic.Stop();
            lap2Music.Play();
        }
        if (laps == 7)
        {
            lap2Music.Stop();
            wwnMusic.Play();
        }
        if (laps <= 15)
        {
            player.walkSpeed += 0.75f;
            player.runSpeed += 1.25f;
            baldiScrpt.baldiWait -= 0.0125f;
        }
        entrance_0.Raise();
        entrance_1.Raise();
        entrance_2.Raise();
        entrance_3.Raise();
        entrance_4.Lower();
        player.transform.position = new Vector3(5, 4, 135);
        player.SetRotation(Quaternion.Euler(0, -90, 0));
        baldiAgent.Warp(new Vector3(65, 1.64f, 135));
        guardianAngel.SetActive(false);
        pizzaface.gameObject.transform.position = new Vector3(5, 4, 135);
        princAgent.Warp(new Vector3(10, 0, 186.3f));
        craftAgent.Warp(new Vector3(-5, 0, 300));
        ballAgent.Warp(new Vector3(65, 1.65f, 335));
        NavMeshAgent yellowey = yellowFace.GetComponent<NavMeshAgent>();
        yellowey.Warp(new Vector3(-230.5f, 2, 53.5f));
    }

    public void Lap2EnterPortal()
    {
        if (laps < 50)
        {
            for (int i = 0; i < escapeCollect.GetComponentsInChildren<CollectableScript>().Length; i++)
            {
                escapeCollect.GetComponentsInChildren<CollectableScript>()[i].TpBackUp();
            }
        }
        if (pizzaface.isActiveAndEnabled)
        {
            pizzaface.pauseTime = 4.5f;
        }
        if (laps > 2)
        {
            pss.AddPoints(3000, 1);
        }
        else
        {
            pss.AddPoints(1205, 1);

        }
        player.stamina += player.maxStamina * 0.75f;
        audioDevice.PlayOneShot(getInPortal);
        camScript.ShakeNow(new Vector3(0.3f, 0.15f, 0.3f), 15);
        playerCharacter.enabled = false;
        playerCollider.enabled = false;
        if (laps > 50)
        {
            timer.timeLeft += 15;
        }
        Invoke("Lap2Enter", 1f);
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        AudioListener.pause = false;
        SceneManager.LoadScene(scene.name);
    }

    public void DeactivateLearningGame(GameObject subject)
    {
        string[] escape = { "Congrattation!", "You found all 7 Dwaynes,", "now all you need to do is...", "GET OUT." };
        float[] duration = { 1.8f, 3f, 2.8f, 1.935f };
        Color[] colors = { Color.white, Color.white, Color.white, Color.white };
        ESCAPEmusic.UnPause();
        cameraNormal.cullingMask = cullingMask;
        learningActive = false;
        if (subject != null)
        {
            Destroy(subject);
        }
        LockMouse();
        if (mode == "speedy" || mode == "miko" || mode == "triple" || mode == "alger" || mode == "stealthy")
        {
            player.stamina += player.maxStamina * 0.55f;
        }
        else
        {
            if (player.stamina < player.maxStamina * 1.15f)
            {
                player.stamina = player.maxStamina * 1.15f;
            }
        }
        if (!spoopMode)
        {
            schoolMusic.Play();
            learnMusic.Stop();
        }
        if ((notebooks == 1) & !spoopMode)
        {
            string[] blabber = { "Hi. You might have noticed the quarter that was lying right next to me,", "but that wasn't my quarter. Some other kid dropped this.", "Here, have my quarter" };
            float[] duratione = { 3.5f, 3, 1.95f };
            Color[] colore = { Color.cyan, Color.cyan, Color.cyan };
            quarter.SetActive(value: true);
            tutorBaldi.PlayOneShot(aud_Prize);
            switch (!ClassicSchoolScene)
            {
                case true: FindObjectOfType<SubtitleManager>().AddChained3DSubtitle(blabber, duratione, colore, tutorBaldi.transform); break;
                case false: FindObjectOfType<SubtitleManager>().Add3DSubtitle("Hi. Cool job. Have my quarter.", aud_Prize.length, Color.cyan, tutorBaldi.transform); break;
            }
        }
        if (spoopMode && quarter.activeSelf == true)
        {
            quarter.SetActive(false);
        }
        if ((notebooks == 4 && (mode == "story" || mode == "pizza" || mode == "free" || mode == "dark")) || (notebooks == 9 && mode == "endless"))
        {
            bigball.SetActive(true);
        }
        else if ((notebooks == maxNoteboos) & (mode == "story" || mode == "chaos"))
        {
            audioDevice.PlayOneShot(aud_AllNotebooks, 0.8f);
            FindObjectOfType<SubtitleManager>().AddChained2DSubtitle(escape, duration, colors);
            ESCAPEmusic.Play();
            if (baldiScrpt.isActiveAndEnabled)
            {
                if (slowerKriller == 1)
                {
                    baldiScript.baldiWait = 0.9f;
                }
                else
                {
                    baldiScript.baldiWait = 0.6f;
                }
                baldiScript.baldiTempAnger = 0f;
            }

        }
        else if ((notebooks == maxNoteboos) & (mode == "speedy"))
        {
            audioDevice.PlayOneShot(aud_AllNotebooks, 0.8f);
            FindObjectOfType<SubtitleManager>().AddChained2DSubtitle(escape, duration, colors);
            if (slowerKriller == 1)
            {
                baldiScript.baldiWait = 0.275f;
            }
            else
            {
                baldiScript.baldiWait = 0.175f;
            }
            baldiScript.baldiTempAnger = 0f;
        }
        else if ((notebooks == maxNoteboos) & (mode == "miko"))
        {
            entrance_2.Lower();
        }
        else if ((notebooks == maxNoteboos) & (mode == "triple" || mode == "dark" || mode == "free" || mode == "classic"))
        {
            audioDevice.PlayOneShot(aud_AllNotebooks, 0.8f);
            FindObjectOfType<SubtitleManager>().AddChained2DSubtitle(escape, duration, colors);
        }
        if (ESCAPEmusic.time < 20 && notebooks < 0)
        {
            ESCAPEmusic.time = 0;
            dwayneDebtTimer = ESCAPEmusic.clip.length;
            player.stamina += player.maxStamina * 0.6f;
        }
        else if (notebooks < 0)
        {
            ESCAPEmusic.time -= 20;
            dwayneDebtTimer += 20;
            player.stamina += player.maxStamina * 0.2f;
        }
        if (dwayneDebtTimer > ESCAPEmusic.clip.length)
        {
            dwayneDebtTimer = ESCAPEmusic.clip.length;
        }
        if (mode == "pizza")
        {
            pss.AddPoints(notebooks * 5, 2);
        }
        Time.timeScale = originalTimeScale;
    }

    private void IncreaseItemSelection()
    {
        itemSelected++;
        if (itemSelected > 3)
        {
            itemSelected = 0;
        }
        itemSelect.anchoredPosition = new Vector3(itemSelectOffset[itemSelected], -6.6f, 0f);
        UpdateItemName();
    }

    private void DecreaseItemSelection()
    {
        itemSelected--;
        if (this.itemSelected < 0)
        {
            this.itemSelected = 3;
        }
        itemSelect.anchoredPosition = new Vector3(itemSelectOffset[itemSelected], -6.6f, 0f);
        UpdateItemName();
    }

    private void UpdateItemSelection()
    {
        itemSelect.anchoredPosition = new Vector3(itemSelectOffset[itemSelected], -6.6f, 0f);
        UpdateItemName();
    }

    public void CollectItem(int item_ID)
    {
        if (item[itemSelected] == 0)
        {
            item[itemSelected] = item_ID;
            itemSlot[itemSelected].texture = itemHudTextures[item_ID];
        }
        else if (item[0] == 0)
        {
            item[0] = item_ID;
            itemSlot[0].texture = itemHudTextures[item_ID];
        }
        else if (item[1] == 0)
        {
            item[1] = item_ID;
            itemSlot[1].texture = itemHudTextures[item_ID];
        }
        else if (item[2] == 0)
        {
            item[2] = item_ID;
            itemSlot[2].texture = itemHudTextures[item_ID];
        }
        else if (item[3] == 0)
        {
            item[3] = item_ID;
            itemSlot[3].texture = itemHudTextures[item_ID];
        }
        else
        {
            item[itemSelected] = item_ID;
            itemSlot[itemSelected].texture = itemHudTextures[item_ID];
        }
        UpdateItemName();
    }

    private IEnumerator Teleporter()
    {
        debugMode = true;
        playerCharacter.enabled = false;
        playerCollider.enabled = false;
        player.teleporting = true;
        int teleports = Random.Range(12, 16);
        int teleportCount = 0;
        float baseTime = 0.2f;
        float currentTime = baseTime;
        float increaseFactor = 1.1f;
        while (teleportCount < teleports)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0f)
            {
                Teleport();
                teleportCount++;
                baseTime *= increaseFactor;
                currentTime = baseTime;
            }
            player.height = 4f;
            yield return null;
        }
        playerCharacter.enabled = true;
        player.teleporting = false;
        yield return new WaitForSeconds(1);
        playerCollider.enabled = true;
        debugMode = false;
    }

    public void Teleport()
    {
        Time.timeScale = 1;
        originalTimeScale = 1;
        AILocationSelector.GetNewTarget();
        player.transform.position = AILocationSelector.transform.position + Vector3.up * player.height;
        audioDevice.PlayOneShot(aud_Teleport);
        FindObjectOfType<SubtitleManager>().Add2DSubtitle("BREOW!", 0.5f, Color.white);
        if (PlayerPrefs.GetInt("shake", 1) == 1)
        {
            camScript.ShakeNow(new Vector3(0.3f, 0.15f, 0.3f), 1);
        }
    }

    private void UseItem()
    {
        if (item[itemSelected] == 0 || bsc.isActiveAndEnabled)
        {
            return;
        }
        tc.usedItem = true;
        RaycastHit hitInfo7;
        if (item[itemSelected] == 1)
        {
            audioDevice.PlayOneShot(crunch);
            tc.zestyEaten++;
            FindObjectOfType<SubtitleManager>().Add2DSubtitle("*CRUNCH*", crunch.length, Color.white);
            player.stamina += player.maxStamina * 2.05f;
            player.health += 15;
            if (mode == "pizza")
            {
                pss.AddPoints(25, 2);
            }
            ResetItem();
        }
        else if (item[itemSelected] == 2)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo) && ((hitInfo.collider.tag == "SwingingDoor") & (Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 10f)))
            {
                hitInfo.collider.gameObject.GetComponent<SwingingDoorScript>().LockDoor(15f);
                ResetItem();
            }
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out hitInfo) && ((hitInfo.collider.tag == "Door") & (Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 10f)))
            {
                hitInfo.collider.gameObject.GetComponent<DoorScript>().LockDoor(65f);
                ResetItem();
            }
            player.ResetGuilt("bullying", 2);
        }
        else if (item[itemSelected] == 3)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo2) && ((hitInfo2.collider.tag == "Door") & (Vector3.Distance(playerTransform.position, hitInfo2.transform.position) <= 10f)))
            {
                DoorScript component = hitInfo2.collider.gameObject.GetComponent<DoorScript>();
                if (component.DoorLocked)
                {
                    component.UnlockDoor();
                    audioDevice.PlayOneShot(aud_Unlock);
                    ResetItem();
                }
            }
        }
        else if (item[itemSelected] == 4)
        {
            Instantiate(bsodaSpray, player.transform.position, cameraTransform.rotation);
            ResetItem();
            player.ResetGuilt("drink", 1f);
            audioDevice.PlayOneShot(aud_Soda);
        }
        else if (item[itemSelected] == 5)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo3))
            {
                if ((hitInfo3.collider.name == "BSODAMachine") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
                {
                    ResetItem();
                    CollectItem(4);
                    audioDevice.PlayOneShot(aud_Paid);
                }
                else if ((hitInfo3.collider.name == "ZestyMachine") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
                {
                    ResetItem();
                    CollectItem(1);
                    audioDevice.PlayOneShot(aud_Paid);
                }
                else if ((hitInfo3.collider.name == "SitsMachine") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
                {
                    ResetItem();
                    CollectItem(22);
                    hitInfo3.collider.gameObject.GetComponent<Animator>().SetTrigger("byeeeee");
                    audioDevice.PlayOneShot(slideWhistle);
                    audioDevice.PlayOneShot(aud_Paid);
                }
                else if ((hitInfo3.collider.name == "RandomMachine") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
                {
                    ResetItem();
                    CollectItem(CollectItemExcluding(5, 18, 15, 16, 22, 24));
                    audioDevice.PlayOneShot(aud_Paid);
                }
                else if ((hitInfo3.collider.name == "PayPhone") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
                {
                    hitInfo3.collider.gameObject.GetComponent<TapePlayerScript>().Play();
                    ResetItem();
                    audioDevice.PlayOneShot(aud_Paid);
                }
            }
        }
        else if (item[itemSelected] == 6)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo4) && ((hitInfo4.collider.name == "TapePlayer") & (Vector3.Distance(playerTransform.position, hitInfo4.transform.position) <= 10f)))
            {
                hitInfo4.collider.gameObject.GetComponent<TapePlayerScript>().Play();
                player.ResetGuilt("bullying", 5);
                ResetItem();
            }
        }
        else if (item[itemSelected] == 7)
        {
            GameObject alarm = Instantiate(alarmClock, playerTransform.position, cameraTransform.rotation);
            alarm.GetComponent<AlarmClockScript>().baldi = baldiScrpt;
            alarm.GetComponent<AlarmClockScript>().miko = mikoScript;
            alarm.GetComponent<AlarmClockScript>().alger = algerScript;
            alarm.GetComponent<AlarmClockScript>().yellowFace = yellowFace;
            ResetItem();
        }
        else if (item[itemSelected] == 8)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo5) && ((hitInfo5.collider.tag == "Door") & (Vector3.Distance(playerTransform.position, hitInfo5.transform.position) <= 10f)))
            {
                hitInfo5.collider.gameObject.GetComponent<DoorScript>().SilenceDoor();
                ResetItem();
                audioDevice.PlayOneShot(aud_Spray);
            }
        }
        else if (item[itemSelected] == 9)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
            RaycastHit hitInfo6;
            if (Physics.Raycast(ray, out hitInfo6) && hitInfo6.collider.name == "Marty")
            {
                firstPrizeScript.GoCrazy(15.5f);
                ResetItem();
                player.stamina += player.maxStamina * 0.8f;
                player.ResetGuilt("bullying", 3);
            }
        }
        else if (item[itemSelected] == 10)
        {
            player.ActivateBoots();
            StartCoroutine(BootAnimation());
            ResetItem();
        }
        else if (item[itemSelected] == 11)
        {
            if (paninoAppleTimer > 0)
            {
                audioDevice.PlayOneShot(no);
                return;
            }
            baldiScript.timeToMove += 20f;
            if (algerScript.isActiveAndEnabled)
            {
                algerScript.timeToMove += 10;
            }
            player.walkSpeed += 2f;
            player.runSpeed += 3f;
            StartCoroutine(Freezed(20));
            if (PlayerPrefs.GetInt("shake", 1) == 1)
            {
                camScript.ShakeNow(new Vector3(0.1f, 0.01f, 0.1f), 5);
            }
            player.ResetGuilt("bullying", 3);
            ResetItem();
            audioDevice.PlayOneShot(aud_Switch);
        }
        else if (item[itemSelected] == 12)
        {
            StartCoroutine(Teleporter());
            ResetItem();
        }
        else if (item[itemSelected] == 13 && Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out hitInfo7))
        {
            if (hitInfo7.collider.name == "Alger (Hair Basics)")
            {
                principal.SetActive(value: false);
                audioDevice.PlayOneShot(aud_Switch);
                audioDevice.PlayOneShot(PRI_Ahh);
                if (baldiScript.isActiveAndEnabled)
                {
                    baldiScript.GetAngry(4.525f);
                    baldiScript.baldiWait -= 1.115f;
                    baldiScript.Hear(player.transform.position, 6f);
                }
                player.walkSpeed += 2f;
                player.runSpeed += 4f;
                ResetItem();
                if (PlayerPrefs.GetInt("shake", 1) == 1)
                {
                    camScript.ShakeNow(new Vector3(1f, 0.5f, 1f), 5);
                }
                algerKrilledByPlayer = true;
            }
            else if (hitInfo7.collider.name == "Alger (Alger's Basics)")
            {
                alger.SetActive(value: false);
                audioDevice.PlayOneShot(aud_Switch);
                audioDevice.PlayOneShot(PRI_Ahh);
                if (PlayerPrefs.GetInt("shake", 1) == 1)
                {
                    camScript.ShakeNow(new Vector3(1f, 0.5f, 1f), 5);
                }
                if (baldiScript.isActiveAndEnabled)
                {
                    baldiScript.GetAngry(4.525f);
                    baldiScript.baldiWait -= 0.615f;
                    baldiScript.Hear(player.transform.position, 6f);
                }
                if (mikoScript.isActiveAndEnabled)
                {
                    mikoScript.GetAngry(4.525f);
                    mikoScript.speed += 8f;
                    mikoScript.Hear(player.transform.position, 6f);
                }
            }
        }
        else if (item[itemSelected] == 14) // 3/3 uses
        {
            if (principalScript.isActiveAndEnabled)
            {
                principalScript.Whistled();
            }
            if (algerScript.isActiveAndEnabled)
            {
                algerScript.timeToMove = 0.1f;
                algerScript.baldiWait = 0.1f;
                algerScript.TargetPlayer();
                debugMode = false;
            }
            audioDevice.PlayOneShot(this.whistle);
            if (!principal.activeSelf)
            {
                if (!algerKrilledByPlayer)
                {
                    audioDevice.PlayOneShot(cantCome);
                    FindObjectOfType<SubtitleManager>().Add2DSubtitle("I can't come, I'm fucking dead!", cantCome.length, Color.cyan);
                }
                else
                {
                    audioDevice.PlayOneShot(killedMe);
                    FindObjectOfType<SubtitleManager>().Add2DSubtitle("You killed me, stupid idiot stupid!", killedMe.length, Color.cyan);
                }
            }
            this.ResetItem();
            this.CollectItem(15);
        }
        else if (item[itemSelected] == 15) // 2/3 uses
        {
            if (principalScript.isActiveAndEnabled)
            {
                principalScript.Whistled();
            }
            if (algerScript.isActiveAndEnabled)
            {
                algerScript.timeToMove = 0.1f;
                algerScript.baldiWait = 0.1f;
                algerScript.TargetPlayer();
                debugMode = false;
            }
            audioDevice.PlayOneShot(this.whistle);
            if (!principal.activeSelf)
            {
                if (!algerKrilledByPlayer)
                {
                    audioDevice.PlayOneShot(cantCome);
                }
                else
                {
                    audioDevice.PlayOneShot(killedMe);
                }
            }
            this.ResetItem();
            this.CollectItem(16);
        }
        else if (item[itemSelected] == 16) // 1/3 uses :/
        {
            if (principalScript.isActiveAndEnabled)
            {
                principalScript.Whistled();
            }
            if (algerScript.isActiveAndEnabled)
            {
                algerScript.timeToMove = 0.1f;
                algerScript.baldiWait = 0.1f;
                algerScript.TargetPlayer();
                debugMode = false;
            }
            audioDevice.PlayOneShot(this.whistle);
            if (!principal.activeSelf)
            {
                if (!algerKrilledByPlayer)
                {
                    audioDevice.PlayOneShot(cantCome);
                    FindObjectOfType<SubtitleManager>().Add2DSubtitle("I can't come, I'm fucking dead!", cantCome.length, Color.cyan);
                }
                else
                {
                    audioDevice.PlayOneShot(killedMe);
                    FindObjectOfType<SubtitleManager>().Add2DSubtitle("You killed me, stupid idiot stupid!", killedMe.length, Color.cyan);
                }
            }
            this.ResetItem();
        }
        else if (item[itemSelected] == 17) // apple product
        {
            audioDevice.PlayOneShot(crounch);
            FindObjectOfType<SubtitleManager>().Add2DSubtitle("*CROUNCH*", crunch.length, Color.white);
            if (notebooks >= 14)
            {
                playerScript.crazyAppleTimer += 12.5f;
            }
            else
            {
                playerScript.crazyAppleTimer += 5.5f;
            }
            if (Mathf.Round(Random.Range(1, 3)) == 2)
            {
                ResetItem();
            }
        }
        else if (item[itemSelected] == 18)
        {
            StartCoroutine(BeginTroll(8));
            audioDevice.PlayOneShot(aud_Switch);
            ResetItem();
        }
        else if (item[itemSelected] == 19)
        {
            objectItem[itemSelected].Objection(cameraTransform, audioDevice, objectionSound, camScript, objection, playerTransform, GetComponent<GameControllerScript>());
        }
        else if (item[itemSelected] == 20)
        {
            GameObject chalkey = Object.Instantiate(chalkCloud, playerTransform.position, Quaternion.identity);
            BoxCollider chalk = chalkey.GetComponent<BoxCollider>();
            Physics.IgnoreCollision(playerCharacter, chalk);
            ResetItem();
        }
        else if (item[itemSelected] == 21)
        {
            Instantiate(donut, donutShooter.transform.position, playerTransform.rotation);
            audioDevice.PlayOneShot(aud_Switch);
            ResetItem();
        }
        else if (item[itemSelected] == 22)
        {
            audioDevice.PlayOneShot(crunch);
            audioDevice.PlayOneShot(crounch);
            tc.esteEaten++;
            FindObjectOfType<SubtitleManager>().Add2DSubtitle("*CR(O)UNCH*", crunch.length, Color.green);
            player.stamina -= 20;
            player.health -= 5;
            if (mode == "pizza")
            {
                pss.AddPoints(-5, 2);
            }
        }
        else if (item[itemSelected] == 23)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
            Physics.Raycast(ray, out RaycastHit hitInfo8);
            FuzzyWindowScript windo = hitInfo8.collider.GetComponent<FuzzyWindowScript>();
            if (windo != null)
            {
                if (windo.mat != windo.windowClean)
                {
                    windo.Clean();
                    if (Random.Range(1, 7) >= 6)
                    {
                        ResetItem();
                    }
                    audioDevice.PlayOneShot(windowWipe);
                }
            }
        }
        else if (item[itemSelected] == 24)
        {
            player.infStamina = true;
            ResetItem();
            player.Invoke(nameof(player.DisableInfStamina), 5);
        }
    }

    public void Objection()
    {
        float num = (float)(Mathf.RoundToInt((this.playerTransform.rotation.eulerAngles.y - 90f) / 4f) * 4);
        this.audioDevice.PlayOneShot(this.objectionSound);
        Object.FindObjectOfType<SubtitleManager>().Add2DSubtitle("OBJECTION!", this.objectionSound.length, Color.white);
        if (PlayerPrefs.GetInt("shake", 1) == 1)
        {
            this.camScript.ShakeNow(new Vector3(0.2f, 0.05f, 0.2f), 20);
        }
        for (int i = 0; i < 3; i++)
        {
            Object.Instantiate<GameObject>(this.objection, this.playerTransform.position, Quaternion.Euler(0f, num, 0f));
            num += 90f;
        }
    }

    public int CollectItemExcluding(params int[] exclusions)
    {
        System.Random random = new System.Random();

        List<int> validNumbers = Enumerable.Range(1, itemHudTextures.Length - 1).Except(exclusions).ToList();

        int randomIndex = random.Next(validNumbers.Count);
        return validNumbers[randomIndex];
    }

    public IEnumerator Freezed(int frozen)
    {
        this.debugMode = true;
        if (mode == "pizza")
        {
            pss.AddPoints(65, 2);
        }
        if (mikoScript.isActiveAndEnabled)
        {
            mikoScript.disableTime += 10;
        }
        if (yellowey.isActiveAndEnabled)
        {
            yellowey.disableTime += 10;
        }
        yield return new WaitForSeconds(frozen);
        this.debugMode = false;
    }

    public IEnumerator BeginTroll(int time)
    {
        player.walkSpeed += 12;
        player.runSpeed += 12;
        yield return new WaitForSeconds(time);
        audioDevice.PlayOneShot(boowomp);
        player.runSpeed -= 20;
        player.walkSpeed -= 20;
        StartCoroutine(playerScript.ActivateTrolling(5));
        if (mode == "pizza")
        {
            pss.AddPoints(-2760, 5);
        }
    }

    private IEnumerator BootAnimation()
    {
        float time = 15f;
        float height = 375f;
        boots.gameObject.SetActive(value: true);
        player.walkSpeed -= 3;
        player.runSpeed -= 3;
        Vector3 localPosition;
        while (height > -375f)
        {
            height -= 375f * Time.deltaTime;
            time -= Time.deltaTime;
            localPosition = boots.localPosition;
            localPosition.y = height;
            boots.localPosition = localPosition;
            yield return null;
        }
        localPosition = boots.localPosition;
        localPosition.y = -375f;
        boots.localPosition = localPosition;
        boots.gameObject.SetActive(value: false);
        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        boots.gameObject.SetActive(value: true);
        player.walkSpeed += 4;
        player.runSpeed += 4;
        while (height < 375f)
        {
            height += 375f * Time.deltaTime;
            localPosition = boots.localPosition;
            localPosition.y = height;
            boots.localPosition = localPosition;
            yield return null;
        }
        localPosition = boots.localPosition;
        localPosition.y = 375f;
        boots.localPosition = localPosition;
        boots.gameObject.SetActive(value: false);
    }

    public void ResetItem()
    {
        if (!TestingItemsMode)
        {
            item[itemSelected] = 0;
            itemSlot[itemSelected].texture = itemTextures[0];
            UpdateItemName();
        }
    }

    public void LoseItem(int id)
    {
        if (!TestingItemsMode)
        {
            item[id] = 0;
            itemSlot[id].texture = itemTextures[0];
            UpdateItemName();
        }
    }

    public void UpdateItemName()
    {
        itemText.text = itemNames[item[itemSelected]];
        SpecificItemChecks();
    }

    void SpecificItemChecks()
    {
        if (item[itemSelected] == 19)
        {
            itemText.text = "<color=blue>Objection! (" + objectItem[itemSelected].objecUses + ")";
        }
        if (item[itemSelected] == 21)
        {
            donutShooter.SetBool("Up", true);
            if (PlayerPrefs.GetInt("heldItemShow", 0) == 1)
            {
                heldItem.SetActive(false);
            }
        }
        else
        {
            donutShooter.SetBool("Up", false);
            if (PlayerPrefs.GetInt("heldItemShow", 0) == 1)
            {
                heldItem.SetActive(true);
            }
        }
        if (item[itemSelected] == 23)
        {
            soapHeldParticle.SetActive(true);
        }
        else
        {
            soapHeldParticle.SetActive(false);
        }
        Texture2D tex = (Texture2D)itemTextures[item[itemSelected]];
        heldItem.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    public void ExitReached()
    {
        player.stamina += player.maxStamina * 0.5f;
        exitsReached++;
        if (mode == "miko")
        {
            if (exitsReached == 1)
            {
                audioDevice.PlayOneShot(aud_Switch, 0.8f);
                notebookCount.text = "1/4 Exits";
            }
            if (exitsReached == 2)
            {
                audioDevice.PlayOneShot(aud_Switch, 0.8f);
                notebookCount.text = "2/4 Exits";
            }
            if (exitsReached == 3)
            {
                audioDevice.PlayOneShot(aud_Switch, 0.8f);
                notebookCount.text = "3/4 Exits";
                entrance_2.Raise();
            }
        }
        else if (mode != "alger" && mode != "classic")
        {
            if (exitsReached == 0)
            {
                notebookCount.text = "0/4 Exits";
            }
            if (exitsReached == 1)
            {
                if (mode != "dark")
                {
                    RenderSettings.ambientLight = Color.cyan;
                }
                else
                {
                    RenderSettings.ambientLight = new Color(0, 0.1f, 0.1f, 1);
                }
                audioDevice.PlayOneShot(aud_Switch, 0.8f);
                notebookCount.color = Color.white;
                notebookCount.text = "1/4 Exits";
                if (mode == "pizza")
                {
                    pss.AddPoints(5, 0);
                }
            }
            if (exitsReached == 2)
            {
                if (mode != "dark")
                {
                    RenderSettings.ambientLight = Color.blue;
                }
                else
                {
                    RenderSettings.ambientLight = new Color(0, 0f, 0.1f, 1);
                }
                RenderSettings.fog = true;
                RenderSettings.fogColor = Color.blue;
                audioDevice.PlayOneShot(aud_Switch, 0.8f);
                notebookCount.text = "2/4 Exits";
                if (mode == "pizza")
                {
                    pss.AddPoints(10, 0);
                }
            }
            if (exitsReached == 3)
            {
                if (mode != "dark")
                {
                    RenderSettings.ambientLight = Color.red;
                }
                else
                {
                    RenderSettings.ambientLight = new Color(0.1f, 0, 0, 1);
                }
                RenderSettings.fogColor = Color.red;
                audioDevice.PlayOneShot(aud_Switch, 0.8f);
                notebookCount.text = "3/4 Exits";
                if (mode == "pizza")
                {
                    pss.AddPoints(15, 0);
                }
            }
            if (exitsReached == 4)
            {
                if (laps == 1)
                {
                    audioDevice.PlayOneShot(BAL_EscapeHair);
                    FindObjectOfType<SubtitleManager>().Add2DSubtitle("Escape from Hair BASICS!", BAL_EscapeHair.length, Color.cyan);
                    cameraNormal.fieldOfView += 22.5f;
                    player.walkSpeed += 2f;
                    player.runSpeed += 6f;
                }
                audioDevice.PlayOneShot(aud_Switch, 0.8f);
                if (mode != "dark")
                {
                    RenderSettings.ambientLight = Color.gray;
                }
                else
                {
                    RenderSettings.ambientLight = new Color(0.05f, 0.05f, 0.05f, 1);
                }
                RenderSettings.fogColor = Color.black;
                if (mode == "story" || mode == "dark" || (mode == "pizza" && laps == 1))
                {
                    baldiScript.baldiWait = 0.625f;
                }
                else if (this.mode == "speedy")
                {
                    baldiScript.baldiWait = 0.185f;
                    getOut.Play();
                    camScript.ShakeNow(new Vector3(0.1f, 0.1f, 0.1f), 2763);
                }
                else if (this.mode == "miko")
                {
                    mikoScript.speed = 40;
                }
                else if (this.mode == "triple")
                {
                    mikoScript.speed = 40;
                    baldiScript.baldiWait = 0.625f;
                    algerScript.baldiWait = 0.2f;
                    player.walkSpeed += 6f;
                    player.runSpeed += 2f;
                }
                entrance_4.Raise();
                notebookCount.text = "4/5 Exits";
                if (mode == "pizza")
                {
                    pss.AddPoints(25, 0);
                }
            }
        }
        else if (mode == "alger")
        {
            if (exitsReached == 1)
            {
                audioDevice.PlayOneShot(aud_Switch, 0.8f);
                notebookCount.text = "1/4 Exits";
            }
            if (exitsReached == 2)
            {
                audioDevice.PlayOneShot(aud_Switch, 0.8f);
                notebookCount.text = "2/4 Exits";
            }
            if (exitsReached == 3)
            {
                audioDevice.PlayOneShot(aud_Switch, 0.8f);
                notebookCount.text = "3/4 Exits";
            }
            if (exitsReached == 4)
            {
                audioDevice.PlayOneShot(aud_Switch, 0.8f);
                notebookCount.text = "4/5 Exits";
                entrance_4.Raise();
            }
        }
        else if (mode == "classic")
        {
            if (exitsReached == 0)
            {
                notebookCount.text = "0/4 Exits";
            }
            if (exitsReached == 1)
            {
                RenderSettings.ambientLight = Color.red;
                audioDevice.PlayOneShot(aud_Switch, 0.8f);
                notebookCount.color = Color.white;
                notebookCount.text = "1/4 Exits";
                audioDevice.clip = aud_MachineQuiet;
                audioDevice.loop = true;
                audioDevice.Play();
            }
            if (exitsReached == 2)
            {
                audioDevice.volume = 0.8f;
                audioDevice.PlayOneShot(aud_Switch, 0.8f);
                notebookCount.text = "2/4 Exits";
                audioDevice.clip = aud_MachineStart;
                audioDevice.loop = true;
                audioDevice.Play();
            }
            if (exitsReached == 3)
            {
                audioDevice.PlayOneShot(aud_Switch, 0.8f);
                notebookCount.text = "3/4 Exits";
                audioDevice.clip = aud_MachineRev;
                audioDevice.loop = false;
                audioDevice.Play();
                audioDevice.time = 0.02f;
            }
        }
    }

    public void FoundTreasure()
    {
        PauseGame();
        AudioListener.pause = false;
        pauseMenu.SetActive(false);
        disablePausing = true;
        audioDevice.PlayOneShot(congratulatation);
        print(congratulatation.length);
        Invoke("UnpauseGame", congratulatation.length);
        print("done");
        pss.AddPoints(3000, 6);
        disablePausing = false;
        Time.timeScale = originalTimeScale;
        gamePaused = false;
    }

    public void DespawnCrafters()
    {
        if (mode == "pizza")
        {
            pss.AddPoints(-50, 2);
        }
        crafters.SetActive(value: false);
        craftersWaitTime = 45;
    }

    public IEnumerator AngelEvent(bool giveItem, float time)
    {
        if (giveItem)
        {
            if (mode == "classic")
            {
                CollectItem(CollectItemExcluding(2, 3, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24));
            }
            else
            {
                if (principal.activeSelf)
                {
                    CollectItem(CollectItemExcluding(2, 3, 7, 8, 9, 10, 15, 16, 18, 21, 22, 24));
                }
                else
                {
                    CollectItem(CollectItemExcluding(2, 3, 7, 8, 9, 10, 13, 14, 15, 16, 18, 21, 22, 24));
                }
            }
        }
        guardianAngel.SetActive(false);
        yield return new WaitForSeconds(time);
        guardianAngel.SetActive(true);
    }

    public void Fliparoo()
    {
        player.height = 6f;
        player.fliparoo = 180f;
        player.flipaturn = -1f;
        Camera.main.GetComponent<CameraScript>().offset = new Vector3(0f, -1f, 0f);
    }

    public IEnumerator PaninoEat()
    {
        GameObject baldiApple = Instantiate(baldiApplePrefab);
        baldiApple.GetComponent<NavMeshAgent>().Warp(baldi.transform.position);
        baldi.SetActive(false);
        baldiApple.GetComponent<AudioSource>().PlayOneShot(bal_appleForMe);
        Animator animator = baldiApple.GetComponentInChildren<Animator>();
        animator.SetBool("EatApple", false);
        paninoAppleTimer = 15;
        FindObjectOfType<SubtitleManager>().Add3DSubtitle("No way, an apple product!", bal_appleForMe.length, Color.cyan, baldiApple.transform);
        yield return new WaitForSeconds(bal_appleForMe.length);
        animator.SetBool("EatApple", true);
        while (paninoAppleTimer > 0)
        {
            int rng;
            rng = Random.Range(1, 3);
            if (rng == 2)
            {
                baldiApple.GetComponent<AudioSource>().PlayOneShot(crunch);
                FindObjectOfType<SubtitleManager>().Add3DSubtitle("*CRUNCH*", Time.deltaTime, Color.white, baldiApple.transform);
            }
            else
            {
                baldiApple.GetComponent<AudioSource>().PlayOneShot(crounch);
                FindObjectOfType<SubtitleManager>().Add3DSubtitle("*CROUNCH*", Time.deltaTime, Color.white, baldiApple.transform);
            }
            rng = Random.Range(1, 30);
            if (rng == 6)
            {
                baldiApple.GetComponent<AudioSource>().PlayOneShot(bal_Yum);
                FindObjectOfType<SubtitleManager>().Add3DSubtitle("yes", bal_Yum.length, Color.white, baldiApple.transform);
            }
            rng = Random.Range(1, 300);
            if (rng == 6)
            {
                baldiApple.GetComponent<AudioSource>().PlayOneShot(run);
                FindObjectOfType<SubtitleManager>().Add3DSubtitle("run", run.length, Color.red, baldiApple.transform);
            }
            paninoAppleTimer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        baldi.transform.position = baldiApple.transform.position;
        Destroy(baldiApple);
        baldi.SetActive(true);
        yield break;
    }

    public void ClickOutNow()
    {
        getoutObject.SetActive(true);
        sliderClickOut.maxValue = at.clickery;
    }

    public bool craftersTime;

    private int speedBoost;
    private int extraStamina;
    private int slowerKriller;
    private int walkThrough;
    private int blockPath;
    private int infItem;
    private int jammers;

    public TMP_Text dwayneDebtTimerText;

    public bool TestingItemsMode;

    public MikoScript yellowey;

    public int secretsFound;

    public AudioClip slideWhistle;

    private bool algerKrilledByPlayer;

    public TMP_Text lapCount;

    public int old;

    public NotebookScript[] dwaynes;

    public GameObject modeTimer;
    public double time;

    public Animator donutShooter;
    public GameObject donut;

    public float paninoAppleTimer;
    
    public int amountOfExit;

    public GameObject eventText;
    public AudioClip riiiing;

    public float dwayneDebtTimer;

    public AudioClip darkSchool;

    public Animator fadeToWhite;

    public GameObject jammerMeter;

    public GameObject hud;

    public AgentTest at;

    public GameObject bigball;

    public AudioClip whistle;

    public float originalTimeScale;

    public MeshRenderer windowedWall;
    public Material broken;
    public AudioClip brokenWindow;

    public TMP_Text getout;

    public GameObject getoutObject;

    public Slider sliderClickOut;

    public float craftersWaitTime;

    public Timer timer;

    public Animator pizzaTimeTimer;

    public GameObject pizzaTimee;

    public CursorControllerScript cursorController;

    public PlayerScript player;

    public Transform playerTransform;

    public Transform cameraTransform;

    public AudioClip cantCome;
    public AudioClip killedMe;

    public Camera cameraNormal;

    private int cullingMask;

    public int math;

    public EntranceScript entrance_0;

    public EntranceScript entrance_1;

    public EntranceScript entrance_2;

    public EntranceScript entrance_3;

    public EntranceScript entrance_4;

    public GameObject chalkCloud;

    public GameObject heldItem;

    public GameObject soapHeldParticle;
    public AudioClip windowWipe;

    public float jammersTimer;

    public GameObject baldiTutor;

    public GameObject baldi;

    public BaldiScript baldiScrpt;
    public MikoScript mikoScript;
    public AlgerScript algerScript;

    public AudioClip aud_Prize;

    public AudioClip aud_PrizeMobile;

    public AudioClip aud_AllNotebooks;

    public GameObject principal;

    public GameObject crafters;

    public GameObject yellowFace;

    public GameObject guardianAngel;

    public GameObject playtime;

    public PlaytimeScript playtimeScript;

    public GameObject gottaSweep;

    public GameObject bully;

    public GameObject firstPrize;

    public GameObject TestEnemy;

    public GameObject baba;

    public GameObject algerNull;

    public GameObject devin;

    public FirstPrizeScript firstPrizeScript;

    public GameObject quarter;

    public AudioSource tutorBaldi;

    public RectTransform boots;

    public string mode;

    public int notebooks;

    public GameObject[] notebookPickups;

    public int failedNotebooks;

    public bool spoopMode;

    public bool finaleMode;

    public bool debugMode;

    public bool mouseLocked;

    public int exitsReached;

    public int itemSelected;

    public int[] item;

    public RawImage[] itemSlot;

    public string[] itemNames;

    public TMP_Text itemText;

    public Texture[] itemTextures;
    public Texture[] itemHudTextures;

    public GameObject bsodaSpray;
    public GameObject objection;

    public GameObject alarmClock;

    public TMP_Text notebookCount;

    public Material wall;

    public GameObject pauseMenu;

    public GameObject highScoreText;

    public GameObject warning;

    public GameObject reticle;

    public RectTransform itemSelect;

    private int[] itemSelectOffset;

    public bool gamePaused;

    private bool learningActive;

    private float gameOverDelay;

    private float CHAOStimer;

    public AudioSource audioDevice;

    public AudioClip aud_Soda;

    public AudioClip no;

    public AudioClip aud_Spray;
    public AudioClip objectionSound;

    public AudioClip aud_buzz;
    public AudioClip aud_chrisAAAAA;
    public AudioClip algerKrilledYouHaha;

    public AudioClip aud_Hang;

    public AudioClip aud_MachineQuiet;

    public AudioClip aud_MachineStart;

    public AudioClip aud_MachineRev;

    public AudioClip aud_MachineLoop;

    public AudioClip aud_Switch;

    public AudioSource schoolMusic;

    public AudioSource learnMusic;

    public AudioSource pizzaTimeMusic;
    public AudioSource lap2Music;
    public AudioSource wwnMusic;

    public AudioSource crazyAppleMusic;

    public AudioClip aud_Paid;

    public BaldiScript baldiScript;

    public AudioSource ESCAPEmusic;

    public ObjectionItem[] objectItem;
    private int objecUsesinit;

    public AudioClip bal_appleForMe;

    public AudioClip bal_Yum;
    public AudioClip run;

    public AudioClip crounch;
    public AudioClip crunch;

    public AudioClip PRI_Ahh;

    public GameObject baldiApplePrefab;

    public AudioClip aud_Unlock;
    public AudioClip aud_Lock;

    public CharacterController playerCharacter;

    public BossControllerScript bsc;

    public Collider playerCollider;

    public AILocationSelectorScript AILocationSelector;

    public AudioClip aud_Teleport;

    public DedTextScript ded;

    public PlayerScript playerScript;

    public PrincipalScript principalScript;

    public AudioClip BAL_EscapeHair;

    public AudioSource getOut;

    public Animator schooldarkenthingy;

    public GameObject locationTextObj;

    public TMP_Text locationText;

    public int hour = System.DateTime.Now.Hour;

    public int minute = System.DateTime.Now.Minute;

    public AudioClip pillarHit;

    public GameObject normalItemLayout;
    public GameObject speedyItemLayout;
    public GameObject mikoItemLayout;
    public GameObject algerItemLayout;
    public GameObject stealthyItemLayout;

    public VideoPlayer[] tutorals;

    public VideoClip panic;

    public GameObject miko;
    public GameObject alger;

    public AudioClip boowomp;

    public CameraScript camScript;

    public int maxNoteboos;

    public GameObject exitIndicator;

    public GameObject asdaa;

    public int maxItems;

    public PizzaScoreScript pss;
    public GameObject psgo;

    public float scoreDecayTimer;

    public AudioClip getInPortal;
    public GameObject escapeCollect;
    public bool disablePausing;

    public int laps;

    public bool gameOverPlayed;

    public GameObject toppins;

    public PizzafaceScript pizzaface;

    public NavMeshAgent baldiAgent;
    public NavMeshAgent princAgent;
    public NavMeshAgent ballAgent;
    public NavMeshAgent craftAgent;

    public MeshRenderer[] secretWalls;
    public GameObject secrets;

    public Material secretWall2;

    public GameObject lap2Portal;

    public AudioClip congratulatation;
}
