using System;
using System.Collections;
using UnityEngine;

//Process the data before instantiate the cells
public class Maze
{
    private Cell[,] map;
    GenerationSettings settings;
    int roomCreated = 0;

    public Maze(GenerationSettings settings)
    {
        this.settings = settings;
        InitCells();
        BruteForceSomeRooms();
    }

    private void InitCells()
    {
        map = new Cell[settings.Size, settings.Size];
        for (int i = 0; i < settings.Size; i++)
        {
            for (int j = 0; j < settings.Size; j++)
            {
                Vector2 position = new Vector2(i, j);
                map[i, j] = new Cell(position);
            }
        }
    }

    private void BruteForceSomeRooms()
    {
        while (roomCreated < settings.MaxRooms)
        {
            Room room = new Room(this, settings);
            if (room.TryCreateRoom())
                roomCreated++;
        }

    }
    public Cell[,] Map { get => map; }
}


