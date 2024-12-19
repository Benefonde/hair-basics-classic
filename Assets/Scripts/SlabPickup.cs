using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlabPickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.transform.gameObject == transform.gameObject & Vector3.Distance(this.player.position, base.transform.position) < 10f & Cursor.lockState == CursorLockMode.Locked)
                {
                    if (gc.mode != "story")
                    {
                        gameObject.SetActive(false);
                    }
                    if (this.gc.item[0] == 0 | this.gc.item[1] == 0 | this.gc.item[2] == 0 || this.gc.item[3] == 0)
                    {
                        if (ID != 0)
                        {
                            gc.CollectItem(ID);
                            ID = 0;
                            gc.CurseOfRa();
                            Texture itemTexture = gc.itemTextures[0];
                            Sprite itemSprite = Sprite.Create((Texture2D)itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), itemTexture.width * 1.55f);
                            GetComponentInChildren<SpriteRenderer>().sprite = itemSprite;
                            return;
                        }
                        if (GetComponentInChildren<SpriteRenderer>().sprite.texture == gc.itemTextures[0] && gc.item[gc.itemSelected] != 0)
                        {
                            ID = gc.item[gc.itemSelected];
                            Texture itemTexture = gc.itemTextures[ID];
                            Sprite itemSprite = Sprite.Create((Texture2D)itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), itemTexture.width * 1.55f);
                            GetComponentInChildren<SpriteRenderer>().sprite = itemSprite;
                            gc.ResetItem();
                            if (ID == 25)
                            {
                                gc.UndoCurse();
                            }
                            else
                            {
                                gc.CurseOfRa();
                            }
                            return;
                        }
                    }
                    else
                    {
                        int orgID = ID;
                        ID = gc.item[gc.itemSelected];
                        Texture itemTexture = gc.itemTextures[ID];
                        Sprite itemSprite = Sprite.Create((Texture2D)itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), itemTexture.width * 1.55f);
                        GetComponentInChildren<SpriteRenderer>().sprite = itemSprite;
                        gc.CollectItem(orgID);
                        if (ID == 25)
                        {
                            gc.UndoCurse();
                        }
                        else if (ID != 25)
                        {
                            gc.CurseOfRa();
                        }
                    }
                
                }
            }
        }
    }

    public GameControllerScript gc;

    AudioSource aud;

    public Transform player;
    public int ID;
}