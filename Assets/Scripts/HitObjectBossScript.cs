using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObjectBossScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        if (camScript == null)
        {
            camScript = FindObjectOfType<CameraScript>();
        }
        state = 0; // its just floating there waiting to be picked up
    }

    // Update is called once per frame
    void Update()
    {
        MeshRenderer mr = objectModel.GetComponent<MeshRenderer>();
        if (state == 0)
        {
            mr.material.color = Color.white;
        }
        if (state == 1)
        {
            mr.material.color = new Color(1, 1, 1, 0.25f);
            if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && Time.timeScale != 0)
            {
                print("pressed");
                decayTimer = 10;
                state = 2;
                FindObjectOfType<PlayerScript>().holdingObject = false;
            }
        }
        if (state == 2)
        {
            transform.Translate(55 * Time.deltaTime * Vector3.forward);
            mr.material.color = Color.white;
            objectModel.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + 90, transform.rotation.z);
            if (decayTimer < 0)
            {
                Destroy(gameObject);
                print("one of them fuckng died");
            }
            else
            {
                decayTimer -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player" && state == 0 && !other.GetComponent<PlayerScript>().holdingObject)
        {
            state = 1; // its ready to be thrown
            other.GetComponent<PlayerScript>().holdingObject = true;
        }
    }

    private void LateUpdate()
    {
        if (state == 1)
        {
            transform.position = camScript.transform.position + camScript.transform.forward * 5;
            transform.rotation = camScript.transform.rotation;
        }
    }

    public int state;

    private float decayTimer = 99f;
    public Transform player;
    public CameraScript camScript;

    public Material[] woodType;
    public Transform objectModel;
}
