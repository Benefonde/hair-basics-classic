using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossObjectManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        maxObjects = locator.newLocation.Length - 1;
        starting[0] = Instantiate(throwable, new Vector3(-75, 4, 5), Quaternion.identity);
        starting[1] = Instantiate(throwable, new Vector3(-130, 4, 5), Quaternion.identity);
        starting[2] = Instantiate(throwable, new Vector3(-105, 4, 35), Quaternion.identity);
    }

    void Update()
    {
        if (FindObjectsOfType<HitObjectBossScript>().Length < maxObjects)
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime;
            }
            else
            {
                locator.GetNewTarget();
                Vector3 spawnPosition = new Vector3(target.position.x, 4, target.position.z);

                HitObjectBossScript existingObject;
                Collider[] colliders = Physics.OverlapSphere(spawnPosition, 0.5f);

                foreach (var collider in colliders)
                {
                    HitObjectBossScript hitObjectScript = collider.GetComponent<HitObjectBossScript>();
                    if (hitObjectScript != null)
                    {
                        existingObject = hitObjectScript;
                        break;
                    }
                }

                if (colliders.Length == 0)
                {
                    GameObject a = Object.Instantiate(throwable, spawnPosition, Quaternion.identity);
                    a.GetComponent<HitObjectBossScript>().camScript = camScript;
                    print("spawned it at " + spawnPosition.ToString());
                    delay = Random.Range(4, 16);
                }
            }
        }
    }

    public GameObject throwable;
    public AlgerNullScript alger;

    public int maxObjects;
    public bool startSpawning;

    public AILocationSelectorScript locator;
    public Transform target;

    public GameObject[] starting;

    private float delay;
    public CameraScript camScript;
}
