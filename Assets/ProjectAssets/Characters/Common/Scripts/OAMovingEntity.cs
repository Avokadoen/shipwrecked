using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: rename OAMoveStats
[CreateAssetMenu(fileName = "MovingEntity", menuName = "OceanAlien/MovingEntity", order = 1)]
public class OAMovingEntity : ScriptableObject
{
    public float movementSpeed;
    public float maxSpeed;
    public float jumpForce;
}
