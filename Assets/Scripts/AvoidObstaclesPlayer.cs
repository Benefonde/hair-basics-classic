using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using FluidMidi;

public class AvoidObstaclesPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(SummonObstacle), 3);
        aud = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (speed < 30)
        {
            speed += 0.75f * Time.deltaTime;
        }
        speedometer.value = Mathf.FloorToInt(speed * 2);
        if (speed >= 28.5f)
        {
            score += speed * 6 * Time.deltaTime;
        }
        score += speed / 7 * Time.deltaTime;
        string a = "<color=white>";
        if (speed <= 28.5f && speed >= 0)
        {
            a = "<color=white>";
            speedometer.GetComponent<ShakyCamScript>().enabled = false;
        }
        if (speed < 0)
        {
            a = "<color=red>";
        }
        if (speed >= 28.5f)
        {
            a = "<color=green>";
            speedometer.GetComponent<ShakyCamScript>().enabled = true;
        }
        scoreText.text = $"Score: {a}{Mathf.RoundToInt(score)}";

        x += (Input.GetAxis("Strafe") * speed) * (Time.deltaTime);
        y += (Input.GetAxis("Forward") * speed) * (Time.deltaTime);

        TotallyRealClampingBroTrustMe(); // Mathf.Clamp didn't work

        transform.localPosition = new Vector2(x, y);

        fluidMidi.Tempo = Mathf.Clamp(speed / 20, 0.65f, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Obstacle(Clone)")
        {
            speed -= 3.5f;
            hp--;
            healthMeter.value--;
            Destroy(other.gameObject);
            anim.SetTrigger("hit");
            aud.PlayOneShot(hurt[Random.Range(0, 2)]);
            if (hp == 0)
            {
                if (score > PlayerPrefs.GetInt("obstaclesScore"))
                {
                    PlayerPrefs.SetInt("obstaclesScore", Mathf.RoundToInt(score));
                }
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    void TotallyRealClampingBroTrustMe()
    {
        if (x > 4.75f)
        {
            x = 4.75f;
        }
        if (x < -4.75f)
        {
            x = -4.75f;
        }
        if (y > 5.75f)
        {
            y = 5.75f;
        }
        if (y < -3.75f)
        {
            y = -3.75f;
        }
        if (speed > 30)
        {
            speed = 30;
        }
        if (speed < 1)
        {
            speed = 1;
        }
    }

    void SummonObstacle()
    {
        GameObject a = Instantiate(obstacle, new Vector2(Random.Range(-5.5f, 5.5f), 8), Quaternion.identity);
        a.gameObject.SetActive(true);
        if (speed >= 22.5f)
        {
            Invoke(nameof(SummonObstacle), Random.Range(0.2f, 0.75f));
        }
        else
        {
            Invoke(nameof(SummonObstacle), Random.Range(0.4f, 1.4f));
        }
    }

    public float speed = 2;
    int hp = 10;
    float score = 0;

    public Slider speedometer;
    public TMP_Text scoreText;
    public Slider healthMeter;

    public GameObject obstacle;

    [SerializeField]
    float x = 0;
    [SerializeField]
    float y = -3;

    Animator anim;
    AudioSource aud;
    public AudioClip[] music;
    public AudioClip[] hurt;

    public SongPlayer fluidMidi;
}
