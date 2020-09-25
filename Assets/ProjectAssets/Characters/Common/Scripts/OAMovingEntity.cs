using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovingEntity", menuName = "OceanAlien/MovingEntity", order = 1)]
public class OAMovingEntity : ScriptableObject
{
    public float movementSpeed;
    public float maxSpeed;
    public float jumpForce;
}
