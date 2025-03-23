using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DonutScript : MonoBehaviour
{
    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        transform.Translate(80 * Time.deltaTime * Vector3.forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name.Contains("Wall"))
        {
            player.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
            if (Random.Range(0, 500) == 28 || (FindObjectOfType<GameControllerScript>().IsAprilFools() && Random.Range(1, 4) == 2))
            {
                SceneManager.LoadScene("Luck");
            }
            Destroy(gameObject);
        }
    }

    private Transform player;
}
