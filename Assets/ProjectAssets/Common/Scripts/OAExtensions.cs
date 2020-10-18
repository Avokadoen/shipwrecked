using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OAExtentions
{
    /// <summary>
    /// Removes the element at index O(1). Does not maintain order
    /// </summary>
    /// <param name="list"></param>
    /// <param name="index">Target index</param>
    public static void SwapRemoveAt<T>(this List<T> list, int index)
    {
        var lastIndex = list.Count - 1;
        list[index] = list[lastIndex];
        list.RemoveAt(lastIndex);
    }

    public static bool isGrounded(this Collider2D collider, LayerMask self, float distance)
    {
        LayerMask mask = Physics2D.GetLayerCollisionMask(self);

        Vector2 pos = collider.bounds.center;
        var rayYPos = pos.y - collider.bounds.extents.y;
        Vector2 leftTestPos = new Vector2(pos.x - collider.bounds.extents.x, rayYPos);
        var leftHit = Physics2D.Raycast(leftTestPos, Vector2.down, distance, mask);
        if (leftHit.collider)
        {
            return true;
        }

        Vector2 rightTestPos = new Vector2(pos.x + collider.bounds.extents.x, rayYPos);
        var rightHit = Physics2D.Raycast(rightTestPos, Vector2.down, distance, mask);
        if (rightHit.collider)
        {
            return true;
        }

        return false;
    }

    public static Vector3 Rotate2D(this Vector3 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return new Vector3((cos * tx) - (sin * ty), (sin * tx) + (cos * ty), v.z);
    }

    // TODO: move this?
    public static void AssertObjectNotNull<T>(T obj, string message) where T : class
    {
        #if UNITY_EDITOR
        if (obj == null) // TODO: find out why I can't do !obj
        {
            Debug.LogError(message);
            UnityEditor.EditorApplication.isPlaying = false;
        }
        #endif
    }


}
