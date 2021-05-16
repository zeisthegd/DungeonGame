using System;
using System.Collections;
using UnityEngine;

public class Room
{
    GenerationSettings settings;
    Maze maze;
    Cell[,] cells;
    public Room(Maze maze, GenerationSettings settings)
    {
        this.maze = maze;
        this.settings = settings;
    }

    public bool TryCreateRoom()
    {
        System.Random random = new System.Random();
        int roomStartPosX = random.Next(0, settings.Size);
        int roomStartPosY = random.Next(0, settings.Size);

        if (maze.Map[roomStartPosX, roomStartPosY].TriedCreateHere == false
            && maze.Map[roomStartPosX, roomStartPosY].IsRoomCell == false)
        {
            for (int i = 0; i < settings.MaxRoomTries; i++)
            {
                int rHeight = random.Next((int)settings.MinRoomSize.x, (int)settings.MaxRoomSize.x);//vertical length
                int rWidth = random.Next((int)settings.MinRoomSize.y, (int)settings.MaxRoomSize.y);//horizontal length

                int bottomX = roomStartPosX + rHeight;
                int rightY = roomStartPosY + rWidth;

                if (RoomIsInMaze(bottomX, rightY)
                        && maze.RoomIsNotOverlapping(roomStartPosX, roomStartPosY, bottomX, rightY)
                            && maze.RoomIsNotAdjacentToAnother(roomStartPosX, roomStartPosY, bottomX, rightY))
                {
                    maze.SetRoomCells(roomStartPosX, roomStartPosY, bottomX, rightY);
                    maze.UpdateRoomCellsWalls(roomStartPosX, roomStartPosY, bottomX, rightY);
                    maze.Map[roomStartPosX, roomStartPosY].TriedCreateHere = true;
                    Debug.Log($"Room: {roomStartPosX}|{roomStartPosY}|{bottomX}|{rightY}");
                    return true;    
                }
            }
            maze.Map[roomStartPosX, roomStartPosY].TriedCreateHere = true;
        }
        return false;
    }

    private bool RoomIsInMaze(int x, int z)
    {
        bool isInMaze = x < settings.Size && z < settings.Size
            && x >= 0 && z >= 0;
        return isInMaze;
    }


}