using UnityEngine;
using UnityEngine.AI;

public class NpcBaseScript : MonoBehaviour
{
	public bool db;

	public Transform player;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	private AudioSource audioDevice;

	public float coolDown;

	private Vector3 previous;

	private NavMeshAgent agent;

	public GameControllerScript gc;

	private void Start()
	{
		audioDevice = GetComponent<AudioSource>();
		agent = GetComponent<NavMeshAgent>();
		Wander();
	}

	private void Update()
	{
		Move();
		if (coolDown > 0f)
		{
			coolDown -= 1f * Time.deltaTime;
		}
	}

	private void FixedUpdate()
	{
		Vector3 direction = player.position - base.transform.position;
		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.name == "Player"))
		{
			db = true;
			TargetPlayer();
		}
		else
		{
			db = false;
			if (coolDown <= 0)
			{
				Wander();
			}
		}
	}

	private void Wander()
	{
		wanderer.GetNewTarget();
		agent.SetDestination(wanderTarget.position);
		coolDown = 1f;
	}

	public void TargetPlayer()
	{
		agent.SetDestination(player.position);
		coolDown = 1f;
	}

	private void Move()
	{
		if (gc.isActiveAndEnabled)
		{
			if ((base.transform.position == previous) & (coolDown < 0f))
			{
				Wander();
			}
		}
		previous = base.transform.position;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
        {
			//player stuffs
        }
		if (other.transform.name == "Yellow Face") // should npc TIE to yellowey?
		{
			gc.SomeoneTied(gameObject);
			gameObject.SetActive(false);
		}
	}
}
