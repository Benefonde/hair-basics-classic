using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class KrillerAnimScriptMTM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        aud = GetComponent<AudioSource>();
        agent.SetDestination(target);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveFrames > 0)
        {
            Move();
            moveFrames--;
        }
        else
        {
            agent.speed = 0;
        }
        if (wait >= 0)
        {
            wait -= Time.deltaTime;
        }
        else
        {
            wait = interval;
            moveFrames = setMoveFrames;
            anim.SetTrigger("slap");
            aud.Play();
        }


        if (transform.name == "Panino")
        {
            if (agent.remainingDistance == 0)
            {
                agent.SetDestination(new Vector3(-180, 4, 130));
                interval = 0.6f;
                speed = 100;
                moveFrames = 8;
                BaldiDies();
                Invoke(nameof(GaymeJoever), 3);
            }
        }
    }

    void Move()
    {
        agent.speed = speed;
    }

    void BaldiDies()
    {
        bald.SetActive(false);
        balDead.transform.position = bald.transform.position;
        balDead.SetActive(true);
    }

    void GaymeJoever()
    {
        SceneManager.LoadScene("GameOver");
    }

    public GameObject bald;
    public GameObject balDead;

    public int speed;
    public float interval;
    public int setMoveFrames;
    public Vector3 target;

    int moveFrames;

    [SerializeField]
    float wait;

    Animator anim;
    NavMeshAgent agent;
    AudioSource aud;
    bool iGotThere;
}
