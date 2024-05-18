using System;
using UnityEditor;
using UnityEngine;

public class CornMazeBuilder : EditorWindow
{
    int currentSize;
    int[] sizes = new int[]
    {
        15,
        20,
        25,
        30,
        35,
        40,
        45,
        50
    };
    bool randomSeed = true;
    int seed;
    Material floor;
    Material wall;
    Sprite flagSprite;
    Sprite signSprite;
    float signChance = 0.1f;
    Transform parent;
    bool busy;
    MazeCell[,] cells;

    [MenuItem("CM Tools/Corn Maze Builder")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CornMazeBuilder));
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
        if (flagSprite == null && TheBCMHT_Helper.getCornMazeFlagSampleSprite() != null)
        {
            flagSprite = TheBCMHT_Helper.getCornMazeFlagSampleSprite();
        }
        if (signSprite == null && TheBCMHT_Helper.getCornMazeSignSampleSprite() != null)
        {
            signSprite = TheBCMHT_Helper.getCornMazeSignSampleSprite();
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
        flagSprite = (Sprite)EditorGUILayout.ObjectField("Flag sprite:", flagSprite, typeof(Sprite), false);
        signSprite = (Sprite)EditorGUILayout.ObjectField("Sign sprite:", signSprite, typeof(Sprite), false);
        signChance = EditorGUILayout.FloatField("Sign chance:", signChance);
        parent = (Transform)EditorGUILayout.ObjectField("Parent:", parent, typeof(Transform), true);
        if (!busy)
        {
            if (GUILayout.Button("Current size: " + (currentSize < sizes.Length ? sizes[currentSize] : 0)))
            {
                currentSize++;
                if (currentSize > sizes.Length - 1)
                {
                    currentSize = 0;
                }
            }
            if (GUILayout.Button("Build maze"))
            {
                if (randomSeed)
                {
                    this.seed = UnityEngine.Random.Range(-2147483648, 2147483647);
                }
                BuildMaze();
            }
        }
    }

    private void BuildMaze()
    {
        busy = true;
        UnityEngine.Random.InitState(this.seed);
        int _size = sizes[currentSize];
        cells = new MazeCell[_size, _size];
        GameObject mazeParentObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        DestroyImmediate(mazeParentObject.GetComponent<Collider>());
        DestroyImmediate(mazeParentObject.GetComponent<MeshRenderer>());
        DestroyImmediate(mazeParentObject.GetComponent<MeshFilter>());
        mazeParentObject.name = "Maze";
        Transform mazeParent = mazeParentObject.transform;
        mazeParent.parent = parent;
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                cells[x, y] = new MazeCell();
                cells[x, y].gridPosition = new Vector3(10f * x, 0f, 10f * y);
                cells[x, y].parent = mazeParent;
                cells[x, y].InitializeCell();
                if (x == _size - 1 && y == _size - 1 && TheBCMHT_Helper.getCornMazeFlag() != null)
                {
                    GameObject flag = Instantiate<GameObject>(TheBCMHT_Helper.getCornMazeFlag(), cells[x, y].gridPosition, Quaternion.identity, mazeParent);
                    SpriteRenderer flagRenderer = flag.GetComponentInChildren<SpriteRenderer>();
                    if (flagRenderer != null)
                    {
                        flagRenderer.sprite = flagSprite;
                    }
                }
                else if (!(x == 0 & y == 0) && !(x == _size - 1 & y == _size - 1) && TheBCMHT_Helper.getCornMazeSign() != null && UnityEngine.Random.value <= this.signChance)
                {
                    GameObject sign = Instantiate<GameObject>(TheBCMHT_Helper.getCornMazeSign(), cells[x, y].gridPosition, Quaternion.identity, mazeParent);
                    SpriteRenderer signRenderer = sign.GetComponentInChildren<SpriteRenderer>();
                    if (signRenderer != null)
                    {
                        signRenderer.sprite = signSprite;
                    }
                }
            }
        }
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                cells[x, y].RemoveCeiling();
                cells[x, y].ApplyMaterials(null, this.wall, this.floor);
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
