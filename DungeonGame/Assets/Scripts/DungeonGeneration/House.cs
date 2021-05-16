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
    
    private void GenerateDoors()
    {
        
    }
}