using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLockClick : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E) && Time.timeScale != 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
