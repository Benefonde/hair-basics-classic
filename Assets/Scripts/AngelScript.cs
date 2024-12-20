using UnityEngine;
using UnityEngine.AI;

public class AngelScript : MonoBehaviour
{
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.Wander();
		audioDevice = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (this.coolDown > 0f && agent.velocity.magnitude <= 0.01f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
	}

	private void FixedUpdate()
	{
		Vector3 direction = this.player.position - base.transform.position;
		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.tag == "Player"))
		{
			if (((gc.item[0] == 0) || (gc.item[1] == 0) || (gc.item[2] == 0) || (gc.item[3] == 0)) && ignorePlayer <= 0)
			{
				TargetPlayer();
				if (!db && !audioDevice.isPlaying)
				{
					audioDevice.PlayOneShot(found);
					FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Player found*", found.length, Color.green, transform);
				}
				this.db = true;
				PlayerScript ps = player.GetComponent<PlayerScript>();
				agent.speed = ps.walkSpeed + 1;
			}
			else if (coolDown <= 0f)
			{
				this.Wander();
			}
		}
		else
		{
			if (db && !audioDevice.isPlaying)
			{
				audioDevice.PlayOneShot(lost);
				FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Player lost...*", found.length, Color.green, transform);
			}
			this.db = false;
			PlayerScript ps = player.GetComponent<PlayerScript>();
			agent.speed = ps.runSpeed / 5;
			if (coolDown <= 0f)
			{
				this.Wander();
			}
		}
	}

	private void Wander()
	{
		this.wanderer.GetNewTarget();
		this.agent.SetDestination(this.wanderTarget.position);
		this.coolDown = 1f;
	}

	private void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
		this.coolDown = 1f;
	}

    private void OnTriggerEnter(Collider other)
    {
		if (other.transform.name == "UbrSpray(Clone)")
		{
			ignorePlayer = 45;	
		}
		if (other.transform.name == "Player" && ignorePlayer <= 0)
        {
			if ((gc.item[0] == 0) || (gc.item[1] == 0) || (gc.item[2] == 0) || (gc.item[3] == 0))
			{
				gc.StartCoroutine(gc.AngelEvent(true, 45));
				gc.Teleport();
				gc.playerScript.health += 20;
				if (gc.mode == "endless")
				{
					gc.LoseNotebooks(-3, 1);
				}
				if (gc.mode == "pizza")
				{
					gc.pss.AddPoints(500, 1);
				}
			}
            else
            {
				gc.StartCoroutine(gc.AngelEvent(false, 75));
			}
			return;
        }
		if (other.gameObject.tag == "NPC")
        {
			gc.StartCoroutine(gc.AngelEvent(false, 25));
		}
    }

    public bool db;

	public Transform player;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	public float coolDown;

	private NavMeshAgent agent;

	public GameControllerScript gc;

	public AudioClip found;
	public AudioClip lost;

	private AudioSource audioDevice;

	float ignorePlayer;
}
