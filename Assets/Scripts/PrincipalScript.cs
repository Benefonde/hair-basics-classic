using UnityEngine;
using UnityEngine.AI;

public class PrincipalScript : MonoBehaviour
{
	public bool seesRuleBreak;

	public Transform player;

	public Transform bully;

	public bool bullySeen;

	public PlayerScript playerScript;

	public BullyScript bullyScript;

	public BaldiScript baldiScript;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	public DoorScript officeDoor;

	public float coolDown;

	public float timeSeenRuleBreak;

	public bool angry;

	public bool inOffice;

	private int detentions;

	private int[] lockTime = new int[12]
	{
		15, 20, 25, 30, 35, 40, 45, 50, 55, 60,
		99, 99
	};

	public AudioClip[] audTimes = new AudioClip[11];

	public AudioClip[] audScolds = new AudioClip[4];

	public AudioClip audDetention;

	public AudioClip audNoDrinking;

	public AudioClip audNoBullying;

	public AudioClip audNoBullying2;

	public AudioClip audNoFaculty;

	public AudioClip audNoLockers;

	public AudioClip audNoRunning;

	public AudioClip audNoStabbing;

	public AudioClip audNoEscaping;

	public AudioClip audWhatDoing;

	public AudioClip aud_Whistle;

	public AudioClip aud_Delay;

	public AudioClip coming;

	private NavMeshAgent agent;

	private AudioQueueScript audioQueue;

	private AudioSource audioDevice;

	public AudioSource quietAudioDevice;

	private RaycastHit hit;

	public int speed = 30;

	private Vector3 aim;

	public CharacterController cc;

	public NavMeshObstacle doorObstacle;

	public GameControllerScript gc;

