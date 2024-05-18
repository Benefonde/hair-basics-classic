using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class RoomBuilder : EditorWindow
{
    RoomChildDat[,] dats;
    int sizeX = 2;
    int sizeY = 2;
    bool busy;
    Vector3 roomPosition;
    Material floorMateral;
    Material wallMaterial;
    Material ceilingMaterial;
    Transform roomParent;
    bool containsCeiling = true;

    [MenuItem("CM Tools/Room Builder")]
    public static void ShowWindow()
    {
        GetWindow(typeof(RoomBuilder));
    }

    private void OnGUI()
    {
        if (floorMateral == null && TheBCMHT_Helper.getSampleFloor(false) != null)
        {
            floorMateral = TheBCMHT_Helper.getSampleFloor(false);
        }
        if (wallMaterial == null && TheBCMHT_Helper.getSampleWall(false) != null)
        {
            wallMaterial = TheBCMHT_Helper.getSampleWall(false);
        }
        if (ceilingMaterial == null && TheBCMHT_Helper.getSampleCeiling(false) != null)
        {
            ceilingMaterial = TheBCMHT_Helper.getSampleCeiling(false);
        }
        if (sizeX * sizeY >= 2500)
        {
            GUILayout.Label("Not recommended", EditorStyles.boldLabel);
        }
        sizeX = Mathf.Clamp(EditorGUILayout.IntField("Size x:", sizeX), 2, 100);
        sizeY = Mathf.Clamp(EditorGUILayout.IntField("Size y:", sizeY), 2, 100);
        if (!busy && GUILayout.Button((!HasRoomChildDatas ? "Create datas" : "New datas")))
        {
            dats = null;
            CreateRoomChildDatas();
        }
        if (HasRoomChildDatas)
        {
            roomPosition = EditorGUILayout.Vector3Field("Room position:", roomPosition);
            floorMateral = (Material)EditorGUILayout.ObjectField("Floor material:", floorMateral, typeof(Material), false);
            wallMaterial = (Material)EditorGUILayout.ObjectField("Wall material:", wallMaterial, typeof(Material), false);
            ceilingMaterial = (Material)EditorGUILayout.ObjectField("Ceiling material:", ceilingMaterial, typeof(Material), false);
            containsCeiling = EditorGUILayout.Toggle("Contains ceiling:", containsCeiling);
            roomParent = (Transform)EditorGUILayout.ObjectField("Room parent:", roomParent, typeof(Transform), true);
            if (!this.busy && GUILayout.Button("Build room"))
            {
                BuildRoom();
            }
        }
    }
    
    private void CreateRoomChildDatas()
    {
        this.dats = TheBCMHT_Helper.createRoomData(sizeX, sizeY);
    }

    private void BuildRoom()
    {
        busy = true;
        GameObject parentObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        DestroyImmediate(parentObject.GetComponent<Collider>());
        DestroyImmediate(parentObject.GetComponent<MeshRenderer>());
        DestroyImmediate(parentObject.GetComponent<MeshFilter>());
        parentObject.name = "Room";
        Transform parent = parentObject.transform;
        parent.parent = roomParent;
        for (int x = 0; x < dats.GetLength(0); x++)
        {
            for (int y = 0; y < dats.GetLength(1); y++)
            {
                Hashtable tilesPath = new Hashtable
                {
                    ["corner"] = "Tiles/Corner_",
                    ["single"] = "Tiles/Single_",
                    ["open"] = "Tiles/Open"
                };
                GameObject _tile = null;
                if (dats[x, y].tileType == TileType.Corner)
                {
                    _tile = Instantiate<GameObject>(Resources.Load<GameObject>((string)tilesPath["corner"] + dats[x, y].tileIndex), this.roomPosition + Vector3.forward * 10f * y + Vector3.right * 10f * x, Quaternion.identity, parent);
                }
                else if (dats[x, y].tileType == TileType.Single)
                {
                    _tile = Instantiate<GameObject>(Resources.Load<GameObject>((string)tilesPath["single"] + dats[x, y].tileIndex), this.roomPosition + Vector3.forward * 10f * y + Vector3.right * 10f * x, Quaternion.identity, parent);
                }
                else if (dats[x, y].tileType == TileType.Open)
                {
                    _tile = Instantiate<GameObject>(Resources.Load<GameObject>((string)tilesPath["open"]), this.roomPosition + Vector3.forward * 10f * y + Vector3.right * 10f * x, Quaternion.identity, parent);
                }
                if (!containsCeiling)
                {
                    foreach (Transform transform in _tile.transform.GetComponentsInChildren<Transform>())
                    {
                        if (transform.gameObject.name.Contains("Ceiling"))
                        {
                            DestroyImmediate(transform.gameObject);
                        }
                    }
                }
                ApplyMaterials(_tile);
            }
        }
        busy = false;
    }

    private void ApplyMaterials(GameObject tile)
    {
        foreach (MeshRenderer meshRenderer in tile.GetComponentsInChildren<MeshRenderer>())
        {
            if (meshRenderer.gameObject.name.Contains("Floor"))
            {
                meshRenderer.sharedMaterial = this.floorMateral;
            }
            else if (meshRenderer.gameObject.name.Contains("Wall"))
            {
                meshRenderer.sharedMaterial = this.wallMaterial;
            }
            else if (meshRenderer.gameObject.name.Contains("Ceiling"))
            {
                meshRenderer.sharedMaterial = this.ceilingMaterial;
            }
        }
    }

    private bool HasRoomChildDatas
    {
        get
        {
            return dats != null;
        }
    }
}
