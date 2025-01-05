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
        int rng = Random.Range(0, sprite.Length - 1);
        collidr.size = collisionSize[rng];
        spriteR.sprite = sprite[rng];
        float rng2 = Random.Range(1.9f - ((1 - player.healthMeter.value) / 5), 2.5f);
        transform.localScale = new Vector3(rng2, rng2, 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, (1 - player.speed) * Time.deltaTime, 0);
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
