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

    public bool RoomIsNotAdjacentToAnother(int startX, int startY, int bottomX, int rightY)
    {
        return (startX > 0 && startY > 0 && bottomX < settings.Size - 1 && rightY < settings.Size - 1)
                //CheckTopLeftCorner
                && map[startX - 1, startY - 1].CloseToRoom == false
                && map[startX, startY - 1].CloseToRoom == false
                && map[startX - 1, startY].CloseToRoom == false
                //CheckBottomLeftCorner
                && map[bottomX + 1, startY].CloseToRoom == false
                && map[bottomX, startY - 1].CloseToRoom == false
                && map[bottomX + 1, startY - 1].CloseToRoom == false
                //CheckTopRightCorner
                && map[startX, rightY + 1].CloseToRoom == false
                && map[startX - 1, rightY].CloseToRoom == false
                && map[startX - 1, rightY + 1].CloseToRoom == false
                //CheckTopRightCorner
                && map[bottomX, rightY + 1].CloseToRoom == false
                && map[bottomX + 1, rightY].CloseToRoom == false
                && map[bottomX + 1, rightY + 1].CloseToRoom == false;
    }

    public bool RoomIsNotOverlapping(int startX, int startY, int bottomX, int rightY)
    {
        for (int i = startX; i <= bottomX; i++)
        {
            for (int j = startY; j <= rightY; j++)
            {
                if (map[i, j].IsRoomCell)
                    return false;
            }
        }
        return true;
    }

    public void SetRoomCells(int startX, int startY, int bottomX, int rightY)
    {
        for (int i = startX; i <= bottomX; i++)
        {
            for (int j = startY; j <= rightY; j++)
            {
                map[i, j].IsRoomCell = true;

                if (i > 0 && j > 0 && j < settings.Size - 1 && i < settings.Size - 1)
                {
                    map[i - 1, j].CloseToRoom = (i == startX);
                    map[i + 1, j].CloseToRoom = (i == bottomX);
                    map[i, j + 1].CloseToRoom = (j == rightY);
                    map[i, j - 1].CloseToRoom = (j == startY);
                }
            }
        }
    }

    public void UpdateRoomCellsWalls(int startX, int startY, int bottomX, int rightY)
    {
        for (int i = startX; i <= bottomX; i++)
        {
            for (int j = startY; j <= rightY; j++)
            {
                map[i, j].UpdateAvailableWalls(map);
            }
        }
    }

    public void SetCloseToRoomCells()
    {

    }
    public Cell[,] Map { get => map; }
}


