using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        collidr = GetComponent<BoxCollider>();
        spriteR = GetComponent<SpriteRenderer>();
        int rng = Random.Range(0, sprite.Length);
        collidr.size = collisionSize[rng];
        spriteR.sprite = sprite[rng];
        float rng2 = Random.Range(1.9f - ((10 - player.healthMeter.value) / 10), 2.5f - ((10 - player.healthMeter.value) / 10));
        Mathf.Clamp(rng2, 0.4f, 2.5f);
        print((1 - player.healthMeter.value) / 5);
        transform.localScale = new Vector3(rng2, rng2, 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, (1 - (player.speed / 1.25f)) * Time.deltaTime, 0);
        if (transform.position.y <= -6)
        {
            Destroy(gameObject);
        } 
    }

    public Vector3[] collisionSize;
    public Sprite[] sprite;

    BoxCollider collidr;
    SpriteRenderer spriteR;

    public AvoidObstaclesPlayer player;
}
