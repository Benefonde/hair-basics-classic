using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntranceBuilder : EditorWindow
{
    bool containsCeiling = true;
    int direction;
    Material floor;
    Material wall;
    Material ceiling;
    Vector3 entrancePos;
    Transform parent;
    bool busy;

    [MenuItem("CM Tools/Entrance Builder")]
    public static void ShowWindow()
    {
        GetWindow(typeof(EntranceBuilder));
    }

    private void OnGUI()
    {
        if (floor == null && TheBCMHT_Helper.getSampleFloor(false) != null)
        {
            floor = TheBCMHT_Helper.getSampleFloor(false);
        }
        if (wall == null && TheBCMHT_Helper.getSampleWall(false) != null)
        {
            wall = TheBCMHT_Helper.getSampleWall(false);
        }
        if (ceiling == null && TheBCMHT_Helper.getSampleCeiling(false) != null)
        {
            ceiling = TheBCMHT_Helper.getSampleCeiling(false);
        }
        containsCeiling = EditorGUILayout.Toggle("Contains ceiling:", containsCeiling);
        direction = EditorGUILayout.IntField("Direction:", direction);
        floor = (Material)EditorGUILayout.ObjectField("Floor material:", floor, typeof(Material), false);
        wall = (Material)EditorGUILayout.ObjectField("Wall material:", wall, typeof(Material), false);
        ceiling = (Material)EditorGUILayout.ObjectField("Ceiling material:", ceiling, typeof(Material), false);
        entrancePos = EditorGUILayout.Vector3Field("Position:", entrancePos);
        parent = (Transform)EditorGUILayout.ObjectField("Parent:", parent, typeof(Transform), true);
        if (!busy && GUILayout.Button("Build entrance"))
        {
            BuildEntrance();
        }
    }

    private void BuildEntrance()
    {
        busy = true;
        GameObject entranceParentObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        DestroyImmediate(entranceParentObject.GetComponent<Collider>());
        DestroyImmediate(entranceParentObject.GetComponent<MeshRenderer>());
        DestroyImmediate(entranceParentObject.GetComponent<MeshFilter>());
        entranceParentObject.name = "Entrance";
        Transform entranceParent = entranceParentObject.transform;
        entranceParent.parent = parent;
        List<GameObject> tiles = new List<GameObject>();
        string[][] tilesPath = new string[][]
        {
            new string[]
            {
                "Tiles/Corner_0",
                "Tiles/Open",
                "Tiles/Corner_3"
            },
            new string[]
            {
                "Tiles/Corner_1",
                "Tiles/Open",
                "Tiles/Corner_0"
            },
            new string[]
            {
                "Tiles/Corner_2",
                "Tiles/Open",
                "Tiles/Corner_1"
            },
            new string[]
            {
                "Tiles/Corner_3",
                "Tiles/Open",
                "Tiles/Corner_2"
            }
        };
        int[] directions = new int[]
        {
            1,
            2,
            3,
            0
        };
        tiles.Add(Instantiate(Resources.Load<GameObject>(tilesPath[direction][0]), entrancePos - TheBCMHT_Helper.getVector3FromDir(directions[direction]) * 10f, Quaternion.identity, entranceParent));
        tiles.Add(Instantiate(Resources.Load<GameObject>(tilesPath[direction][1]), entrancePos, Quaternion.identity, entranceParent));
        tiles.Add(Instantiate(Resources.Load<GameObject>(tilesPath[direction][2]), entrancePos + TheBCMHT_Helper.getVector3FromDir(directions[direction]) * 10f, Quaternion.identity, entranceParent));
        foreach (GameObject tile in tiles)
        {
            ApplyMaterials(tile);
        }
        busy = false;
    }

    private void ApplyMaterials(GameObject tile)
    {
        foreach (MeshRenderer meshRenderer in tile.GetComponentsInChildren<MeshRenderer>())
        {
            if (meshRenderer.gameObject.name.Contains("Floor"))
            {
                meshRenderer.sharedMaterial = this.floor;
            }
            else if (meshRenderer.gameObject.name.Contains("Wall"))
            {
                meshRenderer.sharedMaterial = this.wall;
            }
            else if (meshRenderer.gameObject.name.Contains("Ceiling"))
            {
                meshRenderer.sharedMaterial = this.ceiling;
            }
        }
    }
}
