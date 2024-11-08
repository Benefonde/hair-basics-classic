using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameControllerScriptPuzzleMode : MonoBehaviour
{
    public TrophyCollectingScript tc;

    public Material night;
    public Material day;

    public int yellowFaceOn;

    public GameControllerScriptPuzzleMode()
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
        math = PlayerPrefs.GetInt("math", 1);
        speedBoost = PlayerPrefs.GetInt("speedBoost", 0);
        extraStamina = PlayerPrefs.GetInt("extraStamina", 0);
        slowerKriller = PlayerPrefs.GetInt("slowerKriller", 0);
        walkThrough = PlayerPrefs.GetInt("walkThrough", 0);
        blockPath = PlayerPrefs.GetInt("blockPath", 0);
        infItem = PlayerPrefs.GetInt("infItem", 0);
        yellowFaceOn = PlayerPrefs.GetInt("yellow", 0);
        originalTimeScale = 1;
        Time.timeScale = originalTimeScale;
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 0.5f);
        hour = System.DateTime.Now.Hour;
        minute = System.DateTime.Now.Minute;
        cullingMask = cameraNormal.cullingMask;
        audioDevice = GetComponent<AudioSource>();
        mode = PlayerPrefs.GetString("CurrentMode"); 
        if (PlayerPrefs.GetInt("timer") == 1)
        {
            modeTimer.SetActive(true);
        }
        math = 0;
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
        if (extraStamina == 1)
        {
            player.maxStamina = 200;
        }
        if (speedBoost == 1)
        {
            player.walkSpeed *= 1.7f;
            player.runSpeed *= 1.7f;
        }
        LockMouse();
        itemSelected = 0;
        gameOverDelay = 0.5f;
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
        return false;
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            time += Time.unscaledDeltaTime;
            System.TimeSpan timee = System.TimeSpan.FromSeconds(time);
            modeTimer.GetComponent<TMP_Text>().text = string.Format("{0:00}:{1:00}:{2:000}", timee.Minutes, timee.Seconds, timee.Milliseconds);
        }
        if (curseOfRaActive && !gamePaused)
        {
            StartCoroutine(CurseOfRaLogic());
        }
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
        else if (Time.timeScale != 0f)
        {
            Time.timeScale = 0f;
        }
        if (player.gameOver)
        {
            GameOverStart();
            gameOverDelay -= Time.unscaledDeltaTime * 0.5f;
            Time.timeScale = 0;
            if (gameOverDelay < 0)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("GameOver");
            }
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            player.walkSpeed *= 1.25f;
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
        }

        gameOverPlayed = true;
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

    public void PauseGame()
    {
        AudioListener.pause = true;
        UnlockMouse();
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        FindObjectOfType<SubtitleManager>().localTimeScale = 0;
        gamePaused = true;
        pauseMenu.SetActive(value: true);
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

    public void FadeToWhite()
    {
        SetTime(0);
        fadeToWhite.updateMode = AnimatorUpdateMode.UnscaledTime;
        fadeToWhite.SetTrigger("fade");
    }

    public void GetAngry(float value)
    {
        if (baldiScrpt.isActiveAndEnabled)
        {
            baldiScrpt.GetAngry(value);
        }
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
        if (item[itemSelected] == 0)
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
                player.ResetGuilt("bullying", 2);
            }
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out hitInfo) && ((hitInfo.collider.tag == "Door") & (Vector3.Distance(playerTransform.position, hitInfo.transform.position) <= 10f)))
            {
                hitInfo.collider.gameObject.GetComponent<DoorScript>().LockDoor(65f);
                ResetItem();
                tc.usedItem = true;
                player.ResetGuilt("bullying", 2);
            }
        }
        else if (item[itemSelected] == 3)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo2) && ((hitInfo2.collider.tag == "Door") & (Vector3.Distance(playerTransform.position, hitInfo2.transform.position) <= 10f)))
            {
                DoorScript component = hitInfo2.collider.gameObject.GetComponent<DoorScript>();
                if (component.DoorLocked && !component.johnDoor)
                {
                    component.UnlockDoor();
                    audioDevice.PlayOneShot(aud_Unlock);
                    ResetItem();
                    tc.usedItem = true;
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
                    CollectItem(CollectItemExcluding(5, 18, 15, 16, 22, 24, 25));
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
                hitInfo4.collider.gameObject.GetComponent<TapePlayerScript>().Play();
                player.ResetGuilt("bullying", 5);
                ResetItem();
                tc.usedItem = true;
            }
        }
        else if (item[itemSelected] == 7)
        {
            GameObject alarm = Instantiate(alarmClock, playerTransform.position, cameraTransform.rotation);
            alarm.GetComponent<AlarmClockScript>().baldi = baldiScrpt;
            alarm.GetComponent<AlarmClockScript>().miko = mikoScript;
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
            tc.usedItem = true;
        }
        else if (item[itemSelected] == 9)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
            RaycastHit hitInfo6;
            if (Physics.Raycast(ray, out hitInfo6) && hitInfo6.collider.name == "Marty")
            {
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
                tc.usedItem = true;
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
        else if (item[itemSelected] == 17) // apple product
        {
            audioDevice.PlayOneShot(crounch);
            FindObjectOfType<SubtitleManager>().Add2DSubtitle("*CROUNCH*", crunch.length, Color.white);
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
        if (mikoScript.isActiveAndEnabled)
        {
            mikoScript.disableTime += 10;
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
            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.name == "Wall" || hitInfo.transform.name == "Ceiling" || hitInfo.transform.name == "Floor")
                {
                    hitInfo.transform.gameObject.GetComponent<MeshRenderer>().material = sand;
                }
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

    private int speedBoost;
    private int extraStamina;
    private int slowerKriller;
    private int walkThrough;
    private int blockPath;
    private int infItem;
    private int jammers;

    public bool TestingItemsMode;

    public AudioClip slideWhistle;

    public GameObject modeTimer;
    public double time;

    public Animator donutShooter;
    public GameObject donut;

    public float paninoAppleTimer;

    public AudioClip darkSchool;

    public Animator fadeToWhite;

    public GameObject hud;

    public AgentTest at;

    public GameObject bigball;

    public AudioClip whistle;

    public float originalTimeScale;

    public CursorControllerScript cursorController;

    public PlayerScript player;

    public Transform playerTransform;

    public Transform cameraTransform;

    public Camera cameraNormal;

    private int cullingMask;

    public int math;

    public GameObject chalkCloud;

    public GameObject squee;

    public GameObject heldItem;

    public GameObject soapHeldParticle;
    public AudioClip windowWipe;

    public GameObject baldi;

    public BaldiScript baldiScrpt;
    public MikoScript mikoScript;

    public GameObject principal;

    public GameObject crafters;

    public GameObject guardianAngel;

    public GameObject gottaSweep;

    public GameObject bully;

    public GameObject firstPrize;

    public GameObject baba;

    public GameObject devin;

    public AudioSource tutorBaldi;

    public RectTransform boots;

    public string mode;

    public int level;

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

    public GameObject alarmClock;

    public TMP_Text notebookCount;

    public Material wall;
    public Material sand;

    public GameObject pauseMenu;

    public GameObject reticle;

    public RectTransform itemSelect;

    private int[] itemSelectOffset;

    public bool gamePaused;

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

    public GameObject miko;
    public GameObject alger;

    public AudioClip boowomp;

    public CameraScript camScript;

    public int maxNoteboos;

    public GameObject asdaa;

    public int maxItems;

    public bool disablePausing;

    public int laps;

    public bool gameOverPlayed;

    public NavMeshAgent baldiAgent;
    public NavMeshAgent princAgent;
    public NavMeshAgent ballAgent;
    public NavMeshAgent craftAgent;

    public AudioClip congratulatation;
}