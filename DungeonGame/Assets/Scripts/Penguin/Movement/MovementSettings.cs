using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement Settings", fileName = "Movement/Settings")]
public class MovementSettings : ScriptableObject
{
    public float moveSpeed;
    public float jumpHeight;
    public float sprintMultiplier;
    public float onlandGravity;
    public float swimmingGravity;
    public float mouseSensitivity;
    public Camera camera;
}