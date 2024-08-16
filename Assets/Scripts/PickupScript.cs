using System;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    void Start()
    {
        aud = GetComponent<AudioSource>();
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
                    if (this.gc.item[0] == 0 | this.gc.item[1] == 0 | this.gc.item[2] == 0 || this.gc.item[3] == 0)
                    {
                        if (gc.mode != "endless")
                        {
                            raycastHit.transform.gameObject.SetActive(false);
                            this.gc.CollectItem(ID);
                        }
                        else
                        {
                            raycastHit.transform.Translate(0, -10, 0);
                            this.gc.CollectItem(ID);
                            Invoke(nameof(EndlessRespawn), 300);
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

                    }
                }
            }
        }
    }

    void EndlessRespawn()
    {
        transform.Translate(0, 10, 0);
        aud.Play();
        FindObjectOfType<SubtitleManager>().Add3DSubtitle("An item respawned!", aud.clip.length, Color.white, transform);
    }

    public GameControllerScript gc;

    AudioSource aud;

    public Transform player;
    public int ID;
}