	private Light lighte;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		audioQueue = GetComponent<AudioQueueScript>();
		audioDevice = GetComponent<AudioSource>();
		if (gc.mode == "stealthy")
        {
			TpToWander();
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Spawned!", 0.5f, Color.white, transform);
			if (PlayerPrefs.GetInt("minimap", 1) == 1)
			{
				transform.Find("AlgerMap").gameObject.SetActive(true);
				lighte = gameObject.GetComponent<Light>();
				lighte.enabled = true;
				lighte.range /= 1.5f;
			}
            else
            {
				lighte = gameObject.GetComponent<Light>();
				lighte.color = Color.red;
				lighte.enabled = true;
				lighte.range *= 3f;
			}
        }
		if (gc.mode == "classic")
        {
			speed = 20;
        }
	}

	private void Update()
	{
		if (seesRuleBreak)
		{
			if (gc.mode == "stealthy")
			{
				timeSeenRuleBreak += 0.35f * Time.deltaTime;
			}
            else
			{
				timeSeenRuleBreak += 1f * Time.deltaTime;
			}
			if (((double)timeSeenRuleBreak >= 0.55f) & !angry)
			{
				angry = true;
				seesRuleBreak = false;
				timeSeenRuleBreak = 0f;
				TargetPlayer();
				CorrectPlayer();
			}
		}
		else
		{
			timeSeenRuleBreak = 0f;
		}
		if (coolDown > 0f)
		{
			coolDown -= 1f * Time.deltaTime;
		}
		if ((agent.destination != player.transform.position || bullySeen) && !summon)
        {
			agent.speed = speed;
        }
		if (officeDoor.DoorLocked)
        {
			doorObstacle.enabled = true;
        }
        else
        {
			doorObstacle.enabled = false;
        }
		if (angry && gc.mode == "stealthy")
        {
			agent.speed += (agent.speed * 30) * Time.deltaTime;
			gameObject.layer = 12;
		}
	}

	private void FixedUpdate()
	{
		if (!angry)
		{
			aim = player.position - base.transform.position;
			if (Physics.Raycast(base.transform.position, aim, out hit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hit.transform.tag == "Player") & (playerScript.guilt > 0f) & !inOffice & !angry)
			{
				seesRuleBreak = true;
			}
			else
			{
				seesRuleBreak = false;
				if ((agent.velocity.magnitude <= 1f) & (coolDown <= 0f))
				{
					Wander();
				}
			}
			aim = bully.position - base.transform.position;
			if (Physics.Raycast(base.transform.position, aim, out hit, float.PositiveInfinity, 769) & (hit.transform.name == "Its a Bully") & (bullyScript.guilt > 0f) & !inOffice & !angry)
			{
				TargetBully();
			}
		}
		else
		{
			TargetPlayer();
		}
	}

	private void Wander()
	{
		this.summon = false;
		agent.speed = speed;
		playerScript.principalBugFixer = 1;
		wanderer.GetNewTarget();
		agent.SetDestination(wanderTarget.position);
		if (agent.isStopped)
		{
			agent.isStopped = false;
		}
		coolDown = 1f;
		if (gc.mode == "stealthy")
		{
			if (Random.Range(0f, 3f) < 2f)
			{
				quietAudioDevice.PlayOneShot(aud_Whistle);
				FindObjectOfType<SubtitleManager>().Add3DSubtitle("Do do do do do....", aud_Whistle.length, Color.blue, transform);
			}
		}
        else
		{
			if (Random.Range(0f, 10f) < 1f)
			{
				quietAudioDevice.PlayOneShot(aud_Whistle);
				FindObjectOfType<SubtitleManager>().Add3DSubtitle("Do do do do do....", aud_Whistle.length, Color.blue, transform);
			}
		}
	}

	public void TpToWander()
    {
		wanderer.GetNewTargetHallway();
		agent.Warp(wanderTarget.position);
	}

	private void TargetPlayer()
	{
		agent.SetDestination(player.position);
		coolDown = 1f;
	}

	private void TargetBully()
	{
		if (!bullySeen)
		{
			this.summon = false;
			agent.speed = speed;
			agent.SetDestination(bully.position);
			audioQueue.QueueAudio(audNoBullying);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("Hey bully, stop it, you get detention now BYE!!", audNoBullying.length, Color.blue, transform);
			bullySeen = true;
		}
	}

	private void CorrectPlayer()
	{
		if (gc.mode == "stealthy")
        {
			agent.speed = speed - 10;
        }
		audioQueue.ClearAudioQueue();
		if (playerScript.guiltType == "faculty")
		{
			audioQueue.QueueAudio(audNoFaculty);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("Hey what are you doing in here? Get out!", audNoFaculty.length, Color.blue, transform);
		}
		else if (playerScript.guiltType == "running")
		{
			audioQueue.QueueAudio(audNoRunning);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("Hey you're not allowed to RUN IN THE HALLS", audNoRunning.length, Color.blue, transform);
		}
		else if (playerScript.guiltType == "drink")
		{
			audioQueue.QueueAudio(audNoDrinking);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("Stop D R I N K", audNoDrinking.length, Color.blue, transform);
		}
		else if (playerScript.guiltType == "escape")
		{
			audioQueue.QueueAudio(audNoEscaping);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("Hey what are you doing? E s c a p i n g?", audNoEscaping.length, Color.blue, transform);
		}
		else if (playerScript.guiltType == "bullying")
		{
			audioQueue.QueueAudio(audNoBullying2);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("Hey player, stop bullying NOW!", audNoBullying2.length, Color.blue, transform);
		}
		else if (playerScript.guiltType == "afterHours")
		{
			audioQueue.QueueAudio(audWhatDoing);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("What the hell are you doing in the school? It's 3 AM!", audWhatDoing.length, Color.blue, transform);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.name == "Office Trigger")
		{
			inOffice = true;
		}
		if ((other.tag == "Player") & angry & gc.mode != "stealthy")
		{
			gc.SetTime(1);
			if (detentions >= 11)
			{
				detentions = 10;
			}
			string[] saythisshit = { $"{lockTime[detentions]} seconds,", "depression for you!", "" };
			float[] waitthisshit = { audTimes[detentions].length + 0.2f, audDetention.length + 0.2f, 0.2f };
			Color[] colorthisshit = { Color.blue, Color.blue, Color.blue };
			inOffice = true;
			playerScript.principalBugFixer = 0;
			agent.Warp(new Vector3(10f, 0f, 170f));
			agent.isStopped = true;
			cc.enabled = false;
			other.transform.position = new Vector3(10f, 4f, 160f);
			other.transform.LookAt(new Vector3(base.transform.position.x, other.transform.position.y, base.transform.position.z));
			cc.enabled = true;
			audioQueue.QueueAudio(aud_Delay);
			audioQueue.QueueAudio(audTimes[detentions]);
			audioQueue.QueueAudio(audDetention);
			int num = Mathf.RoundToInt(Random.Range(0f, 3f));
			audioQueue.QueueAudio(audScolds[num]);
			waitthisshit[2] = audScolds[num].length;
			if (num == 0)
            {
				saythisshit[2] = "Don't make me do this again!";
            }
			if (num == 1)
			{
				saythisshit[2] = "You should know better.";
			}
			if (num == 2)
			{
				saythisshit[2] = "When will you learn?";
			}
			if (num == 3)
			{
				saythisshit[2] = "Your parents will hear about this one!";
			}
			officeDoor.LockDoor(lockTime[detentions]);
			officeDoor.detentionTime = lockTime[detentions];
			if (baldiScript.isActiveAndEnabled)
			{
				baldiScript.Hear(base.transform.position, 6f);
			}
			coolDown = 2f;
			angry = false;
			detentions++;
			playerScript.health -= detentions * 5f;
			FindObjectOfType<SubtitleManager>().AddChained3DSubtitle(saythisshit, waitthisshit, colorthisshit, transform);
			playerScript.gonnaBeKriller = transform;
			if (gc.mode == "pizza")
            {
				gc.pss.AddPoints((-detentions * 10) + 5, 2);
            }
			if (gc.mode == "endless")
			{
				gc.LoseNotebooks(detentions, 1.15f);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.name == "Office Trigger")
		{
			inOffice = false;
		}
		if (other.name == "Its a Bully")
		{
			bullySeen = false;
		}
	}
	public void Whistled()
	{
		audioDevice.PlayOneShot(coming); // WHAT
		FindObjectOfType<SubtitleManager>().Add3DSubtitle("Don't worry, I'm coming over!", coming.length, Color.blue, transform);
		this.agent.speed = Mathf.RoundToInt(speed * 3.333f);
		this.TargetPlayer();
		this.summon = true;
		if (baldiScript.isActiveAndEnabled)
		{
			this.baldiScript.Hear(player.transform.position, 5f);
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player" && this.summon)
		{
			this.summon = false;
			this.agent.speed = 30f;
			Time.timeScale = 1;
			gc.originalTimeScale = 1;
		}
		if (other.transform.name == "Yellow Face")
		{
			gc.SomeoneTied(gameObject);
			gameObject.SetActive(false);
		}
		if (other.transform.name == "UbrSpray(Clone)")
		{
			seesRuleBreak = false;
			timeSeenRuleBreak = 0;
			angry = false;
			Wander();
        }
		if (other.transform.name == "Objection(Clone)")
		{
			seesRuleBreak = false;
			timeSeenRuleBreak = 0;
			angry = false;
			Wander();
		}
	}

	public bool summon;
}