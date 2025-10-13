using System;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    void Start()
    {
        aud = GetComponent<AudioSource>();
        if (pickupType == Type.Sword)
        {
            GetComponentInChildren<SpriteRenderer>().color = swordType.color;
            durability = swordType.durability;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f));
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.transform.gameObject == transform.gameObject & Vector3.Distance(this.player.position, base.transform.position) < 10f & Cursor.lockState == CursorLockMode.Locked)
                {
                    if (pickupType == Type.Item)
                    {
                        if (this.gc.item[0] == 0 | this.gc.item[1] == 0 | this.gc.item[2] == 0 || this.gc.item[3] == 0)
                        {
                            if (gc.mode != "endless")
                            {
                                this.gc.CollectItem(ID);
                                raycastHit.transform.gameObject.SetActive(false);
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
                    else if (pickupType == Type.Sword)
                    {
                        if (ss.swordType != ss.none)
                        {
                            Sword orgSword = swordType;
                            int orgDurability = durability;
                            durability = ss.durability;
                            ss.durability = orgDurability;
                            swordType = ss.swordType;
                            GetComponentInChildren<SpriteRenderer>().color = swordType.color;
                            ss.ChangeSword(orgSword);
                        }
                        else
                        {
                            raycastHit.transform.gameObject.SetActive(false);
                            ss.durability = durability;
                            ss.ChangeSword(swordType);
                        }
                    }
                    else if (pickupType == Type.Treasure)
                    {
                        gc.AddTp(1.28f);
                        gc.playerScript.stamina += 28 * (gc.playerScript.maxStamina / 100);
                        raycastHit.transform.gameObject.SetActive(false);
                    }
                    else if (pickupType == Type.Pizza)
                    {
                        gc.audioDevice.PlayOneShot(eat);
                        gc.playerScript.stamina += 47 * (gc.playerScript.maxStamina / 100);
                        raycastHit.transform.gameObject.SetActive(false);
                        gc.paninoTv.count -= 1;
                        gc.paninoTv.pizzaHudText.text = gc.paninoTv.count.ToString();
                        if (gc.mode == "pizza")
                        {
                            gc.pss.AddPoints(150, 0.5f);
                        }
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

    [Header("Generic variables")]
    public GameControllerScript gc;

    AudioSource aud;

    public Transform player;

    [Header("Item variable(s)")]
    public int ID;

    [Header("Sword variables")]
    public Sword swordType;
    public int durability;
    public SwordScript ss;

    [Header("Pizza variable(s)")]
    public AudioClip eat;

    public enum Type
    {
        Item,
        Sword,
        Treasure,
        Pizza
    };

    [Header("Type of pickup")]
    public Type pickupType = Type.Item;
}