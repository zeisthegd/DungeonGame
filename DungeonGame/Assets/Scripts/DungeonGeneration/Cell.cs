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
    private bool closeToRoom;
    private bool isDoor;
    private bool isRoomCell;



    
    public Cell(Vector2 position)
    {
        this.position = position;
        isRoomCell = false;
        isDoor = false;
        closeToRoom = false;
        isVisited = false;
        availDirections = new bool[4] { true, true, true, true };
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
            if (availDirections[i] == false)
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

    public void UpdateWalls(Cell[,] map)
    {
        if (isRoomCell)
        {
            if (position.x > 0 && position.x < map.Length)//Xet
            {
                if (map[(int)position.x - 1, (int)position.y].isRoomCell == true)
                {
                    availDirections[0] = false;
                }
                if (map[(int)position.x + 1, (int)position.y].isRoomCell == true)
                {
                    availDirections[2] = false;
                }
            }
            if (position.y > 0 && position.y < map.Length)
            {
                if (map[(int)position.x, (int)position.y + 1].isRoomCell == true)
                {
                    availDirections[1] = false;
                }
                if (map[(int)position.x, (int)position.y - 1].isRoomCell == true)
                {
                    availDirections[3] = false;
                }
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
}