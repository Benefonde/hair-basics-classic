using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzafaceScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        pauseTime = 3;
        gameObject.transform.position = player.position;
        FindObjectOfType<SubtitleManager>().Add3DSubtitle("HAH HAHA HA HA HAA!!", 2, Color.yellow, transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseTime > 0)
        {
            pauseTime -= 1 * Time.deltaTime;
            gameObject.layer = 8;
            return;
        }
        gameObject.transform.position = Vector3.MoveTowards(transform.position, player.transform.position, (moveSpeed) * (1 * Time.deltaTime));
        if (Time.deltaTime == 1)
        {
            gc.tc.pizzafaceTime += Time.deltaTime;
        }
        else if (Time.deltaTime != 0)
        {
            gc.tc.pizzafaceTime += Time.unscaledDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "UbrSpray(Clone)")
        {
            pauseTime = 15;
        }
        if (other.transform.name == "Yellow Face")
        {
            gc.SomeoneTied(gameObject);
            gameObject.SetActive(false);
        }
    }

    public float pauseTime;
    public Transform player;

    public int moveSpeed;

    public GameControllerScript gc;
}
