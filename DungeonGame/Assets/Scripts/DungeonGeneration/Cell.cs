using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions { N, E, S, W };
public enum OppositeDirections { S, W, N, E }
public class Cell : MonoBehaviour
{
    private Vector2 position;
    private bool isVisited;
    private bool isRoom;
    [SerializeField]
    private bool[] availDirections;//NESW
    [SerializeField]
    private bool[] availWalls;//NESW
    private bool closeToRoom;
    private bool isDoor;
    [SerializeField]
    private bool isRoomCell;
    private bool triedCreateHere;

    public Cell(Vector2 position)
    {
        this.position = position;
        isRoomCell = false;
        isDoor = false;
        triedCreateHere = false;
        closeToRoom = false;
        isVisited = false;
        availDirections = new bool[4] { true, true, true, true };
        availWalls = new bool[4] { true, true, true, true };

    }

    public Cell()
    {
    }

    //The cell will only have to render the available walls
    //when its data has been processed
    void Start()
    {
        DestroyUnavailableWalls();
    }

    public void DestroyUnavailableWalls()
    {

        for (int i = 0; i < 4; i++)
            if (availWalls[i] == false)
                DestroyWallOnIndex(i);
    }

    private void DestroyWallOnIndex(int i)
    {
        switch (i)
        {
            case 0:
                DestroyWall("North");
                break;
            case 1:
                DestroyWall("East");
                break;
            case 2:
                DestroyWall("South");
                break;
            case 3:
                DestroyWall("West");
                break;
            default:
                Debug.Log("Unavailable Wall");
                break;
        }
    }


    public void DestroyWall(string direction)
    {
        var wall = transform.Find(direction);
        Destroy(wall.gameObject);
    }

    public void UpdateAvailableWalls(Cell[,] map)
    {
        if (position.x >= 0 && position.x < Math.Sqrt(map.Length) - 1)//Xet
        {
            if (position.x > 0 && map[(int)position.x - 1, (int)position.y].isRoomCell == true)
            {
                availWalls[0] = false;
            }

            if (map[(int)position.x + 1, (int)position.y].isRoomCell == true)
            {
                availWalls[2] = false;
            }
        }
        if (position.y >= 0 && position.y < Math.Sqrt(map.Length) - 1)
        {
            if (map[(int)position.x, (int)position.y + 1].isRoomCell == true)
            {
                availWalls[1] = false;
            }

            if (position.y > 0 && map[(int)position.x, (int)position.y - 1].isRoomCell == true)
            {
                availWalls[3] = false;
            }
        }
    }



    public Vector2 Position { get => position; set => position = value; }
    public bool IsVisited { get => isVisited; set => isVisited = value; }
    public bool IsRoom { get => isRoom; set => isRoom = value; }
    public bool[] AvailDirections { get => availDirections; set => availDirections = value; }
    public bool CloseToRoom { get => closeToRoom; set => closeToRoom = value; }
    public bool IsDoor { get => isDoor; set => isDoor = value; }
    public bool IsRoomCell { get => isRoomCell; set => isRoomCell = value; }
    public bool[] AvailWalls { get => availWalls; set => availWalls = value; }
    public bool TriedCreateHere { get => triedCreateHere; set => triedCreateHere = value; }
}