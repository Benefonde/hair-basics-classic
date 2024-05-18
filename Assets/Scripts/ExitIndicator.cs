using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class ExitIndicator : MonoBehaviour
{
    public Transform player;
    public Transform[] exits;
    public TMP_Text text;

    public float offset;

    void Update()
    {
        Transform nearestExit = GetNearestExit();
        Vector3 directionToExit = Vector3.zero;

        if (nearestExit != null)
        {
            directionToExit = nearestExit.position - player.position;
            directionToExit.y = 0f;

            float angleToExit = Vector3.SignedAngle(player.forward, directionToExit, Vector3.up);

            transform.rotation = Quaternion.Euler(0f, -angleToExit, 0f);

            float distance = CalculateDistance(player.position, nearestExit.position);
            text.text = $"NEAREST EXIT: {Mathf.RoundToInt(distance / 10)}m";
        }
    }

    Transform GetNearestExit()
    {
        Transform nearestExit = null;
        float minDistance = float.MaxValue;

        foreach (Transform exit in exits)
        {
            if (exit.position.y == 0f)
            {
                float distance = Vector3.Distance(player.position, exit.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestExit = exit;
                }
            }
        }

        return nearestExit;
    }

    float CalculateDistance(Vector3 start, Vector3 end)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path);

        float distance = 0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }

        return distance;
    }
}
