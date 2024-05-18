using System;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif

public class CorrectPlaneQuad : MonoBehaviour
{
    private void Start()
    {
        foreach (MeshFilter mf in FindObjectsOfType<MeshFilter>())
        {
            string objectName = mf.gameObject.name;
            if (Array.IndexOf(exclusions, objectName) > -1)
            {
                continue;
            }
            MeshRenderer mr = mf.gameObject.GetComponent<MeshRenderer>();
#if UNITY_EDITOR
            string mfn = "";
            if (mf.sharedMesh != null)
            {
                mfn = mf.sharedMesh.name;
            }
            else
            {
                Debug.LogWarning("Mesh Filter with NULL Mesh found, please make sure your Mesh Filters all have Meshes!" + " Mesh Filter was attached to Game Object " + objectName);
                continue;
            }
#else
            string mfn = "";
            if (mf.mesh != null)
            {
                mfn = mf.mesh.name;
            }
            else
            {
                Debug.LogWarning("Mesh Filter with NULL Mesh found, please make sure your Mesh Filters all have Meshes!" + " Mesh Filter was attached to Game Object " + objectName);
                continue;
            }
#endif
            if (mr == null || !(mfn.Contains("Plane")))
            {
                continue;
            }
            Mesh[] availableMeshes = Resources.FindObjectsOfTypeAll<Mesh>();
            Mesh quad = null;
            foreach (Mesh mesh in availableMeshes)
            {
                if (mesh.name != "Quad")
                {
                    continue;
                }
                quad = mesh;
            }
#if UNITY_EDITOR
            mf.sharedMesh = quad;
#else
            mf.mesh = quad;
#endif
            MeshCollider mc = mf.gameObject.GetComponent<MeshCollider>();
            if (mc != null)
            {
                mc.sharedMesh = quad;
            }
            Transform mt = mf.transform;
            Vector3 scale = mt.localScale;
            scale.x *= 10f;
            if (Mathf.Sign(scale.y) == -1f)
            {
                scale.y *= -10f;
                scale.z *= -1f;
            }
            else
            {
                scale.y *= 10f;
            }
            scale.x = Mathf.Round(scale.x);
            scale.y = Mathf.Round(scale.y);
            scale.z = Mathf.Round(scale.z);
            mt.localScale = scale;
            Vector3 rotation = mt.eulerAngles;
            if (rotation.x == 0f)
            {
                rotation.x += 90f;
            }
            else
            {
                rotation.x -= 90f;
            }
            rotation.y += 180f;
            rotation.x = Mathf.Round(rotation.x);
            rotation.y = Mathf.Round(rotation.y);
            mt.eulerAngles = rotation;
        }
    }

    //ADD EXCLUDED GAME OBJECT NAMES HERE!
    //THESE ARE CASE SENSITIVE AND MUST BE THE ENTIRE NAME!
    private static readonly string[] exclusions = new string[] { "Game Object Exclusion" };
}