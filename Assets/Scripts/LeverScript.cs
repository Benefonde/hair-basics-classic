using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    private void Start()
    {
        pull = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.transform.gameObject == transform.gameObject & Vector3.Distance(this.player.position, base.transform.position) < 10f & Cursor.lockState == CursorLockMode.Locked)
                {
                    Pull();
                }
            }
        }
    }

    public void Pull()
    {
        if (!luckScreen.isRolling)
        {
            pull.SetTrigger("Pull the lever or whatever RIGHT NOW RIGHT NOW");
            luckScreen.Roll();
        }
    }

    public LuckScreenScript luckScreen;

    Animator pull;

    public Transform player;
}