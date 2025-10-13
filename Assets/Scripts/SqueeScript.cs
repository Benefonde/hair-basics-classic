using UnityEngine;

public class SqueeScript : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(Die), dieTime);
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public float dieTime = 300;
}
