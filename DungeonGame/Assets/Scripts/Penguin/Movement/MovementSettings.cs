using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement Settings", fileName = "Movement/Settings")]
public class MovementSettings : ScriptableObject
{
    public float moveSpeed;
    public float jumpHeight;
    public float sprintMultiplier;
    public float turnSmooth;
    public float turnSmoothVelocity;
    public float onlandGravity;
    public float aerialGravity;
    public float swimmingGravity;
    public Camera camera;
}