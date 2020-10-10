using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Register event when user holds build button
// TODO: Change text based on bound key to built button
public class OABuildingArea : MonoBehaviour
{
    [Tooltip("Child object that holds canvas for text")]
    [SerializeField]
    private GameObject textObject = null;

    [SerializeField]
    private LayerMask playerMask = -1;

    private float builtButtonHold;
    private bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {
        OAExtentions.AssertObjectNotNull(textObject, "OABuildingArea missing textObject");

        if (playerMask.value <= 0)
            playerMask = LayerMask.GetMask("Player");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer != playerMask)
            return;

        textObject.SetActive(true);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer != playerMask)
            return;

        // if (Input.GetKey())
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer != playerMask)
            return;

        textObject.SetActive(false);
    }
}
