using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName ="Settings",menuName ="Dungeon/Generation/Settings")]
public class GenerationSettings : ScriptableObject
{
    public int Size;
    public int MaxRoomTries;
    public int MaxRooms;
    public Vector2 MinRoomSize;
    public Vector2 MaxRoomSize;

    public int RoomBorderLeniancy;
    public int RoomBorderRetries;

    
    
}