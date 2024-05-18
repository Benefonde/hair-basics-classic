using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell
{
    public void InitializeCell()
    {
        if (this.initialized)
        {
            return;
        }
        if (Resources.Load("Tiles/Full") == null)
        {
            Debug.LogWarning("Initialization failed, Tiles/Full doesn't exist");
            return;
        }
        this.availableDirs.Add(0);
        this.availableDirs.Add(1);
        this.availableDirs.Add(2);
        this.availableDirs.Add(3);
        MeshRenderer[] array = new MeshRenderer[4];
        GameObject _tile = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Tiles/Full"), this.gridPosition, Quaternion.identity, this.parent);
        foreach (MeshRenderer meshRenderer in _tile.GetComponentsInChildren<MeshRenderer>())
        {
            if (meshRenderer.gameObject.name == "Wall")
            {
                array[0] = meshRenderer;
            }
            else if (meshRenderer.gameObject.name == "Wall (1)")
            {
                array[1] = meshRenderer;
            }
            else if (meshRenderer.gameObject.name == "Wall (2)")
            {
                array[2] = meshRenderer;
            }
            else if (meshRenderer.gameObject.name == "Wall (3)")
            {
                array[3] = meshRenderer;
            }
            else if (meshRenderer.gameObject.name.Contains("Floor"))
            {
                this.floor = meshRenderer;
            }
            else if (meshRenderer.gameObject.name.Contains("Ceiling"))
            {
                this.ceiling = meshRenderer;
            }
        }
        for (int i = 0; i < array.Length; i++)
        {
            this.walls.Add(array[i]);
        }
    }

    public void RemoveCeiling()
    {
        if (this.ceiling != null)
        {
            UnityEngine.Object.DestroyImmediate(this.ceiling.gameObject);
        }
    }

    public void ApplyMaterials(Material ceiling, Material wall, Material floor)
    {
        foreach (MeshRenderer meshRenderer in this.walls)
        {
            meshRenderer.sharedMaterial = wall;
        }
        this.floor.sharedMaterial = floor;
        if (this.ceiling != null)
        {
            this.ceiling.sharedMaterial = ceiling;
        }
    }

    public void DestroyWall(int direction)
    {
        if (this.availableDirs.Contains(direction))
        {
            MeshRenderer targetedWall = this.walls[this.GetIntegerFromDir(direction)];
            this.walls.Remove(targetedWall);
            UnityEngine.Object.DestroyImmediate(targetedWall.gameObject);
            this.availableDirs.Remove(direction);
        }
    }

    private int GetIntegerFromDir(int direction)
    {
        for (int i = 0; i < this.availableDirs.Count; i++)
        {
            if (this.availableDirs[i] == direction)
            {
                return i;
            }
        }
        return 0;
    }

    private List<int> availableDirs = new List<int>();

    private List<MeshRenderer> walls = new List<MeshRenderer>();

    private MeshRenderer floor;

    private MeshRenderer ceiling;

    public bool initialized;

    public Vector3 gridPosition;

    public Transform parent;

    public bool visited;
}
