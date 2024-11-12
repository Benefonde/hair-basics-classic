using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DicordManager : MonoBehaviour
{
    Discord.Discord discord;
    void Start()
    {
        //if (SceneManager.GetActiveScene().name == "School" || SceneManager.GetActiveScene().name == "thing")
        //{
         //   gc = FindObjectOfType<GameControllerScript>();
        //}
        discord = new Discord.Discord(1305985185585827892, (ulong)Discord.CreateFlags.NoRequireDiscord);
        ChangeActivity();
    }

    void OnDisable()
    {
        discord.Dispose();
    }

    public void ChangeActivity()
    {
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = "Collecting Dwaynes",
            Details = "Playing a mode",
            Assets =
            {
                LargeImage = "panino",
                LargeText = "Story Mode"
            },
            Party =
            {
                Id = "shutupnerd",
                Size =
                {
                    CurrentSize = 7,
                    MaxSize = 17
                }
            }
        };

        activityManager.UpdateActivity(activity, (res) =>
        {

        });
    }

    void Update()
    {
        discord.RunCallbacks();
    }

    GameControllerScript gc;
}
