using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    void Start()
    {
        animator = GetComponent<Animator>();
        originalYposition = transform.position.y;
        if (collect == Type.Topping || collect == Type.BigTopping)
        {
            int typeOfFoodYoureGonnaBe = Mathf.FloorToInt(Random.Range(1, typeAnim.Length));

            animator.SetInteger("type", typeAnim[typeOfFoodYoureGonnaBe]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gc.pss.AddPoints(scoreGive, 0.2f);
            gc.audioDevice.PlayOneShot(collectSound);
            if (collect == Type.Topping || collect == Type.BigTopping)
            {
                gc.tc.collectedToppings++;
                if (collect == Type.BigTopping)
                {
                    gc.player.stamina += gc.player.maxStamina / 12;
                    gc.player.health += 0.5f;
                }
                else
                {
                    gc.player.stamina += gc.player.maxStamina / 28;
                    gc.player.health += 0.02f;
                }
                gameObject.SetActive(false);
            }
            if (collect == Type.PizzaTime || collect == Type.BigPizzaTime)
            {
                if (collect == Type.BigPizzaTime)
                {
                    gc.player.stamina += gc.player.maxStamina / 45;
                    gc.player.health += 0.4f;
                    if (gc.laps >= 3)
                    {
                        gc.timer.timeLeft += 1.35f;
                    }
                }
                else
                {
                    gc.player.stamina += gc.player.maxStamina / 60;
                    gc.player.health += 0.1f;
                    if (gc.laps >= 3)
                    {
                        gc.timer.timeLeft += 0.4f;
                    }
                }
                transform.Translate(new Vector3(0, -10, 0));
            }
        }
    }

    public void TpBackUp()
    {
        transform.position = new Vector3(transform.position.x, originalYposition, transform.position.z);
    }

    public int[] typeAnim;

    private float originalYposition;

    private Animator animator;

    public GameControllerScript gc;

    public enum Type
    {
        Topping,
        BigTopping,
        PizzaTime,
        BigPizzaTime
    }

    public Type collect;

    public int scoreGive;

    public AudioClip collectSound;
}
