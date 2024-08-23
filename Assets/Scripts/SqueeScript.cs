using UnityEngine;

public class SqueeScript : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(Die), 300);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
