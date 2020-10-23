using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OAPlayerSwimUpdater : MonoBehaviour
{
    [Tooltip("Player reference")]
    [SerializeField]
    private OAPlayerStateStore playerState;

    private Collider2D col;

    void Start()
    {
        OAExtentions.AssertObjectNotNull(playerState, "Ocean is missing player reference");

        // Assure that the object has a trigger collider 
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        playerState.SetIsUnderWater(true);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        playerState.SetIsUnderWater(false);
    }
}
