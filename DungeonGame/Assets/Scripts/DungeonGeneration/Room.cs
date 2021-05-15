using System;
using System.Collections;
using UnityEngine;

public class Room
{
    GenerationSettings settings;
    Cell[,] map;
    Cell[,] cells;
    public Room(Maze maze, GenerationSettings settings)
    {
        this.map = maze.Map;
        this.settings = settings;
    }

    public bool TryCreateRoom()
    {
        System.Random random = new System.Random();
        int roomStartPosX = random.Next(0, settings.Size);
        int roomStartPosY = random.Next(0, settings.Size);

        if (!map[roomStartPosX, roomStartPosY].IsRoomCell)
        {
            for (int i = 0; i < (settings.MaxRoomSize.x + settings.MaxRoomSize.x) / 2; i++)
            {
                int rHeight = random.Next((int)settings.MinRoomSize.x, (int)settings.MaxRoomSize.x);//vertical length
                int rWidth = random.Next((int)settings.MinRoomSize.y, (int)settings.MaxRoomSize.y);//horizontal length


                if (RoomIsInMaze(roomStartPosX + rHeight, roomStartPosY + rWidth)
                        && RoomIsNotOverlapping(roomStartPosX, roomStartPosY, rHeight, rWidth))
                {
                    SetRoomCells(roomStartPosX, roomStartPosY, rHeight, rWidth);
                    UpdateRoomCellsWalls(roomStartPosX, roomStartPosY, rHeight, rWidth);
                    Debug.Log($"Created: Height = {rHeight} || Width = {rWidth}"
                                     + $"|| Random Start X = {roomStartPosX} || Random Start X = {roomStartPosY}");
                    return true;
                }
                else
                {
                    Debug.Log($"Failed: Height = {rHeight} || Width = {rWidth}"
                                     + $"|| Random Start X = {roomStartPosX} || Random Start X = {roomStartPosY}");
                }
            }
        }
        return false;
    }

    private bool RoomIsInMaze(int x, int z)
    {
        return x < settings.Size && z < settings.Size
        && x >= 0 && z >= 0;
    }

    private bool RoomIsNotOverlapping(int startX, int startY, int height, int width)
    {
        return map[startX, startY].IsRoomCell == false
                && map[startX + height, startY].IsRoomCell == false
                && map[startX, startY + width].IsRoomCell == false
                && map[startX + height, startY + width].IsRoomCell == false;
    }

    private void SetRoomCells(int startX, int startY, int height, int width)
    {
        for (int i = startX; i < startX + height; i++)
        {
            for (int j = startY; j < startY + width; j++)
            {
                map[i, j].IsRoomCell = true;
            }
        }
    }

    private void UpdateRoomCellsWalls(int startX, int startY, int height, int width)
    {
        for (int i = startX; i < startX + height; i++)
        {
            for (int j = startY; j < startX + height; j++)
            {
                map[i, j].UpdateWalls(map);
            }
        }
    }
}