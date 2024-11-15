using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DicordManager : MonoBehaviour
{
    public string state;
    public string details;
    public string largeImage;
    public string largeText;
    public string partyId;
    public int size;
    public int maxSize;

    Discord.Discord discord;

    // considering if i should actually do this or not

    /*void Start()
    {
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
            State = state,
            Details = details,
            Assets =
            {
                LargeImage = largeImage,
                LargeText = largeText
            },
            Party =
            {
                Id = partyId,
                Size =
                {
                    CurrentSize = size,
                    MaxSize = maxSize
                }
            }
        };

        activityManager.UpdateActivity(activity, (res) =>
        {
            // 
        });
    }

    void Update()
    {
        discord.RunCallbacks();
    }*/
}
