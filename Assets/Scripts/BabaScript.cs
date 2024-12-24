using UnityEngine;
using UnityEngine.AI;

public class BabaScript : MonoBehaviour
{
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.TargetPlayer();
	}

	private void Update()
	{
		if (this.coolDown > 0f && agent.velocity.magnitude <= 0.01f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
		if (this.ignorePlayer > 0f)
		{
			agent.speed = gc.playerScript.walkSpeed * 1.5f;
			this.ignorePlayer -= 1f * Time.deltaTime;
		}
        else
        {
			agent.speed = gc.playerScript.walkSpeed / 4;
		}
	}

	private void FixedUpdate()
	{
		Vector3 direction = this.player.position - base.transform.position;
		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.tag == "Player"))
		{
			this.db = true;
			if (ignorePlayer <= 0f && coolDown <= 0f)
			{
				TargetPlayer();
			}
			else if (ignorePlayer >= 0f && coolDown <= 0f)
            {
				Wander();
            }
		}
		else
		{
			this.db = false;
			if (ignorePlayer <= 0f && coolDown <= 0f)
			{
				this.Wander();
			}
            else if (coolDown <= 0f)
            {
				this.TargetPlayer();
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
		if (ignorePlayer <= 0)
		{
			this.agent.SetDestination(this.player.position);
			this.coolDown = 1f;
			return;
		}
		Wander();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.name == "UbrSpray(Clone)")
		{
			ignorePlayer = 10;
			Wander();
		}
		if (other.transform.name == "Player" && ignorePlayer <= 0f)
		{
            StartCoroutine(gc.playerScript.BabaTime());
			ignorePlayer = 40;
			coolDown = 10;
			Wander();
			gc.tc.babaGotPushed = true;
		}
		if (other.transform.name == "Yellow Face")
		{
			gc.SomeoneTied(gameObject);
			gameObject.SetActive(false);
		}
		if (other.transform.tag == "BSODA")
        {
			gc.tc.babaGotPushed = true;
        }
	}

	public bool db;

	public Transform player;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	public float coolDown;

	public float ignorePlayer;

	private NavMeshAgent agent;

	public GameControllerScript gc;
}
