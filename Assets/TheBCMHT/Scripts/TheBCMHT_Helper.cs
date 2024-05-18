using System;
using UnityEngine;

public static class TheBCMHT_Helper
{
    public static int getMaxValueFromTileType(TileType tileType)
    {
        int maxVal = 1000;
        if (tileType == TileType.Corner || tileType == TileType.End)
        {
            maxVal = 1;
        }
        return maxVal;
    }

    public static Vector3 getVector3FromDir(int direction)
    {
        Vector3[] vector3s = new Vector3[]
        {
            Vector3.forward,
            Vector3.right,
            Vector3.back,
            Vector3.left
        };
        return vector3s[direction];
    }

    public static Material getSampleFloor(bool v132)
    {
        return Resources.Load<Material>("Samples/" + (v132 ? "V1.3.2/Ph_Floor" : "Ph_Floor"));
    }

    public static Material getSampleWall(bool v132)
    {
        return Resources.Load<Material>("Samples/" + (v132 ? "V1.3.2/Ph_Wall" : "Ph_Wall"));
    }

    public static Material getSampleCeiling(bool v132)
    {
        return Resources.Load<Material>("Samples/" + (v132 ? "V1.3.2/Ph_Ceiling" : "Ph_Ceiling"));
    }

    public static int intClampBySize(int value, int size)
    {
        return Mathf.Clamp(value, 0, size - 1);
    }

    public static GameObject getCornMazeFlag()
    {
        return Resources.Load<GameObject>("Environment/Flag");
    }

    public static Sprite getCornMazeFlagSampleSprite()
    {
        return Resources.Load<Sprite>("Samples/Placeholder_Flag");
    }

    public static GameObject getCornMazeSign()
    {
        return Resources.Load<GameObject>("Environment/Sign");
    }

    public static Sprite getCornMazeSignSampleSprite()
    {
        return Resources.Load<Sprite>("Samples/Placeholder_Sign");
    }

    public static RoomChildDat[,] createRoomData(int sizeX, int sizeY)
    {
        if (sizeX < 2 || sizeY < 2) { Debug.LogWarning("failed to create a room data size (" + sizeX + ", " + sizeY + ")"); return null; }
        RoomChildDat[,] dats = new RoomChildDat[sizeX, sizeY];
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                dats[x, y] = new RoomChildDat();
                if (x == 0 && y == 0)
                {
                    dats[x, y].tileType = TileType.Corner;
                    dats[x, y].tileIndex = 0;
                }
                else if (x == 0 && y == sizeY - 1)
                {
                    dats[x, y].tileType = TileType.Corner;
                    dats[x, y].tileIndex = 1;
                }
                else if (x == sizeX - 1 && y == 0)
                {
                    dats[x, y].tileType = TileType.Corner;
                    dats[x, y].tileIndex = 3;
                }
                else if (x == sizeX - 1 && y == sizeY - 1)
                {
                    dats[x, y].tileType = TileType.Corner;
                    dats[x, y].tileIndex = 2;
                }
                else if (x == 0 && y > 0 && y <= sizeY - 2)
                {
                    dats[x, y].tileType = TileType.Single;
                    dats[x, y].tileIndex = 3;
                }
                else if (x > 0 && x <= sizeX - 2 && y == sizeY - 1)
                {
                    dats[x, y].tileType = TileType.Single;
                    dats[x, y].tileIndex = 0;
                }
                else if (x > 0 && x <= sizeX - 2 && y == 0)
                {
                    dats[x, y].tileType = TileType.Single;
                    dats[x, y].tileIndex = 2;
                }
                else if (x == sizeX - 1 && y > 0 && y <= sizeY - 2)
                {
                    dats[x, y].tileType = TileType.Single;
                    dats[x, y].tileIndex = 1;
                }
                else if (x > 0 && y > 0 && x <= sizeX - 2 && y <= sizeY - 2)
                {
                    dats[x, y].tileType = TileType.Open;
                }
            }
        }
        return dats;
    }
}
