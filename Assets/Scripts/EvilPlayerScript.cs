using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EvilPlayerScript : MonoBehaviour
{
	public GameControllerScript gc;

	public BaldiPlayerScript baldi;

	public DoorScript door;

	private Quaternion playerRotation;

	public Vector3 frozenPosition;

	public float walkSpeed;

	public float runSpeed;

	public float maxStamina;

	public float staminaRate;

	public float stamina;

	public float db;

	public CharacterController cc;

	public float height;

	public Material blackSky;

	bool timeToNun;

	Rigidbody rb;

	private void Start()
	{
		playerAgent.enabled = true;
		cc.enabled = false;
		height = base.transform.position.y;
		stamina = maxStamina;
		playerRotation = base.transform.rotation;
		rb = GetComponent<Rigidbody>();
		gc.tc.ruleBreak = true;
	}

	private void Update()
	{
		NavMeshMove();
	}

	public void NavMeshMove()
    {
		if (gc.notebooks != gc.maxNoteboos)
		{
			playerAgent.SetDestination(FindNearestDwayne());
		}
        else
        {
			playerAgent.SetDestination(FindNearestExit());
		}
		if (playerAgent.remainingDistance < 2f && gc.notebooks != gc.maxNoteboos)
        {
			transform.LookAt(FindNearestDwayne());
		}
		timeToNun = false;
		if (Vector3.Distance(baldi.transform.position, transform.position) <= 25)
        {
			timeToNun = true;
        }
		playerAgent.speed = walkSpeed; 
		if (timeToNun && stamina > 0)
        {
			stamina -= staminaRate * Time.deltaTime;
			playerAgent.speed = runSpeed;
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
		return nearestDwayne.transform.position;
	}

	Vector3 FindNearestExit()
    {
		Transform nearestExit = null;
		float shortestDistance = Mathf.Infinity;
		Vector3 playerPosition = transform.position;

		foreach (Transform exit in exits)
		{
			if (exit.transform.position.y == 0)
			{
				float distance = Vector3.Distance(playerPosition, exit.transform.position);
				if (distance < shortestDistance)
				{
					shortestDistance = distance;
					nearestExit = exit.transform;
				}
			}
		}
		if (gc.exitsReached == 5)
		{
			return transform.position;
		}
		return nearestExit.position;
	}

	private void StaminaCheck()
	{
		if (!timeToNun)
		{
			stamina += (staminaRate / 30) * Time.deltaTime;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.name == "Panino (Player)")
		{
			Die();
		}
	}

	public void Die()
	{
		gc.SomeoneTied(gameObject, false);
		playerRotation.eulerAngles = new Vector3(-35, playerRotation.y, playerRotation.z);
		height = 0.5f;
		GetComponent<CapsuleCollider>().enabled = false;
		bob.SetPositionAndRotation(new Vector3(transform.position.x, 0.125f, transform.position.z), Quaternion.Euler(90, transform.rotation.y, transform.rotation.z));
		bob.localScale = new Vector3(4, 4, 1);
		bob.gameObject.layer = 9;
		walkSpeed = 0;
		runSpeed = 0;
		StartCoroutine(YourWiener());
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

	IEnumerator YourWiener()
    {
		yield return new WaitForSeconds(2);
		ets.BeatPaninoMode();
	}

	public Transform bob;

	public Transform[] exits;
	public NavMeshAgent playerAgent;

	public ExitTriggerScript ets;
}
