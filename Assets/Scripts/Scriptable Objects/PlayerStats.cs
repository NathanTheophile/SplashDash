using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float moveSpeed = 5;
    public float friction = 2;
    public float maxMoveSpeed = 5;
    public float rotationSpeed = 150;
    public float jumpForce = 10;
    public float jumpCooldown = 3;
}
