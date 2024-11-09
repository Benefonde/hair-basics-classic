using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerScript : MonoBehaviour
{
	public GameControllerScript gc;

	public BaldiScript baldi;

	public DoorScript door;

	public PlaytimeScript playtime;

	public bool gameOver;

	public bool jumpRope;

	public bool sweeping;

	public bool inSecret;

	public bool hugging;

	public bool bootsActive;

	public int principalBugFixer;

	public FirstPrizeScript fpr;

	public float sweepingFailsave;

	public float fliparoo;

	public float flipaturn;

	private Quaternion playerRotation;

	public Vector3 frozenPosition;

	public bool sensitivityActive;

	private float sensitivity;

	public float mouseSensitivity;

	public float walkSpeed;

	public float runSpeed;

	public float slowSpeed;

	public float maxStamina;

	public float staminaRate;

	public float guilt;

	public float initGuilt;

	private float moveX;

	private float moveZ;

	private Vector3 moveDirection;

	private float playerSpeed;

	public float stamina;

	public float health;

	public CharacterController cc;

	public NavMeshAgent gottaSweep;

	public NavMeshAgent firstPrize;

	public Transform firstPrizeTransform;

	public Slider staminaBar;
	public Slider healthBar;
	public Slider jammerBar;

	public float db;

	public string guiltType;

	public GameObject jumpRopeScreen;

	public float height;

	public Material blackSky;

	public Canvas hud;

    public Canvas mobile1;

	public Canvas mobile2;

	public AgentTest at;

	public bool teleporting;

	private bool isGrounded;

	private float gravity;

	public bool pipeGame;
	public float pipeGameGravity;

	public bool isJumping;

	public TMP_Text percent;
	public TMP_Text hercent;

	public bool walkThroughAbility;
	public bool blockPathAbility;

	private float walkThroughCooldown;
	private int walkThroughUses;
	private float blockPathCooldown;

	public bool FreezePlayer;

	private void Start()
	{
		height = base.transform.position.y;
		stamina = maxStamina;
		health = 100;
		playerRotation = base.transform.rotation;
		mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2);
		principalBugFixer = 1;
		flipaturn = 1f;
		walkThroughCooldown = 2;
		blockPathCooldown = 3;
	}

	public IEnumerator BabaTime()
    {
		FreezePlayer = true;
		babaWinThing.SetActive(true);
		yield return new WaitForSeconds(5);
		babaWinThing.SetActive(false);
		FreezePlayer = false;
    }

	public void SetRotation(Quaternion angle)
    {
		playerRotation = angle;
    }

	private void Update()
	{
		mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2);
		if (!inSecret)
		{
			base.transform.position = new Vector3(base.transform.position.x, height, base.transform.position.z);
		}
		if (inSecret)
		{
			CheckGround();
			Jump();
		}
		if (!pipeGame)
		{
			if (crazyAppleTimer < 0 & !FreezePlayer)
			{
				PlayerMove();
				MouseMove();
			}
			else if (!FreezePlayer)
			{
				NavMeshMove();
			}
			StaminaCheck();
			HealthCheck();
			GuiltCheck();
		}
        else
        {
			PipegameMove();
			MouseMove();
		}
		if (Time.timeScale != 0)
		{
			if (Input.GetKeyDown(KeyCode.T) && blockPathCooldown > 0 && blockPathAbility)
			{
				gc.audioDevice.PlayOneShot(gc.no);
				FindObjectOfType<SubtitleManager>().Add2DSubtitle("Not yet.", 1, Color.white);
			}
			if (Input.GetKeyDown(KeyCode.R) && walkThroughCooldown > 0 && walkThroughAbility)
			{
				gc.audioDevice.PlayOneShot(gc.no);
				FindObjectOfType<SubtitleManager>().Add2DSubtitle("Not yet.", 1, Color.white);
			}
			if (Input.GetKeyDown(KeyCode.R) && walkThroughCooldown < 0 && walkThroughAbility)
			{
				PhaseThroughCheck();
			}
			else if (walkThroughCooldown > 0)
			{
				walkThroughCooldown -= Time.deltaTime;
			}
			if (Input.GetKeyDown(KeyCode.T) && blockPathCooldown < 0 && blockPathAbility)
			{
				StartCoroutine(BlockPathAbility());
			}
			else if (blockPathCooldown > 0)
			{
				blockPathCooldown -= Time.deltaTime;
			}
		}
		jammerBar.value = gc.jammersTimer;
		if (cc.velocity.magnitude > 0f)
		{
			gc.LockMouse();
		}
		if (sweepingFailsave > 0f)
		{
			sweepingFailsave -= Time.deltaTime;
			return;
		}
		sweeping = false;
		hugging = false;
		if (at.touchingPlayer & !bootsActive)
        {
			if (gc.mode != "free")
			{
				transform.LookAt(new Vector3(baldi.transform.position.x, base.transform.position.y, baldi.transform.position.z));
			}
            else
            {
				transform.LookAt(new Vector3(gc.tutorBaldi.transform.position.x, base.transform.position.y, gc.tutorBaldi.transform.position.z));
			}
        }
		mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2);
	}

	public IEnumerator PlayerInv()
	{
		gc.debugMode = true;
		yield return new WaitForSeconds(1);
		gc.debugMode = false;
	}

	void PipegameMove()
    {
		if (pipeGameGravity != 0)
		{
			if (Input.GetKey(KeyCode.LeftShift))
            {
				cc.Move(1.8f * runSpeed * Time.deltaTime * transform.forward);
            }
		}
    }

	void PhaseThroughCheck()
	{
		Vector3 destinationPosition = transform.position + transform.forward * 10;

		RaycastHit floorHit;
		if (Physics.Raycast(destinationPosition, -transform.up, out floorHit, 5f))
		{
			if (floorHit.transform.name == "Floor")
			{
				transform.position = destinationPosition;

				gc.audioDevice.PlayOneShot(gc.aud_Teleport);
				FindObjectOfType<SubtitleManager>().Add2DSubtitle("BREOW!", 1, Color.green);
				walkThroughUses++;
				if (walkThroughUses == 3)
				{
					walkThroughUses = 0;
					walkThroughCooldown = 5f;
				}
				return;
			}
		}

		gc.audioDevice.PlayOneShot(gc.no);
		FindObjectOfType<SubtitleManager>().Add2DSubtitle("Not here.", 1, Color.white);
	}

	IEnumerator BlockPathAbility()
    {
		blockPathCooldown = 45;
		transform.Find("Block").gameObject.SetActive(true);
		gc.audioDevice.PlayOneShot(gc.aud_Lock);
		FindObjectOfType<SubtitleManager>().Add2DSubtitle("Blockage appears", 1, Color.green);
		yield return new WaitForSeconds(15);
		gc.audioDevice.PlayOneShot(gc.aud_Unlock);
		FindObjectOfType<SubtitleManager>().Add2DSubtitle("Blockage disappears, wait 30 seconds to try again", 1, Color.red);
		transform.Find("Block").gameObject.SetActive(false);
	}

	public void NavMeshMove()
    {
		gc.debugMode = true;
		playerAgent.enabled = true;
		cc.enabled = false;
		if (!gc.crazyAppleMusic.isPlaying)
        {
			gc.crazyAppleMusic.Play();
        }
		playerAgent.SetDestination(FindNearestDwayne());
		if (playerAgent.remainingDistance < 2f)
        {
			transform.LookAt(FindNearestDwayne());
        }
		playerAgent.speed = runSpeed + 20;
		health += 1 * Time.deltaTime;
		stamina += 0.3f * staminaRate * Time.deltaTime;
		crazyAppleTimer -= Time.deltaTime;
		if (crazyAppleTimer < 0)
        {
			gc.crazyAppleMusic.Stop();
			playerAgent.enabled = false;
			gc.debugMode = false;
			cc.enabled = true;
		}
    }
	Vector3 FindNearestDwayne()
	{
		GameObject nearestDwayne = null;
		float shortestDistance = Mathf.Infinity;
		Vector3 playerPosition = transform.position;

		foreach (NotebookScript dwayne in gc.dwaynes)
		{
			if (dwayne.transform.position.y == 4)
			{
				float distance = Vector3.Distance(playerPosition, dwayne.transform.position);
				if (distance < shortestDistance)
				{
					shortestDistance = distance;
					nearestDwayne = dwayne.gameObject;
				}
			}
		}
		if (gc.notebooks == gc.maxNoteboos)
        {
			crazyAppleTimer = 0;
			return transform.position;
        }
		return nearestDwayne.transform.position;
	}

	private void Jump()
	{
		if (isGrounded && Input.GetKeyDown(KeyCode.Space))
		{
			verticalVelocity = Mathf.Sqrt(jumpHeight * -2 * gravityValue);
			isGrounded = false;
			isJumping = true;
		}
			verticalVelocity += gravityValue * Time.deltaTime;

			Vector3 newPosition = transform.position;
			newPosition.y += verticalVelocity * Time.deltaTime;
			transform.position = newPosition;
		if (isGrounded)
		{
			newPosition.y = 4.0f;
			transform.position = newPosition;
			verticalVelocity = 0.0f;
		}
	}

	private void CheckGround()
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position, Vector3.down, out hit, 4.1f) && gravity <= 0)
		{
			if (hit.collider.transform.name == "Floor")
			{
				isGrounded = true;
			}
			else
			{
				isGrounded = false;
			}
		}
		else
		{
			isGrounded = false;
		}
	}

	private void MouseMove()
	{
		playerRotation.eulerAngles = new Vector3(playerRotation.eulerAngles.x, playerRotation.eulerAngles.y, fliparoo);
		playerRotation.eulerAngles += Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity * Time.timeScale * flipaturn;
		if (PlayerPrefs.GetInt("3dCam", 0) == 1)
		{
			camscript.verticalLook -= 0.8f * Input.GetAxis("Mouse Y") * mouseSensitivity * Time.timeScale * flipaturn;
		}
		base.transform.rotation = playerRotation;
	}

	private void PlayerMove()
	{
		Vector3 vector = new Vector3(0f, 0f, 0f);
		Vector3 vector2 = new Vector3(0f, 0f, 0f);
		vector = base.transform.forward * Input.GetAxis("Forward");
		vector2 = base.transform.right * Input.GetAxis("Strafe");
		if (stamina > 0f)
		{
			if (Input.GetButton("Run"))
			{
				playerSpeed = runSpeed;
				sensitivity = 1f;
				if ((cc.velocity.magnitude > 0.05f) & !hugging & !sweeping)
				{
					ResetGuilt("running", 0.2f);
				}
			}
			else
			{
				playerSpeed = walkSpeed;
				if (sensitivityActive)
				{
					sensitivity = Mathf.Clamp((vector2 + vector).magnitude, 0f, 1f);
				}
				else
				{
					sensitivity = 1f;
				}
			}
		}
		else
		{
			playerSpeed = walkSpeed;
			if (sensitivityActive)
			{
				sensitivity = Mathf.Clamp((vector2 + vector).magnitude, 0f, 1f);
			}
			else
			{
				sensitivity = 1f;
			}
		}
		playerSpeed *= Time.deltaTime;
		moveDirection = (vector + vector2).normalized * playerSpeed * sensitivity;
		if (!(!jumpRope & !sweeping & !hugging))
		{
			if (sweeping && !bootsActive)
			{
				moveDirection = gottaSweep.velocity * Time.deltaTime + moveDirection * 0.3f;
			}
			else if (hugging && !bootsActive)
			{
				moveDirection = (firstPrize.velocity * 1.2f * Time.deltaTime + (new Vector3(firstPrizeTransform.position.x, height, firstPrizeTransform.position.z) + new Vector3(Mathf.RoundToInt(firstPrizeTransform.forward.x), 0f, Mathf.RoundToInt(firstPrizeTransform.forward.z)) * 3f - base.transform.position)) * principalBugFixer;
			}
		}
		cc.Move(moveDirection);
	}

	private void StaminaCheck()
	{
		if (cc.velocity.magnitude > 0.1f)
		{
			if (Input.GetButton("Run") & (stamina > 0f))
			{
				stamina -= staminaRate * Time.deltaTime;
			}
			if ((stamina < 0f) & (stamina < -0.01f))
			{
				stamina = -0.01f;
			}
		}
		else if (stamina < maxStamina)
		{
			stamina += staminaRate * Time.deltaTime;
		}
		staminaBar.value = stamina / maxStamina * 100f;
		percent.text = $"{Mathf.RoundToInt(stamina / maxStamina * 100f)}%";
		if (infStamina)
        {
			stamina = Mathf.Infinity;
			percent.text = $"Infinity";
		}
		else if (stamina > maxStamina * 6.50f)
        {
			stamina = maxStamina * 6.50f;
        }
	}

	public void DisableInfStamina()
    {
		infStamina = false;
		stamina = maxStamina * 0f;
    }

	private void HealthCheck()
    {
		hercent.text = $"{Mathf.RoundToInt(health)}%";
		healthBar.value = health / 100 * 100f;
		if (health < 100)
        {
			health += staminaRate * Time.deltaTime / 40;
		}
		if (health < 0.1f)
        {
			health = 2763;
			Die();
			if (gc.mode == "endless")
			{
				if (gc.notebooks > PlayerPrefs.GetInt("HighBooks"))
				{
					PlayerPrefs.SetInt("HighBooks", gc.notebooks);
				}
				PlayerPrefs.SetInt("CurrentBooks", gc.notebooks);
			}
		}
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 12)
		{
			if ((gc.HasItemInInventory(17) && other.transform.name == "Panino") && crazyAppleTimer < 0)
			{
				StartCoroutine(gc.PaninoEat());
				for (int i = 0; i < 5; i++)
				{
					if (gc.item[i] == 17)
					{
						gc.LoseItem(i);
						return;
					}
				}
				gc.tc.usedItem = true;
			}

			camscript.SetCharacter(other.gameObject);
			if (!gc.debugMode)
			{
				gameOver = true;
				RenderSettings.skybox = blackSky;
			}
		}
		if (other.transform.name == "Lap2Portal")
        {
			gc.Lap2EnterPortal();
        }
		if (other.transform.name == "Toppin 1")
        {
			stamina += maxStamina;
			health += 2;
			other.gameObject.SetActive(false);
			ths.ToppinRecruit(0);
			gc.pss.AddPoints(1000, 3);
			gc.tc.collectedToppings++;
		}
		if (other.transform.name == "Toppin 2")
		{
			stamina += maxStamina;
			health += 2;
			other.gameObject.SetActive(false);
			ths.ToppinRecruit(1);
			gc.pss.AddPoints(1000, 3);
			gc.tc.collectedToppings++;
		}
		if (other.transform.name == "Toppin 3")
		{
			stamina += maxStamina;
			health += 2;
			other.gameObject.SetActive(false);
			ths.ToppinRecruit(2);
			gc.pss.AddPoints(1000, 3);
			gc.tc.collectedToppings++;
		}
		if (other.transform.name == "Toppin 4")
		{
			stamina += maxStamina;
			health += 2;
			other.gameObject.SetActive(false);
			ths.ToppinRecruit(3);
			gc.pss.AddPoints(1000, 3);
			gc.tc.collectedToppings++;
		}
		if (other.transform.name == "Toppin 5")
		{
			stamina += maxStamina;
			health += 2;
			other.gameObject.SetActive(false);
			ths.ToppinRecruit(4);
			gc.pss.AddPoints(1000, 3);
			gc.tc.collectedToppings++;
		}
		if (other.transform.name == "Treasure")
		{
			stamina += maxStamina * 1.25f;
			health += 4;
			other.gameObject.SetActive(false);
			gc.FoundTreasure();	
		}
		if (other.transform.name == "Panino" || other.transform.name == "Miko" || other.transform.name == "Alger (Alger's Basics")
		{
			health = 0.1f;
		}
	}

	public void Die()
    {
		gc.SomeoneTied(gameObject);
		gc.playerCollider.enabled = false;
		playerRotation.eulerAngles = new Vector3(-35, playerRotation.y, playerRotation.z);
		height = 0.5f;
		gc.playerCollider.enabled = false;
		gc.player.enabled = false;
		bob.SetPositionAndRotation(new Vector3(transform.position.x, 0.125f, transform.position.z), Quaternion.Euler(90, transform.rotation.y, transform.rotation.z));
		bob.localScale = new Vector3(4, 4, 1);
		bob.gameObject.layer = 9;
		camscript.FuckingDead = true;
		camscript.follow = bob;
		gc.hud.SetActive(false);
		playerRotation.eulerAngles = new Vector3(-35, playerRotation.y, playerRotation.z);
		Invoke("Tie", 2);
	}

	public bool TouchingNavMesh(Vector3 center, float range)
	{
		for (int i = 0; i < 30; i++)
		{
			Vector3 randomPoint = center + Random.insideUnitSphere * range;
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                return true;
            }
        }
		return false;
	}

	public void Tie()
    {
		camscript.follow = gonnaBeKriller;
		Invoke("SendToSchool", 4);
    }

	public void SendToSchool()
    {
		if (gc.mode == "classic")
		{
			SceneManager.LoadScene("thing");
			return;
		}
		SceneManager.LoadScene("School");
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.transform.name == "Pizzaface" && pissface.pauseTime < 0.1f)
        {
			health -= 4.5f * (9 * Time.deltaTime);
			camscript.ShakeNow(new Vector3(0.1f, 0.1f, 0.1f), 1);
			gonnaBeKriller = other.transform;
		}
		if (other.transform.name == "Yellow Face")
		{
			health -= 45f * (10 * Time.deltaTime);
			gonnaBeKriller = other.transform;
		}
		if (other.transform.name == "Sonic" && !bootsActive)
		{
			sweeping = true;
		}
		else if (other.transform.name == "Marty" && !bootsActive && firstPrize.velocity.magnitude > 5f && gc.firstPrizeScript.crazyTime <= 0)
		{
			hugging = true;
		}
		if (other.transform.name == "Zombie")
		{
			health -= 10 * (Time.deltaTime);
			gonnaBeKriller = other.transform;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.name == "Office Trigger")
		{
			ResetGuilt("escape", door.detentionTime);
		}
		else if (other.transform.name == "Sonic")
		{
			sweeping = false;
		}
		else if (other.transform.name == "Marty")
		{
			hugging = false;
		}
	}

	public void ResetGuilt(string type, float amount)
	{
		if (amount >= guilt)
		{
			guilt = amount;
			guiltType = type;
			gc.tc.ruleBreak = false;
		}
	}

	private void GuiltCheck()
	{
		if (guilt > 0f)
		{
			guilt -= Time.deltaTime;
		}
	}

	public void ActivateBoots()
	{
		bootsActive = true;
		StartCoroutine(BootTimer(15));
	}

	private IEnumerator BootTimer(float timer)
	{
		float time = timer;
		while (time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		bootsActive = false;
	}

	public IEnumerator ActivateTrolling(int wait)
    {
		camscript.camYdefault -= 4;
		camscript.camYoffset -= 4;
		health -= 99;
		stamina -= 1000;
		if (gc.mode == "endless")
		{
			gc.LoseNotebooks(30, 1);
		}
		yield return new WaitForSeconds(wait * 1.35f);
		walkSpeed += 10;
		runSpeed += 11;
		camscript.camYdefault += 4;
		camscript.camYoffset += 4;
    }

	public CameraScript camscript;

	public ToppinHudScript ths;
	public PizzafaceScript pissface;

	public Transform bob;

	private float verticalVelocity = 0.0f;
	public float jumpHeight = 3.0f;
	public float gravityValue = -9.81f;

	public AudioClip smallTopping;
	public AudioClip bigTopping;

	public Transform gonnaBeKriller;

	public bool infStamina;

	public Transform[] exits;

	public float crazyAppleTimer;
	public NavMeshAgent playerAgent;

	public GameObject babaWinThing;

	public bool holdingObject;
}
