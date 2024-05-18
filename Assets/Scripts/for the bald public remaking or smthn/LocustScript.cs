using UnityEngine;
using UnityEngine.AI;

public class LocustScript : MonoBehaviour
{
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.SpawnToWander();
		Wander();
		agent.speed = Random.Range(22, 30);
		despawnTime = Random.Range(50, 90);
	}

	private void Update()
	{
		if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
		if (this.despawnTime > 0f)
		{
			this.despawnTime -= 1f * Time.deltaTime;
		}
        else
        {
			Destroy(gameObject);
        }
	}

	private void FixedUpdate()
	{
		Vector3 direction = this.player.position - base.transform.position;
		if (coolDown <= 0f)
		{
			this.TargetPlayer();
		}
	}

	private void Wander()
	{
		this.wanderer.GetNewTarget();
		this.agent.SetDestination(this.wanderTarget.position);
		this.coolDown = 1f;
	}
	private void SpawnToWander()
    {
		this.wanderer.GetNewTarget();
		this.agent.Warp(this.wanderTarget.position);
	}

	private void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
		this.coolDown = 1f;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.name == "Player")
        {
			Wander();
			coolDown = 10;
        }
	}

	public bool db;

	public Transform player;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	public float coolDown;
	public float despawnTime;

	private NavMeshAgent agent;

	public GameControllerScript gc;
}
