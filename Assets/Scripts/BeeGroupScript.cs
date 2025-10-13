using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeeGroupScript : MonoBehaviour
{
    void Start()
    {
        transform.name = "Angry Bees";
        spot = FindObjectOfType<AILocationSelectorScript>();
        agent = GetComponent<NavMeshAgent>();
        bee = transform.GetChild(0);
        for (int i = 0; i < Random.Range(10, 40); i++)
        {
            MakeBee();
        }
        spot.GetNewTarget();
        agent.SetDestination(spot.transform.position);
        Invoke(nameof(DiesOfDeath), Random.Range(10, 14) * 5);
    }

    void MakeBee()
    {
        Transform a = Instantiate(bee.gameObject, transform).transform;
        a.localPosition = new Vector3(Random.Range(-3f, 3f), Random.Range(-0.75f, 0.75f) + 4.5f, Random.Range(-3f, 3f));
        a.GetComponent<SpriteRenderer>().sprite = beeSprite[Random.Range(0, 2)];
    }

    private void Update()
    {
        if (agent.remainingDistance < 5.5f)
        {
            agent.SetDestination(spot.GetNewTargetQuick(true));
        }
    }

    void DiesOfDeath()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Yellow Face")
        {
            FindObjectOfType<GameControllerScript>().SomeoneTied(gameObject);
            gameObject.SetActive(false);
        }
    }

    Transform bee;
    public Sprite[] beeSprite;

    NavMeshAgent agent;

    AILocationSelectorScript spot;
}