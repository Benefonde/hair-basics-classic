using System;
using UnityEditor;
using UnityEngine;

public class LibraryBuilder : EditorWindow
{
    int currentSize;
    int[] sizes = new int[]
    {
        15,
        20
    };
    bool randomSeed = true;
    int seed;
    Material floor;
    Material wall;
    Material ceiling;
    Transform parent;
    bool busy;
    MazeCell[,] cells;

    [MenuItem("CM Tools/Library Builder")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LibraryBuilder));
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
        randomSeed = EditorGUILayout.Toggle("Random seed:", randomSeed);
        if (!randomSeed)
        {
            seed = EditorGUILayout.IntField("Seed:", seed);
            if (Convert.ToInt64(seed) > 2147483615L)
            {
                seed = Convert.ToInt32(2147483615L);
            }
            else if (Convert.ToInt64(seed) < -2147483615L)
            {
                seed = Convert.ToInt32(-2147483615L);
            }
        }
        floor = (Material)EditorGUILayout.ObjectField("Floor material:", floor, typeof(Material), false);
        wall = (Material)EditorGUILayout.ObjectField("Wall material:", wall, typeof(Material), false);
        ceiling = (Material)EditorGUILayout.ObjectField("Ceiling material:", ceiling, typeof(Material), false);
        parent = (Transform)EditorGUILayout.ObjectField("Parent:", parent, typeof(Transform), true);
        if (!busy)
        {
            if (GUILayout.Button("Current size: " + sizes[currentSize]))
            {
                currentSize = (currentSize == 0 ? 1 : 0);
            }
            if (GUILayout.Button("Build library"))
            {
                if (randomSeed)
                {
                    this.seed = UnityEngine.Random.Range(-2147483648, 2147483647);
                }
                BuildLibrary();
            }
        }
    }

    private void BuildLibrary()
    {
        busy = true;
        UnityEngine.Random.InitState(this.seed);
        int _size = sizes[currentSize];
        cells = new MazeCell[_size, _size];
        GameObject libraryParentObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        DestroyImmediate(libraryParentObject.GetComponent<Collider>());
        DestroyImmediate(libraryParentObject.GetComponent<MeshRenderer>());
        DestroyImmediate(libraryParentObject.GetComponent<MeshFilter>());
        libraryParentObject.name = "Library";
        Transform libraryParent = libraryParentObject.transform;
        libraryParent.parent = parent;
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                cells[x, y] = new MazeCell();
                cells[x, y].gridPosition = new Vector3(10f * x, 0f, 10f * y);
                cells[x, y].parent = libraryParent;
                cells[x, y].InitializeCell();
            }
        }
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                cells[x, y].ApplyMaterials(this.ceiling, this.wall, this.floor);
            }
        }
        int _row = 0;
        int _column = 0;
        cells[_row, _column].visited = true;
        IL_0001:
        while (RouteAvailable(_row, _column))
        {
            int direction = UnityEngine.Random.Range(0, 4);
            if (direction == 0 && CellAvailable(_row, _column + 1))
            {
                cells[_row, _column].DestroyWall(0);
                cells[_row, _column + 1].DestroyWall(2);
                _column++;
            }
            else if (direction == 1 && CellAvailable(_row + 1, _column))
            {
                cells[_row, _column].DestroyWall(1);
                cells[_row + 1, _column].DestroyWall(3);
                _row++;
            }
            else if (direction == 2 && CellAvailable(_row, _column - 1))
            {
                cells[_row, _column].DestroyWall(2);
                cells[_row, _column - 1].DestroyWall(0);
                _column--;
            }
            else if (direction == 3 && CellAvailable(_row - 1, _column))
            {
                cells[_row, _column].DestroyWall(3);
                cells[_row - 1, _column].DestroyWall(1);
                _row--;
            }
            cells[_row, _column].visited = true;
        }
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                if (!cells[x, y].visited && CellNeighborsVisited(x, y))
                {
                    _row = x;
                    _column = y;
                    bool flag = false;
                    while (!flag)
                    {
                        int direction = UnityEngine.Random.Range(0, 4);
                        if (direction == 0 && _column < _size - 2 && cells[_row, _column + 1].visited)
                        {
                            cells[_row, _column].DestroyWall(0);
                            cells[_row, _column + 1].DestroyWall(2);
                            flag = true;
                        }
                        else if (direction == 1 && _row < _size - 2 && cells[_row + 1, _column].visited)
                        {
                            cells[_row, _column].DestroyWall(1);
                            cells[_row + 1, _column].DestroyWall(3);
                            flag = true;
                        }
                        else if (direction == 2 && _column > 0 && cells[_row, _column - 1].visited)
                        {
                            cells[_row, _column].DestroyWall(2);
                            cells[_row, _column - 1].DestroyWall(0);
                            flag = true;
                        }
                        else if (direction == 3 && _row > 0 && cells[_row - 1, _column].visited)
                        {
                            cells[_row, _column].DestroyWall(3);
                            cells[_row - 1, _column].DestroyWall(1);
                            flag = true;
                        }
                    }
                    cells[_row, _column].visited = true;
                    goto IL_0001;
                }
            }
        }
        int blankRoomPosition = 5;
        int blankSize = (_size == 15 ? 5 : 10);
        for (int x = blankRoomPosition; x < blankRoomPosition + blankSize; x++)
        {
            for (int y = blankRoomPosition; y < blankRoomPosition + blankSize; y++)
            {
                if (x == blankRoomPosition && y == blankRoomPosition)
                {
                    cells[x, y].DestroyWall(0);
                    cells[x, y].DestroyWall(1);
                }
                else if (x == blankRoomPosition && y > blankRoomPosition && y <= (blankRoomPosition + blankSize) - 2)
                {
                    cells[x, y].DestroyWall(0);
                    cells[x, y].DestroyWall(1);
                    cells[x, y].DestroyWall(2);
                }
                else if (x == blankRoomPosition && y == (blankRoomPosition + blankSize) - 1)
                {
                    cells[x, y].DestroyWall(1);
                    cells[x, y].DestroyWall(2);
                }
                else if (x > blankRoomPosition && x <= (blankRoomPosition + blankSize) - 2 && y == blankRoomPosition)
                {
                    cells[x, y].DestroyWall(0);
                    cells[x, y].DestroyWall(1);
                    cells[x, y].DestroyWall(3);
                }
                else if (x > blankRoomPosition && x <= (blankRoomPosition + blankSize) - 2 && y > blankRoomPosition && y <= (blankRoomPosition + blankSize) - 2)
                {
                    cells[x, y].DestroyWall(0);
                    cells[x, y].DestroyWall(1);
                    cells[x, y].DestroyWall(2);
                    cells[x, y].DestroyWall(3);
                }
                else if (x > blankRoomPosition && x <= (blankRoomPosition + blankSize) - 2 && y == (blankRoomPosition + blankSize) - 1)
                {
                    cells[x, y].DestroyWall(1);
                    cells[x, y].DestroyWall(2);
                    cells[x, y].DestroyWall(3);
                }
                else if (x == (blankRoomPosition + blankSize) - 1 && y == blankRoomPosition)
                {
                    cells[x, y].DestroyWall(0);
                    cells[x, y].DestroyWall(3);
                }
                else if (x == (blankRoomPosition + blankSize) - 1 && y > blankRoomPosition && y <= (blankRoomPosition + blankSize) - 2)
                {
                    cells[x, y].DestroyWall(0);
                    cells[x, y].DestroyWall(2);
                    cells[x, y].DestroyWall(3);
                }
                else if (x == (blankRoomPosition + blankSize) - 1 && y == (blankRoomPosition + blankSize) - 1)
                {
                    cells[x, y].DestroyWall(2);
                    cells[x, y].DestroyWall(3);
                }
            }
        }
        busy = false;
    }

    private bool RouteAvailable(int row, int column)
    {
        int num = 0;
        if (row > 0 && !cells[row - 1, column].visited)
        {
            num++;
        }
        else if (row < sizes[currentSize] - 1 && !cells[row + 1, column].visited)
        {
            num++;
        }
        else if (column > 0 && !cells[row, column - 1].visited)
        {
            num++;
        }
        else if (column < sizes[currentSize] - 1 && !cells[row, column + 1].visited)
        {
            num++;
        }
        return num > 0;
    }

    private bool CellAvailable(int row, int column)
    {
        return row >= 0 && row < sizes[currentSize] && column >= 0 && column < sizes[currentSize] && !cells[row, column].visited;
    }

    private bool CellNeighborsVisited(int row, int column)
    {
        int num = 0;
        if (row > 0 && cells[row - 1, column].visited)
        {
            num++;
        }
        else if (row < sizes[currentSize] - 1 && cells[row + 1, column].visited)
        {
            num++;
        }
        else if (column > 0 && cells[row, column - 1].visited)
        {
            num++;
        }
        else if (column < sizes[currentSize] - 1 && cells[row, column + 1].visited)
        {
            num++;
        }
        return num > 0;
    }
}
