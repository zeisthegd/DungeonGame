using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour
{
    public static string[] Directions = new string[] { "North", "East", "South", "West" };
    [SerializeField]
    GameObject door;
    private Vector2 position;
    private bool isVisited;
    private bool[] availDirections;//NESW
    [SerializeField]
    private bool[] availWalls;//NESW
    private bool closeToRoom;
    [SerializeField]
    private bool isDoor;
    [SerializeField]
    private bool isRoomCell;
    private bool triedCreateHere;



    public Cell()
    {
        position = new Vector2(-1, -1);
    }

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

    //The cell will only have to render the available walls
    //when its data has been processed
    void Start()
    {
        BuildCell();
    }

    public void BuildCell()
    {
        for (int i = 0; i < 4; i++)
            BuildWall(i);

    }

    private void BuildWall(int direction)
    {
        DestroyWall(direction, availWalls[direction]);
        CreateDoor(direction);
    }


    public void DestroyWall(int direction, bool condition)
    {
        if (condition == false)
        {
            var wall = transform.Find(Directions[direction]);
            Destroy(wall.gameObject);
        }

    }

    private void CreateDoor(int direction)
    {
        if (isDoor && availWalls[direction] == true)
        {
            // var wall = transform.Find(Directions[direction]);
            // Instantiate(door, wall.transform.position, Quaternion.identity);
            Debug.Log(availWalls[direction]);
            DestroyWall(direction, false);
        }
    }

    public void UpdateAvailableWalls(Cell[,] map)
    {
        int mapLength = (int)Math.Sqrt(map.Length);
        if (position.x > 0 && map[(int)position.x - 1, (int)position.y].isRoomCell == true)
        {
            availWalls[0] = false;
        }

        if (position.x < mapLength - 1 && map[(int)position.x + 1, (int)position.y].isRoomCell == true)
        {
            availWalls[2] = false;
        }
        if (position.y < mapLength - 1 && map[(int)position.x, (int)position.y + 1].isRoomCell == true)
        {
            availWalls[1] = false;
        }

        if (position.y > 0 && map[(int)position.x, (int)position.y - 1].isRoomCell == true)
        {
            availWalls[3] = false;
        }
    }



    public Vector2 Position { get => position; set => position = value; }
    public bool IsVisited { get => isVisited; set => isVisited = value; }
    public bool[] AvailDirections { get => availDirections; set => availDirections = value; }
    public bool CloseToRoom { get => closeToRoom; set => closeToRoom = value; }
    public bool IsDoor { get => isDoor; set => isDoor = value; }
    public bool IsRoomCell { get => isRoomCell; set => isRoomCell = value; }
    public bool[] AvailWalls { get => availWalls; set => availWalls = value; }
    public bool TriedCreateHere { get => triedCreateHere; set => triedCreateHere = value; }
}