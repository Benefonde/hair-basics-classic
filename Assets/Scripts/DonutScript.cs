using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutScript : MonoBehaviour
{
    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        transform.Translate(50 * Time.deltaTime * Vector3.forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name.Contains("Wall"))
        {
            player.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
            Destroy(gameObject);
        }
    }

    private Transform player;
}
