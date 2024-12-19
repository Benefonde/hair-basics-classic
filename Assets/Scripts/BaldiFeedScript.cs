using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000020 RID: 32
public class BaldiFeedScript : MonoBehaviour
{
    // Token: 0x060000EB RID: 235 RVA: 0x00005F8C File Offset: 0x0000418C
    private void OnEnable()
    {
        image = base.GetComponent<Image>();
    }

    // Token: 0x060000EC RID: 236 RVA: 0x00005FB0 File Offset: 0x000041B0
    private void Update()
    {
        if (au.dbValue > -30f & au.dbValue < -24f)
        {
            image.sprite = images[1];
            return;
        }
        if (au.dbValue > -24f & au.dbValue < -18f)
        {
            image.sprite = images[2];
            return;
        }
        if (au.dbValue > -18f & au.dbValue < -12f)
        {
            image.sprite = images[3];
            return;
        }
        if (au.dbValue > -12f & au.dbValue < -6f)
        {
            image.sprite = images[4];
            return;
        }
        if (au.dbValue > -6f & au.dbValue < 0f)
        {
            image.sprite = images[5];
            return;
        }
        if (au.dbValue > 0f)
        {
            image.sprite = images[6];
            return;
        }
        image.sprite = images[0];
    }

    // Token: 0x040000CE RID: 206
    private Image image;

    // Token: 0x040000CF RID: 207
    public Sprite[] images = new Sprite[7];

    // Token: 0x040000D2 RID: 210
    public bool completed;

    // Token: 0x040000D3 RID: 211
    public AudioSourceLoudness au;
}