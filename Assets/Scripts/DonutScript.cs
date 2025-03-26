using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DonutScript : MonoBehaviour
{
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        gc = FindObjectOfType<GameControllerScript>();
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
            if (Random.Range(0, 600) == 28 || (gc.IsAprilFools() && Random.Range(1, 5) == 2))
            {
                for (int i = 0; i < 4; i++)
                {
                    if (gc.item[i] == 4)
                    {
                        PlayerPrefs.SetInt("rollBonus", PlayerPrefs.GetInt("rollBonus") + 5);
                    }
                }
                SceneManager.LoadScene("Luck");
            }
            Destroy(gameObject);
        }
    }

    private Transform player;

    GameControllerScript gc;
}
