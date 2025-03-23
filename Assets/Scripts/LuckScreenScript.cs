using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckScreenScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        rollers[0].sprite = rollResults[number];
        if (rollers[0].sprite == rollResults[2] || rollers[0].sprite == rollResults[3])
        {
            StartCoroutine(Roll_Fail());
        }
        else
        {
            if (Random.Range(1, 16) == 7)
            {
                StartCoroutine(Roll_ItemSuccess(number));
            }
            else
            {
                StartCoroutine(Roll_ItemFailure());
            }
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
            yield return new WaitForSeconds(1);
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
        //wow mayan u won
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
    public SpriteRenderer[] rollers;

    AudioSource aud;
    public AudioClip tch;
    public AudioClip lose;
}
