using UnityEngine;

public class PressToBurnScript : MonoBehaviour
{
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && !pressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.transform.gameObject == transform.gameObject & Vector3.Distance(this.player.position, base.transform.position) < 10f & Cursor.lockState == CursorLockMode.Locked)
                {
                    transform.tag = "Untagged";
                    pressed = true;
                    frs.Press();
                }
            }
        }      
    }

    public Transform player;

    public FireRoomScript frs;

    bool pressed;
}