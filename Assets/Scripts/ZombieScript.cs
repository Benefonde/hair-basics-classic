﻿using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour
{
    private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		wanderer.GetNewTargetHallway();
		agent.Warp(wanderTarget.position);
		zombieSpeed = Random.Range(5, 20);
		for (int i = 0; i < 4; i++)
		{
			if (Random.Range(0, 20 / gc.notebooks) <= 0.3f)
			{
				armor[i].SetActive(true);
				switch (i)
                {
					case 0: defense += 2; health += 5; break;
					case 1: defense += 5; health += 10; break;
					case 2: defense += 3; health += 5; break;
					case 3: defense += 1; break;
				}
			}
		}
	}

	private void Update()
	{
		if (disableTime < 0)
		{
			Move();
		}
        else
        {
			disableTime -= Time.deltaTime;
        }

		if (invTime > 0)
        {
			invTime -= Time.deltaTime;
        }

		if (coolDown > 0f)
		{
			coolDown -= 1f * Time.deltaTime;
		}
		if (PlayerPrefs.GetInt("slowerKrillers", 0) == 1)
		{
			speed = zombieSpeed * 0.7f;
		}
		else
		{
			speed = zombieSpeed * 1f;
		}

		if (health <= 0)
        {
			Destroy(gameObject);
        }
	}

	private void FixedUpdate()
	{
		if (disableTime < 0)
		{
			agent.speed = speed;
		}
        else
        {
			agent.speed = 0;
        }
		Vector3 direction = player.position - base.transform.position;
		if (!gc.finaleMode)
		{
			if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.tag == "Player"))
			{
				db = true;
				TargetPlayer();
				coolDown = 1;
			}
			else
			{
				if (coolDown <= 0)
				{
					db = false;
					Wander();
					coolDown = 1;
				}
			}
		}
        else
        {
			TargetPlayer();
		}
	}

	private void Wander()
	{
		wanderer.GetNewTarget();
		agent.SetDestination(wanderTarget.position);
		coolDown = 1f;
		currentPriority = 0f;
	}

	public void TargetPlayer()
	{
		agent.SetDestination(player.position);
		coolDown = 1f;
		currentPriority = 1f;
	}

	private void Move()
	{
		if ((transform.position == previous) & (coolDown < 0f))
		{
			Wander();
		}
		previous = transform.position;
	}

	public void TakeDamage(int attack)
    {
		if (invTime > 0)
        {
			return;
        }
		health -= attack;
		invTime = 0.5f;
		disableTime = 0.5f;
		ss.durability -= Mathf.RoundToInt(defense / 2) + 1;
    }

    public bool db;

	public GameControllerScript gc;

	public SwordScript ss;

	public float speed;

	public GameObject[] armor = new GameObject[4];

	float zombieSpeed;

	private float currentPriority;

	public float defense = 1;

	int health = 35;

	float invTime;

	public Transform player;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	public float coolDown;

	private Vector3 previous;

	private NavMeshAgent agent;

	public float disableTime;
}