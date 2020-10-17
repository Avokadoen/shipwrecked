using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUtilityState", menuName = "OceanAlien/PlayerUtilityState")]
public class OAPlayerUtilityState : ScriptableObject
{
    [Tooltip("How much input you need to consider it a input signal")]
    public float deadZone = 0.001f;

    [Tooltip("Flip the player sprite when the horizontal movement is positive")]
    public bool flipOnHorizontalPositive;

    [Tooltip("Modifier on speed in y direction when swimming")]
    public float verticalSwimModifier = 0.2f;

    [Tooltip("Modifier on speed in x direction when swimming")]
    public float horizontalSwimModifier = 4f;

    // Get set to hide it in inspector
    private bool isUnderWater = false;
    public bool IsUnderWater { get => isUnderWater; set => isUnderWater = value; }

    private bool isGrounded = false;
    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }

    private float horizontal = 0f;
    public float Horizontal { get => horizontal; set => horizontal = value; }

    private float vertical = 0f;
    public float Vertical { get => vertical; set => vertical = value; }
}
