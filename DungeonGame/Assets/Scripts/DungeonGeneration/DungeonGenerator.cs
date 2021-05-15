using System;
using System.Collections;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField]
    GenerationSettings settings;
    [SerializeField]
    GameObject cellType;

    Maze maze;

    int positionOffset = 5;

    void Start()
    {
        StartCoroutine(GenerateMaze());
        GenerateDungeonFromMaze();
    }

    private IEnumerator GenerateMaze()
    {
        maze = new Maze(settings);
        yield return new WaitForSeconds(3);
    }

    private void GenerateDungeonFromMaze()
    {
        for (int i = 0; i < settings.Size; i++)
        {
            for (int j = 0; j < settings.Size; j++)
            {
                Vector3 spawnPos = new Vector3((i * positionOffset) - 0.5F * i, 0, (j * positionOffset));
                CopyCellValue(maze.Map[i, j]);    
                Instantiate(cellType, spawnPos, Quaternion.identity,this.transform);

            }
        }
    }

    private void CopyCellValue(Cell data)
    {
        Cell cellscrpit = cellType.GetComponent<Cell>();
        cellType.GetComponent<Cell>().AvailDirections = data.AvailDirections;
    }


}