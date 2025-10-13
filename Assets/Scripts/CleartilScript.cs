using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CleartilScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        string[] text = { "You naughty child! I won't tolerate this behavior.", "Come here, young one, and let me teach you a lesson." }; ;
        float[] time = { 4.15f, 3.35f};
        Color[] color = { new Color32(85, 63, 63, 255), new Color32(85, 63, 63, 255) };
        FindObjectOfType<SubtitleManager>().AddChained2DSubtitle(text, time, color);
    }

    // Update is called once per frame
    void Update()
    {
        if (wait > 0)
        {
            wait -= Time.deltaTime;
            return;
        }
        agent.SetDestination(player.position);
        StartCoroutine(Step());
    }

    IEnumerator Step()
    {
        wait = waitTime;
        anim.SetTrigger("step");
        agent.speed = 84;
        aud.Play();
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        agent.speed = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "UbrSpray(Clone)")
        {
            wait += 5;
            waitTime += 0.1f;
        }
        if (other.transform.name == "Yellow Face")
        {
            FindObjectOfType<GameControllerScript>().SomeoneTied(gameObject);
            gameObject.SetActive(false);
        }
    }

    public float waitTime = 2.4f;
    float wait = 3;

    public Animator anim;
    AudioSource aud;
    NavMeshAgent agent;

    public Transform player;
}
