using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField]
    [Range(1, 50)]
    private int width = 10;

    [SerializeField]
    [Range(1,50)]
    private int height = 10;

    [SerializeField]
    private float size = 1f;

    [SerializeField]
    private Transform wallPrefab = null;
    [SerializeField]
    private Transform floorPrefab = null;
    [SerializeField]
    private Transform compPrefab;
    [SerializeField]
    private Transform endPoint;
    

    void Start()
    {
        WallState[,] maze = MazeGenerator.Generate(width, height);
        Draw(maze);
    }

    private void Draw(WallState[,] maze)
    {
        var floor = Instantiate(floorPrefab, transform);
        floor.localScale = new Vector3(width, 1, height);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var cell = maze[i, j];
                var position = new Vector3(-width / 2 + i, 0, -height / 2 + j);

                if(cell.HasFlag(WallState.UP))
                {
                    var topWall = Instantiate(wallPrefab, transform) as Transform;
                    topWall.position = position + new Vector3(0, 0, size / 2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                    topWall.gameObject.layer = 8;
                }

                if (cell.HasFlag(WallState.LEFT))
                {
                    var leftWall = Instantiate(wallPrefab, transform) as Transform;
                    leftWall.position = position + new Vector3(-size / 2, 0, 0);
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                    leftWall.gameObject.layer = 8;
                }

                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.RIGHT))
                    {
                        Transform rightWall = Instantiate(wallPrefab, transform) as Transform;
                        rightWall.position = position + new Vector3(size / 2, 0, 0);
                        rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                        rightWall.gameObject.layer = 8;
                    }
                }

                if (j == 0)
                {
                    if (cell.HasFlag(WallState.DOWN))
                    {
                        Transform bottomWall = Instantiate(wallPrefab, transform) as Transform;
                        bottomWall.position = position + new Vector3(0, 0, -size / 2);
                        bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                        bottomWall.gameObject.layer = 8;
                    }
                }

                if (i == MazeGenerator.startWall.X && j == MazeGenerator.startWall.Y)
                {
                    Transform comp = Instantiate(compPrefab, transform) as Transform;
                    comp.position = position + new Vector3(-size / 2, 0, 0);
                    compPrefab = comp;
                }

                if (i == MazeGenerator.endWall.X && j == MazeGenerator.endWall.Y)
                {
                    Transform endPos = Instantiate(endPoint, transform);
                    endPos.position = position = position + new Vector3(size / 2, 0, 0);
                    endPoint = endPos;
                }
            }
        }
        Grid grid = GetComponent<Grid>();
        grid.vGridWorldSize.x = width + 1;
        grid.vGridWorldSize.y = height + 1;
        grid.StartPosition = compPrefab;
        grid.InitGrid();
        Pathfinding pathfinding = GetComponent<Pathfinding>();
        pathfinding.StartPosition = compPrefab;
        pathfinding.TargetPosition = endPoint;
        pathfinding.FindThePath();
    }
}
