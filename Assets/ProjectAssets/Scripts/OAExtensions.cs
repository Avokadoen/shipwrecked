using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OAExtentions
{
    // List
    // O(1) remove function
    // Very useful if you don't care about the order of the list
    public static void SwapRemoveAt<T>(this List<T> list, int index)
    {
        var lastIndex = list.Count - 1;
        list[index] = list[lastIndex];
        list.RemoveAt(lastIndex);
    }

    // TODO: make sure we don't hit ourself
    public static bool CheckIfGrounded(this Collider2D collider)
    {
        Vector2 pos = collider.bounds.center;
        Vector2 leftTestPos = new Vector2(pos.x - collider.bounds.extents.x, pos.y - collider.bounds.extents.y);
        var leftHit = Physics2D.Raycast(leftTestPos, Vector2.down, 0.01f);
        if (leftHit.collider)
        {
            return true;
        }

        Vector2 rightTestPos = new Vector2(pos.x + collider.bounds.extents.x, pos.y + collider.bounds.extents.y);
        var rightHit = Physics2D.Raycast(rightTestPos, Vector2.down, 0.01f);
        if (rightHit.collider)
        {
            return true;
        }

        return false;
    }

}
