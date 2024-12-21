using UnityEngine;
using UnityEngine.AI;

public class FirstPrizeScript : MonoBehaviour
{
	public float debug;

	public float turnSpeed;

	public float str;

	public float angleDiff;

	public float normSpeed;

	public float runSpeed;

	public float currentSpeed;

	public float acceleration;

	public float speed;

	public float autoBrakeCool;

	public float crazyTime;

	public Quaternion targetRotation;

	public float coolDown;

	public float prevSpeed;

	public bool playerSeen;

	public bool hugAnnounced;

	public AILocationSelectorScript wanderer;

	public Transform player;

	public Transform wanderTarget;

	public AudioClip[] aud_Found = new AudioClip[2];

	public AudioClip[] aud_Lost = new AudioClip[2];

	public AudioClip[] aud_Hug = new AudioClip[2];

	public AudioClip[] aud_Random = new AudioClip[2];

	public AudioClip[] aud_Sorry = new AudioClip[2];

	public AudioClip audBang;

	public AudioSource audioDevice;

	public AudioSource motorAudio;

	private NavMeshAgent agent;

	public BaldiScript baldiScript;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		coolDown = 1f;
		Wander();
	}

	private void Update()
	{
		if (coolDown > 0f)
		{
			coolDown -= 1f * Time.deltaTime;
		}
		if (autoBrakeCool > 0f)
		{
			autoBrakeCool -= 1f * Time.deltaTime;
		}
		else
		{
			agent.autoBraking = true;
		}
		angleDiff = Mathf.DeltaAngle(base.transform.eulerAngles.y, Mathf.Atan2(agent.steeringTarget.x - base.transform.position.x, agent.steeringTarget.z - base.transform.position.z) * 57.29578f);
		if (crazyTime <= 0f)
		{
			if (Mathf.Abs(angleDiff) < 5f)
			{
				base.transform.LookAt(new Vector3(agent.steeringTarget.x, base.transform.position.y, agent.steeringTarget.z));
				agent.speed = currentSpeed;
			}
			else
			{
				base.transform.Rotate(new Vector3(0f, turnSpeed * Mathf.Sign(angleDiff) * Time.deltaTime, 0f));
				agent.speed = 0f;
			}
		}
		else
		{
			agent.speed = 0f;
			base.transform.Rotate(new Vector3(0f, 180f * Time.deltaTime, 0f));
			crazyTime -= Time.deltaTime;
		}
		if (prevSpeed - agent.velocity.magnitude > 85f)
		{
			audioDevice.PlayOneShot(audBang);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Bang!*", audBang.length, Color.gray, transform);
			int num = Mathf.RoundToInt(Random.Range(0f, 1f));
			audioDevice.PlayOneShot(aud_Sorry[num]);
			switch (num)
            {
				case 0: FindObjectOfType<SubtitleManager>().Add3DSubtitle("OH NO", aud_Sorry[num].length, Color.white, transform); break;
				case 1: FindObjectOfType<SubtitleManager>().Add3DSubtitle("SORRY", aud_Sorry[num].length, Color.white, transform); break;
			}
			if (baldiScript.isActiveAndEnabled)
			{
				baldiScript.Hear(base.transform.position, 2f);
			}
		}
		prevSpeed = agent.velocity.magnitude;
	}

	private void FixedUpdate()
	{
		Vector3 direction = player.position - base.transform.position;
		if (Physics.Raycast(base.transform.position, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.tag == "Player"))
		{
			if (!playerSeen && !audioDevice.isPlaying)
			{
				int num = Mathf.RoundToInt(Random.Range(0f, 1f));
				audioDevice.PlayOneShot(aud_Found[num]);
				switch (num)
				{
					case 0: FindObjectOfType<SubtitleManager>().Add3DSubtitle("HELLO", aud_Found[num].length, Color.white, transform); break;
					case 1: FindObjectOfType<SubtitleManager>().Add3DSubtitle("I SEE YOU, BUDDY", aud_Found[num].length, Color.white, transform); break;
				}
			}
			playerSeen = true;
			TargetPlayer();
			currentSpeed = runSpeed;
			return;
		}
		currentSpeed = normSpeed;
		if (playerSeen & (coolDown <= 0f))
		{
			if (!audioDevice.isPlaying)
			{
				int num2 = Mathf.RoundToInt(Random.Range(0f, 1f));
				audioDevice.PlayOneShot(aud_Lost[num2]);
				switch (num2)
				{
					case 0: FindObjectOfType<SubtitleManager>().Add3DSubtitle("WHERE ARE YOU GOING", aud_Lost[num2].length, Color.white, transform); break;
					case 1: FindObjectOfType<SubtitleManager>().Add3DSubtitle("COME BACK PLEASE", aud_Lost[num2].length, Color.white, transform); break;
				}
			}
			playerSeen = false;
			Wander();
		}
		else if ((agent.velocity.magnitude <= 1f) & (coolDown <= 0f) & ((base.transform.position - agent.destination).magnitude < 5f))
		{
			Wander();
		}
	}

	private void Wander()
	{
		wanderer.GetNewTargetHallway();
		agent.SetDestination(wanderTarget.position);
		hugAnnounced = false;
		int num = Mathf.RoundToInt(Random.Range(0f, 9f));
		if ((!audioDevice.isPlaying && num == 0) & (coolDown <= 0f))
		{
			int num2 = Mathf.RoundToInt(Random.Range(0f, 1f));
			audioDevice.PlayOneShot(aud_Random[num2]);
			switch (num2)
			{
				case 0: FindObjectOfType<SubtitleManager>().Add3DSubtitle("I AM LOOKING FOR SOMEONE TO GO WITH", aud_Random[num2].length, Color.white, transform); break;
				case 1: FindObjectOfType<SubtitleManager>().Add3DSubtitle("I HATE BEING ALONE", aud_Random[num2].length, Color.white, transform); break;
			}
		}
		coolDown = 1f;
	}

	private void TargetPlayer()
	{
		agent.SetDestination(player.position);
		coolDown = 0.5f;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.name == "UbrSpray(Clone)")
        {
			crazyTime += 5;
        }
		if (other.tag == "Player")
		{
			if (!audioDevice.isPlaying & !hugAnnounced)
			{
				int num = Mathf.RoundToInt(Random.Range(0f, 1f));
				audioDevice.PlayOneShot(aud_Hug[num]);
				switch (num)
				{
					case 0: FindObjectOfType<SubtitleManager>().Add3DSubtitle("I LOVE ROLLERSKATING WITH YOU", aud_Hug[num].length, Color.white, transform); break;
					case 1: FindObjectOfType<SubtitleManager>().Add3DSubtitle("LETS GO TOGETHER", aud_Hug[num].length, Color.white, transform); break;
				}
				hugAnnounced = true;
			}
			agent.autoBraking = false;
		}
		if (other.tag == "Player" || baldiScript.gc.playerScript.hugging)
        {
			if (prevSpeed - agent.velocity.magnitude > 85f)
			{
				baldiScript.gc.playerScript.health -= 50;
				baldiScript.gc.camScript.ShakeNow(new Vector3(0.5f, 0.5f, 0.5f), 5);
			}
		}
		if (other.transform.name == "Yellow Face")
		{
			baldiScript.gc.SomeoneTied(gameObject);
			gameObject.SetActive(false);
		}
	}

    private void OnTriggerStay(Collider other)
    {
		if (other.tag == "Player" || baldiScript.gc.playerScript.hugging)
		{
			if (prevSpeed - agent.velocity.magnitude > 85f)
			{
				baldiScript.gc.playerScript.health -= 50;
				baldiScript.gc.camScript.ShakeNow(new Vector3(0.5f, 0.5f, 0.5f), 5);
			}
		}
	}

    private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			autoBrakeCool = 1f;
		}
	}

	public void GoCrazy(float num)
	{
		crazyTime = num;
	}
}
