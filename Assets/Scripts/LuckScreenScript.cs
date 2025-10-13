using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuckScreenScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    public void Roll()
    {
        isRolling = true;
        StartCoroutine(RealRoll());
    }

    IEnumerator RealRoll()
    {
        for (int i = 0; i < rollers.Length; i++)
        {
            rollers[i].sprite = rollResults[1];
        }
        yield return new WaitForSeconds(2);
        int number = Random.Range(2, 13);
        switch (TestMode)
        {
            default:
                rollers[0].sprite = rollResults[number];
                if (rollers[0].sprite == rollResults[2] || rollers[0].sprite == rollResults[3])
                {
                    StartCoroutine(Roll_Fail());
                }
                else
                {
                    if (Random.Range(1, 28) == 7)
                    {
                        StartCoroutine(Roll_ItemSuccess(number));
                    }
                    else
                    {
                        StartCoroutine(Roll_ItemFailure());
                    }
                }
                break;
            case 1:
                number = Random.Range(2, 3);
                rollers[0].sprite = rollResults[number];
                if (rollers[0].sprite == rollResults[2] || rollers[0].sprite == rollResults[3])
                {
                    StartCoroutine(Roll_Fail());
                }
                break;
            case 2:
                number = Random.Range(4, 13);
                rollers[0].sprite = rollResults[number];
                StartCoroutine(Roll_ItemSuccess(number));
                break;
            case 3:
                number = Random.Range(4, 13);
                rollers[0].sprite = rollResults[number];
                StartCoroutine(Roll_ItemFailure());
                break;
        }
    }
    IEnumerator Roll_Fail()
    {
        if (rollers[0].sprite == rollResults[3])
        {
            aud.PlayOneShot(tch);
            yield return new WaitForSeconds(tch.length);
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(0.4f, 1.2f));
        }
        if (rollers[0].sprite == rollResults[2] || rollers[0].sprite == rollResults[3])
        {
            rollers[1].sprite = rollers[0].sprite;
            if (rollers[1].sprite == rollResults[3])
            {
                aud.PlayOneShot(tch);
            }
            yield return new WaitForSeconds(tch.length);
            rollers[2].sprite = rollers[0].sprite;
            if (rollers[2].sprite == rollResults[3])
            {
                aud.PlayOneShot(tch);
                yield return new WaitForSeconds(tch.length);
                aud.PlayOneShot(lose);
                yield return new WaitForSeconds(lose.length);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
        isRolling = false;
    }
    IEnumerator Roll_ItemSuccess(int ID)
    {
        yield return new WaitForSeconds(3);
        rollers[1].sprite = rollers[0].sprite;
        yield return new WaitForSeconds(4);
        rollers[2].sprite = rollers[0].sprite;
        yield return new WaitForSeconds(0.5f);
        PlayerPrefs.SetInt($"itemWon{offset}", itemRollResults[ID]);
        itemSlots[offset].texture = itemRollSlotResults[ID];
        offset++;
        isRolling = false;
    }
    IEnumerator Roll_ItemFailure()
    {
        yield return new WaitForSeconds(3);
        if (Random.Range(1, 4) == 3)
        {
            rollers[1].sprite = rollResults[Random.Range(2, 13)];
            yield return new WaitForSeconds(Random.Range(0.4f, 1.2f));
            rollers[2].sprite = rollResults[Random.Range(2, 13)];
            while (rollers[2].sprite == rollers[1].sprite) // you can not win when Roll_ItemFailure()
            {
                rollers[2].sprite = rollResults[Random.Range(2, 13)];
            }
        }
        else
        {
            rollers[1].sprite = rollers[0].sprite;
            yield return new WaitForSeconds(4);
            rollers[2].sprite = rollResults[Random.Range(2, 13)];
            while (rollers[2].sprite == rollers[1].sprite) // you can not win when Roll_ItemFailure()
            {
                rollers[2].sprite = rollResults[Random.Range(2, 13)];
            }
        }
        yield return new WaitForSeconds(0.5f);
        isRolling = false;
    }

    public bool isRolling;

    public Sprite[] rollResults;
    public int[] itemRollResults;
    public SpriteRenderer[] rollers;

    AudioSource aud;
    public AudioClip tch;
    public AudioClip lose;

    public int offset;

    public RawImage[] itemSlots;
    public Texture[] itemRollSlotResults;

    public int TestMode;
}
