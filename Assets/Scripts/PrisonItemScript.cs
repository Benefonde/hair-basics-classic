using System;
using UnityEngine;

public class PrisonItemScript : MonoBehaviour
{
    private void Update()
    {
        if (ID == 0)
        {
            mapThing.SetActive(false);
            return;
        }
        mapThing.SetActive(true);
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.transform.gameObject == transform.gameObject & Vector3.Distance(player.position, base.transform.position) < 10f & Cursor.lockState == CursorLockMode.Locked)
                {
                    if (gc.item[0] == 0 | gc.item[1] == 0 | gc.item[2] == 0 || gc.item[3] == 0)
                    {
                        gc.CollectItem(ID);
                        raycastHit.transform.gameObject.SetActive(false);
                    }
                    else
                    {
                        int orgID = ID;
                        ID = gc.item[gc.itemSelected];
                        Texture itemTexture = gc.itemTextures[ID];
                        Sprite itemSprite = Sprite.Create((Texture2D)itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), itemTexture.width * 1.55f);
                        GetComponentInChildren<SpriteRenderer>().sprite = itemSprite;
                        gc.CollectItem(orgID);
                    }
                }
            }
        }
    }

    public void UpdateItemID(int i)
    {
        if (i == 0 && ID == 0)
        {
            return;
        }
        ID = i;
        Texture itemTexture = gc.itemTextures[ID];
        Sprite itemSprite = Sprite.Create((Texture2D)itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), itemTexture.width * 1.55f);
        GetComponentInChildren<SpriteRenderer>().sprite = itemSprite;
    }

    public GameControllerScript gc;

    public Transform player;
    public int ID;

    public GameObject mapThing;
}