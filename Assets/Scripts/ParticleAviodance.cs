using UnityEngine;

public class ParticleAviodance : MonoBehaviour
{
    public Transform playerFace;
    public float avoidanceRadius = 2f;
    public float avoidanceForce = 1f; 

    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(playerFace.position, avoidanceRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Particle"))
            {
                Vector3 avoidanceDirection = (collider.transform.position - playerFace.position).normalized;
                collider.GetComponent<Rigidbody>().AddForce(avoidanceDirection * avoidanceForce, ForceMode.Force);
            }
        }
    }
}
