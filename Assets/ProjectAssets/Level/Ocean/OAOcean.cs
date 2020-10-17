using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAOcean : MonoBehaviour
{
    [Tooltip("Player reference")]
    [SerializeField]
    private OAPlayerMovement playerMov;

    [Tooltip("How high the ocean will be at max")]
    [SerializeField]
    private float maxHeigth;

    [Tooltip("How wide the ocean will be at all times")]
    [SerializeField]
    private float wide;

    // Start is called before the first frame update
    void Start()
    {
        OAExtentions.AssertObjectNotNull(playerMov, "Ocean is missing player reference");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        playerMov.SetUnderWater(true);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        playerMov.SetUnderWater(false);
    }
}
