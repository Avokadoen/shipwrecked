using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OASpriteGroupFlipper : MonoBehaviour
{
    [SerializeField]
    private float flipOffset = 1f;

    private bool previousFlip = false;

    // TODO: refactor this function!
    public void FlipX(bool flip)
    {
        Vector3 localScale = transform.localScale;
        Vector3 position = transform.position;

        if (flip)
        {
            position.x += flipOffset;
            localScale.x = -1;
            transform.localScale = localScale;
        }
        else
        {
            position.x -= flipOffset;
            localScale.x = 1;
            transform.localScale = localScale;
        }

        if (flip != previousFlip)
        {
            transform.position = position;
        }

        previousFlip = flip;
    }
}
