using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToScene : MonoBehaviour
{
    void Update()
    {
        if (!noWait)
        {
            if (seconds > 0)
            {
                seconds -= 1f * Time.deltaTime;
            }
            if (seconds < 0)
            {
                if (inputNeeded && Input.anyKeyDown)
                {
                    if (asyncLoad)
                    {
                        SceneManager.LoadSceneAsync(scene);
                    }
                    else
                    {
                        SceneManager.LoadScene(scene);
                    }
                }
                else if (!inputNeeded)
                {
                    if (asyncLoad)
                    {
                        SceneManager.LoadSceneAsync(scene);
                    }
                    else
                    {
                        SceneManager.LoadScene(scene);
                    }
                }
            }
        }
    }

    public void SendToScene(string scenee)
    {
        SceneManager.LoadScene(scenee);
    }

    public float seconds;
    public string scene;
    public bool inputNeeded;
    public bool asyncLoad;

    public bool noWait;
}
