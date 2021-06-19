using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Process the data before instantiate the cells
public class Maze
{
    House house;
    private Cell[,] map;
    GenerationSettings settings;
    int roomCreated = 0;

    public Maze(GenerationSettings settings)
    {
        house = new House();
        this.settings = settings;
        InitCells();
        InitRooms();
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

    private void InitRooms()
    {
        BruteForceSomeRooms();
        if (roomCreated < settings.MinRooms)
        {
            ClearTriedAttempts();
            InitRooms();
        }
    }

    #region Room Generation
    private void BruteForceSomeRooms()
    {
        for (int i = 0; i < 5000 && roomCreated <= settings.MaxRooms; i++)
        {
            Room room = new Room(this, settings);
            if (room.TryCreateRoom())
            {
                house.AddRoom(room);
                roomCreated++;
            }
        }
        house.GenerateRooms();
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

                if (i > 0)
                    map[i - 1, j].CloseToRoom = true;
                if (i < settings.Size - 1)
                    map[i + 1, j].CloseToRoom = true;
                if (j < settings.Size - 1)
                    map[i, j + 1].CloseToRoom = true;
                if (j > 0)
                    map[i, j - 1].CloseToRoom = true;

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

    #endregion

    #region Maze Generation

    public void GenerateCorridors()
    {
        System.Random random = new System.Random();
        List<Cell> corCells = GetNonRoomCells();
        Cell startCell = corCells[random.Next(0, corCells.Count)];
        RunMazeAlgorithm(startCell);
    }

    private void RunMazeAlgorithm(Cell cell)
    {
        System.Random random = new System.Random();
        cell.IsVisited = true;
        List<Cell> neighbors = GetNeighborCellsOf(cell);
        for (int i = 0; i < neighbors.Count; i++)
        {
            Cell nextCellInPath = neighbors[random.Next(0, neighbors.Count)];
            DeleteWallInBetween(cell, nextCellInPath);
            RunMazeAlgorithm(nextCellInPath);
            neighbors.Remove(nextCellInPath);
        }
    }

    private List<Cell> GetNonRoomCells()
    {
        List<Cell> cells = new List<Cell>();
        foreach (Cell cell in map)
        {
            if (cell.IsRoomCell == false)
                cells.Add(cell);
        }
        return cells;
    }

    private List<Cell> GetNeighborCellsOf(Cell cell)
    {
        List<Cell> cells = new List<Cell>();
        for (int i = 0; i < 4; i++)
        {
            Cell neighbor = GetCellAtDirection(cell, i);
            if (!neighbor.Position.Equals(new Vector2(-1, -1)))
            {
                if (!neighbor.IsVisited && !neighbor.IsRoomCell)
                    cells.Add(neighbor);
                else if (neighbor.IsDoor)
                    DeleteWallInBetween(cell, neighbor);
            }
        }
        return cells;
    }

    private Cell GetCellAtDirection(Cell cell, int direction)
    {
        Vector2 currentCellPos = cell.Position;
        Cell cellAtDir = new Cell();
        if (cell.Position.x > 0 && direction == 0)
            cellAtDir = map[(int)currentCellPos.x - 1, (int)currentCellPos.y];

        else if (cell.Position.x < settings.Size - 1 && direction == 1)
            cellAtDir = map[(int)currentCellPos.x + 1, (int)currentCellPos.y];

        else if (cell.Position.y < settings.Size - 1 && direction == 2)
            cellAtDir = map[(int)currentCellPos.x, (int)currentCellPos.y + 1];

        else if (cell.Position.y > 0 && direction == 3)
            cellAtDir = map[(int)currentCellPos.x, (int)currentCellPos.y - 1];
        return cellAtDir;
    }

    private void DeleteWallInBetween(Cell a, Cell b)
    {
        bool[] wallsToDel = new bool[] { false, false, false, false };

        wallsToDel[0] = !(a.Position.x > b.Position.x);//N to lose | a is below b
        wallsToDel[1] = !(a.Position.y < b.Position.y);//E to lose | a is on the left of b     
        wallsToDel[2] = !(a.Position.x < b.Position.x);//S to lose | a is above b
        wallsToDel[3] = !(a.Position.y > b.Position.y);//W to lose | a is on the right of b

        for (int i = 0; i < 4; i++)
        {
            if (wallsToDel[i] == false)
            {
                int oppDir = (i % 2 == 0) ? ((i == 0) ? 2 : 0) : ((4 - i) % 4);
                a.AvailWalls[i] = false;
                b.AvailWalls[oppDir] = false;
                return;
            }

        }
    }

    public void SetDoorCell(int x, int y)
    {
        map[x, y].IsDoor = true;
        DeleteWallInBetween(map[x, y], map[x + 1, y]);
        DeleteWallInBetween(map[x, y], map[x - 1, y]);
        DeleteWallInBetween(map[x, y], map[x, y + 1]);
        DeleteWallInBetween(map[x, y], map[x, y - 1]);
    }

    #endregion
    private void ClearTriedAttempts()
    {
        foreach (Cell cell in map)
        {
            if (cell.IsRoomCell == false)
                cell.TriedCreateHere = false;
        }
        house = new House();
    }


    public Cell[,] Map { get => map; }
}


