using UnityEngine;

public class SqueeScript : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(Die), dieTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name != "Player")
        {
            print($"{other.transform.name} collided with spray");
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public float dieTime = 300;
}
