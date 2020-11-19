using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAOffsetUpdater : MonoBehaviour
{
    [Tooltip("How fast the ocean will scroll when the player moves")]
    [SerializeField]
    private float scrollModifer = 0.03f;

    [SerializeField]
    private Transform oceanTransform;

    // magic number to keep water still when moving in y 
    readonly private float heightModifier = 0.05f;

    DisplacementBehaviour displacement;
    Vector3 cStartPos;
    Vector3 oStartPos;

    // Start is called before the first frame update
    void Start()
    {
        displacement = Camera.main.GetComponent<DisplacementBehaviour>();

        cStartPos = displacement.transform.position;
        oStartPos = oceanTransform.position;
    }

    private void FixedUpdate()
    {
        var cCurrentPos = displacement.transform.position;
        var oCurrentYPos = oceanTransform.position.y;

        displacement._scrollOffset = (cStartPos.x + cCurrentPos.x) * scrollModifer;
        displacement._heightOffset = ((cStartPos.y - cCurrentPos.y) - (oStartPos.y - oCurrentYPos)) * heightModifier; 
    }

}
