using System;
using System.Collections;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    GenerationSettings settings;
    [SerializeField]
    GameObject cellType;
    [SerializeField]
    bool toGenerate = false;

    Maze maze;

    int positionOffset = 5;

    void Awake()
    {
        if (toGenerate)
        {
            StartCoroutine(InitializeMaze());
            StartCoroutine(GenerateCorridors());
        }

    }
    void Start()
    {
        if (toGenerate)
        {
            GenerateDungeonFromMaze();
        }
    }

    private IEnumerator InitializeMaze()
    {
        maze = new Maze(settings);
        yield return new WaitForSeconds(4);
    }

    public IEnumerator GenerateCorridors()
    {
        maze.RunMazeAlgorithm();
        yield return new WaitForSeconds(6);
    }

    private void GenerateDungeonFromMaze()
    {
        for (int i = 0; i < settings.Size; i++)
        {
            for (int j = 0; j < settings.Size; j++)
            {
                Vector3 spawnPos = new Vector3((i * positionOffset) - 0.5F * i, 0, (j * positionOffset));
                CopyCellValue(maze.Map[i, j]);
                cellType.name = $"Cell[{i},{j}]";
                Instantiate(cellType, spawnPos, Quaternion.identity, this.transform);

            }
        }
    }

    private void CopyCellValue(Cell data)
    {
        Cell cellscrpit = cellType.GetComponent<Cell>();
        cellscrpit.IsRoomCell = data.IsRoomCell;
        cellscrpit.AvailDirections = data.AvailDirections;
        cellscrpit.AvailWalls = data.AvailWalls;
        cellscrpit.IsDoor = data.IsDoor;

    }

    public void GenerateDungeon()
    {
        Cell[] cells = FindObjectsOfType<Cell>();
        foreach (Cell cell in cells)
            Destroy(cell.gameObject);
        StartCoroutine(InitializeMaze());
        StartCoroutine(GenerateCorridors());
        GenerateDungeonFromMaze();
    }


}