using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using FluidMidi;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameControllerScriptSecret : MonoBehaviour
{
    public TrophyCollectingScript tc;

    private int pCounter;

    public GameControllerScriptSecret()
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
        if (SceneManager.GetActiveScene().name == "Luck")
        {
            disablePausing = true;
        }
        else
        {
            LockMouse();
        }
        tc = GetComponent<TrophyCollectingScript>();
        maxItems = 4;
        math = PlayerPrefs.GetInt("math", 1);
        speedBoost = PlayerPrefs.GetInt("speedBoost", 0);
        extraStamina = PlayerPrefs.GetInt("extraStamina", 0);
        slowerKriller = PlayerPrefs.GetInt("slowerKriller", 0);
        walkThrough = PlayerPrefs.GetInt("walkThrough", 0);
        blockPath = PlayerPrefs.GetInt("blockPath", 0);
        infItem = PlayerPrefs.GetInt("infItem", 0);
        jammers = PlayerPrefs.GetInt("jammer", 0);
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
        UpdateNotebookCount();
        /*if (dm != null)
        {
            dm.largeText = $"{mode.ToUpper()} Mode";
        }*/
        itemSelected = 0;
        gameOverDelay = 0.5f;
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
        if (slowerKriller == 1 && mode != "panino")
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

    public void LoseNotebooks(int amount, float multiply)
    {
        notebooks -= amount;
        UpdateNotebookCount();
        if (notebooks < 0)
        {
            notebooks *= Mathf.RoundToInt(multiply);
            UpdateNotebookCount();
        }
    }

    public void ItsPizzaTime()
    {
        audioDevice.PlayOneShot(pillarHit);
        fadeToWhite.SetTrigger("pisstime");
        pizzaTimee.SetActive(true);
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
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && Time.timeScale != 0f && mode != "zombie")
            {
                DecreaseItemSelection();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Time.timeScale != 0f && mode != "zombie")
            {
                IncreaseItemSelection();
            }
            if (Time.timeScale != 0f && mode != "zombie")
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
                if (camScript.character.name == "Alger" || PlayerPrefs.GetInt("duplicatedBalls", 0) == 1)
                {
                    Application.Quit();
                    return;
                }
                Time.timeScale = 1f;
                if (PlayerPrefs.GetInt("fastRestart", 0) == 1)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    return;
                }
                SceneManager.LoadScene("GameOver");
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            pCounter++;
            if (pCounter >= 50)
            {
                PlayerPrefs.SetInt("pSecretFound", 1);
                SceneManager.LoadScene("p");
            }
        }
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
            else if (charac == "Evil Leafy")
            {
                audioDevice.PlayOneShot(bfdiScream);
                FindObjectOfType<SubtitleManager>().Add2DSubtitle("AAAAAAAAAAHHHHH!!!", bfdiScream.length, Color.white);
                gameOverDelay = 1;
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
        /*if (dm != null)
        {
            dm.size = notebooks;
            dm.maxSize = maxNoteboos;
        }*/
        int highScoreBotenook = PlayerPrefs.GetInt("HighBooks");
        if (mode != "endless" && SceneManager.GetActiveScene().name != "Luck")
        {
            notebookCount.text = $"{notebooks}/{maxNoteboos} Dwaynes";
        }
        else if (SceneManager.GetActiveScene().name != "Luck")
        {
            notebookCount.text = $"{notebooks}/{highScoreBotenook} H.S. Dwaynes";
        }
        else
        {
            notebookCount.text = $"{notebooks} left";
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

    public void SpawnEvilLeafy()
    {
        if (evilLeafy.activeSelf)
        {
            evilLeafy.GetComponent<EvilLeafyScript>().baldiWait -= 0.2f;
        }
        baldiTutor.SetActive(value: false);
        principal.SetActive(value: false);
        crafters.SetActive(false);
        gottaSweep.SetActive(false);
        bully.SetActive(false);
        firstPrize.SetActive(false);
        guardianAngel.SetActive(false);
        craftersTime = false;
        crafters.SetActive(false);
        schoolMusic.gameObject.SetActive(false);
        evilLeafy.SetActive(true);
        math = 0;
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
    }

    public void SpawnWithChance(GameObject character, float minRange, float maxRange, float TargetNum, bool integer = true, bool targetIsKey = true)
    {
        float rng = Random.Range(minRange, maxRange); 
        
        if (targetIsKey)
        {
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
        }
        else
        {
            if (Mathf.FloorToInt(rng) != TargetNum)
            {
                character.SetActive(true);
                if (character == crafters)
                {
                    craftersTime = true;
                }
            }
            if (character == crafters && Mathf.FloorToInt(rng) == TargetNum)
            {
                craftersTime = false;
            }
        }

        print($"{character} got a {minRange} in {maxRange} chance, and got {Mathf.FloorToInt(rng)}. Target is {TargetNum}");
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        AudioListener.pause = false;
        SceneManager.LoadScene(scene.name);
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
        if (mode == "zombie")
        {
            return;
        }
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
        if (item_ID == 5)
        {
            tc.QuarterCheck();
        }
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
        if (item[itemSelected] == 0 || bsc.isActiveAndEnabled || mode == "zombie" || camScript.FuckingDead)
        {
            return;
        }
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
            tc.usedItem = true;
        }
        else if (item[itemSelected] == 2)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo) && ((hitInfo.collider.tag == "SwingingDoor") & (Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 10f)))
            {
                hitInfo.collider.gameObject.GetComponent<SwingingDoorScript>().LockDoor(15f);
                ResetItem();
                tc.usedItem = true;
            }
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out hitInfo) && ((hitInfo.collider.tag == "Door") & (Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 10f)))
            {
                hitInfo.collider.gameObject.GetComponent<DoorScript>().LockDoor(65f);
                ResetItem();
                tc.usedItem = true;
            }
        }
        else if (item[itemSelected] == 3)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo2) && ((hitInfo2.collider.tag == "Door") & (Vector3.Distance(playerTransform.position, hitInfo2.transform.position) <= 10f)))
            {
                DoorScript component = hitInfo2.collider.gameObject.GetComponent<DoorScript>();
                PrisonDoor component2 = hitInfo2.collider.gameObject.GetComponent<PrisonDoor>();
                if (component != null)
                {
                    if (!component.johnDoor)
                    {

                        component.UnlockDoor();
                        audioDevice.PlayOneShot(aud_Unlock);
                        ResetItem();
                        tc.usedItem = true;
                    }
                }
                if (component2 != null)
                {
                    if (component2.openable)
                    {
                        component2.SetClicks(1);
                    }
                }
            }
        }
        else if (item[itemSelected] == 4)
        {
            Instantiate(bsodaSpray, player.transform.position, Quaternion.Euler(0f, (cameraTransform.rotation.eulerAngles.y), 0f));
            ResetItem();
            player.ResetGuilt("drink", 1f);
            audioDevice.PlayOneShot(aud_Soda);
            tc.usedItem = true;
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
                    tc.usedItem = true;
                }
                else if ((hitInfo3.collider.name == "ZestyMachine") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
                {
                    ResetItem();
                    CollectItem(1);
                    audioDevice.PlayOneShot(aud_Paid);
                    tc.usedItem = true;
                }
                else if ((hitInfo3.collider.name == "SitsMachine") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
                {
                    ResetItem();
                    CollectItem(22);
                    hitInfo3.collider.gameObject.GetComponent<Animator>().SetTrigger("byeeeee");
                    audioDevice.PlayOneShot(slideWhistle);
                    audioDevice.PlayOneShot(aud_Paid);
                    tc.usedItem = true;
                }
                else if ((hitInfo3.collider.name == "RandomMachine") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
                {
                    ResetItem();
                    CollectItem(CollectItemExcluding(5, 18, 15, 16, 22, 24, 25, 27));
                    audioDevice.PlayOneShot(aud_Paid);
                    tc.usedItem = true;
                }
                else if ((hitInfo3.collider.name == "PayPhone") & (Vector3.Distance(playerTransform.position, hitInfo3.transform.position) <= 10f))
                {
                    hitInfo3.collider.gameObject.GetComponent<TapePlayerScript>().Play();
                    ResetItem();
                    audioDevice.PlayOneShot(aud_Paid);
                    tc.usedItem = true;
                }
            }
        }
        else if (item[itemSelected] == 6)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo4) && ((hitInfo4.collider.name == "TapePlayer") & (Vector3.Distance(playerTransform.position, hitInfo4.transform.position) <= 10f)))
            {
                ResetItem();
                hitInfo4.collider.gameObject.GetComponent<TapePlayerScript>().Play();
                tc.usedItem = true;
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
            tc.usedItem = true;
        }
        else if (item[itemSelected] == 8)
        {
            ResetItem();
            Instantiate(squee, playerTransform.position, Quaternion.identity);
            audioDevice.PlayOneShot(aud_Spray);
            if (baldiScript.isActiveAndEnabled)
            {
                baldiScript.FindSquees();
            }
            if (mikoScript.isActiveAndEnabled)
            {
                mikoScript.FindSquees();
            }
            if (algerScript.isActiveAndEnabled)
            {
                algerScript.FindSquees();
            }
            tc.usedItem = true;
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
                tc.usedItem = true;
            }
        }
        else if (item[itemSelected] == 10)
        {
            player.ActivateBoots();
            StartCoroutine(BootAnimation());
            ResetItem();
            tc.usedItem = true;
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
            ResetItem();
            audioDevice.PlayOneShot(aud_Switch);
            tc.usedItem = true;
        }
        else if (item[itemSelected] == 12)
        {
            StartCoroutine(Teleporter());
            ResetItem();
            tc.usedItem = true;
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
                    baldiScript.baldiWait -= 0.215f;
                    baldiScript.Hear(player.transform.position, 6f);
                }
                player.walkSpeed += 6f;
                player.runSpeed += 6f;
                ResetItem();
                if (PlayerPrefs.GetInt("shake", 1) == 1)
                {
                    camScript.ShakeNow(new Vector3(1f, 0.5f, 1f), 5);
                }
                algerKrilledByPlayer = true;
                tc.usedItem = true;
                StartCoroutine(SpawnAlgerAfter(130));
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
                tc.usedItem = true;
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
            tc.usedItem = true;
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
            tc.usedItem = true;
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
            tc.usedItem = true;
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
            tc.usedItem = true;
        }
        else if (item[itemSelected] == 18)
        {
            StartCoroutine(BeginTroll(5));
            audioDevice.PlayOneShot(aud_Switch);
            ResetItem();
            tc.usedItem = true;
        }
        else if (item[itemSelected] == 19)
        {
            objectItem[itemSelected].Objection(cameraTransform, audioDevice, objectionSound, camScript, objection, playerTransform, GetComponent<GameControllerScript>());
            tc.usedItem = true;
        }
        else if (item[itemSelected] == 20)
        {
            GameObject chalkey = Object.Instantiate(chalkCloud, playerTransform.position, Quaternion.identity);
            BoxCollider chalk = chalkey.GetComponent<BoxCollider>();
            Physics.IgnoreCollision(playerCharacter, chalk);
            ResetItem();
            tc.usedItem = true;
        }
        else if (item[itemSelected] == 21)
        {
            Instantiate(donut, donutShooter.transform.position, playerTransform.rotation);
            audioDevice.PlayOneShot(aud_Switch);
            ResetItem();
            tc.usedItem = true;
        }
        else if (item[itemSelected] == 22)
        {
            audioDevice.PlayOneShot(crunch);
            audioDevice.PlayOneShot(crounch);
            tc.esteEaten++;
            FindObjectOfType<SubtitleManager>().Add2DSubtitle("*CR(O)UNCH*", crunch.length, Color.green);
            player.stamina -= 5;
            player.health -= 5;
            if (mode == "pizza")
            {
                pss.AddPoints(-5, 2);
            }
            tc.usedItem = true;
        }
        else if (item[itemSelected] == 23)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
            Physics.Raycast(ray, out RaycastHit hitInfo8, 10);
            if (hitInfo8.transform == null)
            {
                return;
            }
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
                    tc.usedItem = true;
                }
            }
        }
        else if (item[itemSelected] == 24)
        {
            player.infStamina = true;
            ResetItem();
            player.Invoke(nameof(player.DisableInfStamina), 5);
            tc.usedItem = true;
        }
        else if (item[itemSelected] == 26)
        {
            PlayerPrefs.SetInt("duplicatedBalls", 1);
            PlayerPrefs.Save();
            item[0] = 26;
            item[1] = 26;
            item[2] = 26;
            item[3] = 26;
            for (int i = 0; i < 30; i++)
            {
                Ray ray = Camera.main.ScreenPointToRay(Random.rotation.eulerAngles);
                Physics.Raycast(ray, out RaycastHit hitInfo, 20);
                if (hitInfo.transform != null && hitInfo.transform.GetComponent<MeshRenderer>() != null)
                {
                    hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material = ballWall;
                }
            }
            for (int i = 0; i < notebookPickups.Length; i++)
            {
                notebookPickups[i].SetActive(false);
            }
            pizzaTimeMusic.Play();
            pizzaTimeMusic.time = Random.Range(20, 180);
            baldi.SetActive(true);
            baldiScript.timeToMove = 0;
            baldiScript.baldiWait = 0;
            baldiScript.TargetPlayer();
            principal.SetActive(true);
            principalScript.angry = true;
            principalScript.summon = true;
            camScript.ShakeNow(new Vector3(0.2f, 0.2f, 0.2f), 276300);
            tc.usedItem = true;
        }
        else if (item[itemSelected] == 27)
        {
            GameObject a = Instantiate(ubrSpray, playerTransform.position + Vector3.up, Quaternion.Euler(0f, (cameraTransform.rotation.eulerAngles.y), 0f));
            a.transform.name = "UbrSpray(Clone)";
            audioDevice.PlayOneShot(aud_Spray);
            if (Random.Range(1, 10) != 2)
            {
                ResetItem();
            }
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

    public IEnumerator SpawnAlgerAfter(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        principal.SetActive(true);
        principalScript.angry = false;
    }

    public IEnumerator BeginTroll(int time)
    {
        player.walkSpeed += 12;
        player.runSpeed += 12;
        yield return new WaitForSeconds(time);
        audioDevice.PlayOneShot(boowomp);
        player.runSpeed = 6;
        player.walkSpeed = 4;
        StartCoroutine(playerScript.ActivateTrolling(5));
        if (mode == "pizza")
        {
            pss.AddPoints(-2760, 5);
        }
        tc.GetTrophy(19);
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
        if (mode == "zombie")
        {
            return;
        }
        if (!TestingItemsMode)
        {
            item[itemSelected] = 0;
            itemSlot[itemSelected].texture = itemTextures[0];
            UpdateItemName();
        }
    }

    public void LoseItem(int id)
    {
        if (mode == "zombie")
        {
            return;
        }
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

    public void CurseOfRa()
    {
        if (curseOfRaActive)
        {
            return;
        }
        curseOfRaMusic.Play();
        curseOfRaActive = true;
    }

    IEnumerator CurseOfRaLogic()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        curseOfRaTime += Time.deltaTime / 2;
        if (Random.Range(1, Mathf.RoundToInt(900 / curseOfRaTime)) <= 2)
        {
            Ray ray = Camera.main.ScreenPointToRay(Random.rotation.eulerAngles);
            Physics.Raycast(ray, out RaycastHit hitInfo, 20);
            if (hitInfo.transform != null && hitInfo.transform.GetComponent<MeshRenderer>() != null)
            {
                hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material = sand;
            }
            sandUI.color = new Color(1, 1, 1, sandUI.color.a + Random.Range(0.0f, 0.01f));
            camScript.ShakeNow(new Vector3(0.1f, 0.1f, 0.1f), 2);
            if (sandUI.color.a >= 0.85f)
            {
                player.health = 0;
            }
        }
    }

    public void UndoCurse()
    {
        curseOfRaMusic.Stop();
        curseOfRaActive = false;
        if (sandUI.color.a >= 0.625)
        {
            tc.GetTrophy(23);
        }
        sandUI.color = new Color(1, 1, 1, 0);
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
        else if (mode != "alger" && mode != "classic" && mode != "zombie")
        {
            if (exitsReached == 0)
            {
                notebookCount.text = "0/4 Exits";
            }
            if (exitsReached == 1)
            {
                RenderSettings.ambientLight = Color.cyan;
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
                RenderSettings.ambientLight = Color.blue;
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
                RenderSettings.ambientLight = Color.red;
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
                RenderSettings.ambientLight = Color.gray;
                RenderSettings.fogColor = Color.black;
                if (mode == "story" || (mode == "pizza" && laps == 1))
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
        else if (mode == "alger" || mode == "zombie")
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

    public IEnumerator KillZombies()
    {
        killZombiesToWin.SetActive(true);
        yield return new WaitForSeconds(3);
        killZombiesToWin.SetActive(false);
    }

    public void DespawnCrafters()
    {
        crafters.SetActive(value: false);
        craftersWaitTime = 45;
    }

    public IEnumerator AngelEvent(bool giveItem, float time)
    {
        if (giveItem)
        {
            if (mode == "classic")
            {
                CollectItem(CollectItemExcluding(2, 3, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 25, 26, 27));
            }
            else
            {
                if (principal.activeSelf)
                {
                    CollectItem(CollectItemExcluding(2, 3, 7, 8, 9, 10, 15, 16, 18, 21, 22, 24, 25, 26, 27));
                }
                else
                {
                    CollectItem(CollectItemExcluding(2, 3, 7, 8, 9, 10, 13, 14, 15, 16, 18, 21, 22, 24, 25, 26, 27));
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
            yield return new WaitForSeconds(0.01667f); // 60fps basically
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

    public int speedBoost;
    public int extraStamina;
    public int slowerKriller;
    public int walkThrough;
    public int blockPath;
    public int infItem;
    public int jammers;

    public TMP_Text dwayneDebtTimerText;

    public bool TestingItemsMode;

    public MikoScript yellowey;

    public int secretsFound;

    public AudioClip slideWhistle;

    public GameObject killZombiesToWin;

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

    public GameObject[] playerHudStuff;

    public GameObject paninisSlider;

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

    public Transform evilPlayerTransform;

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

    public GameObject squee;

    public GameObject heldItem;

    public GameObject soapHeldParticle;
    public AudioClip windowWipe;

    public float jammersTimer;

    public GameObject baldiTutor;

    public GameObject baldi;

    public GameObject baldiPlayer;

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

    public GameObject zombie;

    public GameObject evilLeafy;

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

    public GameObject[] stuffNoZombieMode;
    public GameObject[] stuffYesZombieMode;

    public TMP_Text itemText;

    public Texture[] itemTextures;
    public Texture[] itemHudTextures;

    public GameObject bsodaSpray;
    public GameObject objection;

    public GameObject alarmClock;

    public TMP_Text notebookCount;

    public Material wall;
    public Material sand;
    public Material ballWall;

    public GameObject pauseMenu;

    public PaninoTV paninoTv;

    public GameObject highScoreText;

    public GameObject warning;

    public GameObject reticle;

    public RectTransform itemSelect;

    private int[] itemSelectOffset;

    public bool gamePaused;

    private bool learningActive;

    bool curseOfRaActive;
    float curseOfRaTime;

    private float gameOverDelay;

    public AudioSource audioDevice;

    public AudioClip aud_Soda;

    public AudioClip no;

    public AudioClip aud_Spray;
    public AudioClip objectionSound;

    public AudioClip aud_buzz;
    public AudioClip aud_chrisAAAAA;
    public AudioClip algerKrilledYouHaha;
    public AudioClip bfdiScream;

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
    public AudioSource curseOfRaMusic;

    public AudioSource ambient;

    public AudioSource crazyAppleMusic;

    public AudioClip aud_Paid;

    public BaldiScript baldiScript;

    public BaldiPlayerScript baldiPlayerScript;

    public AudioSource ESCAPEmusic;
    public AudioClip BESTESCAPE;

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
    public GameObject zombieItemLayout;

    public VideoPlayer[] tutorals;

    public GameObject pharohsWall;

    public Image sandUI;

    public VideoClip panic;

    public GameObject locust;

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

    public GameObject ubrSpray;

    public GameObject retroCanvas;

    public GameObject A;
}