using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A house consists of some rooms. To easily mangage the generation of rooms
public class House
{
    List<Room> rooms = new List<Room>();

    public House()
    {
    }

    public House(List<Room> rooms)
    {
        this.rooms = rooms;
    }

    public void GenerateRooms()
    {
        GenerateDoors();
    }
    
    private void GenerateDoors()
    {
        foreach (Room room in rooms)
        {
            room.CreateDoor();
        }
    }

    public void AddRoom(Room room)
    {
        rooms.Add(room);
    }

    
}